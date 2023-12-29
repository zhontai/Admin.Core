/**
* @description: 列表转树形列表
* @example
listToTree(cloneDeep(list))

listToTree(cloneDeep(list), {
  rootWhere: (parent, self) => {
    return self.parentId === 0
  },
  childsWhere: (parent, self) => {
    return parent.id === self.parentId
  },
  addChilds: (parent, childList) => {
    if (childList?.length > 0) {
      parent['children'] = childList
    }
  }
})
*/
export function listToTree(list: any = [], options = {}, data = null) {
  const { rootWhere, childsWhere, addChilds } = Object.assign(
    {
      rootWhere: (parent: any, self: any) => {
        return self.parentId === 0
      },
      childsWhere: (parent: any, self: any) => {
        return parent.id === self.parentId
      },
      addChilds: (parent: any, childList: any) => {
        if (childList?.length > 0) {
          parent['children'] = childList
        }
      },
    },
    options || {}
  )
  let tree = [] as any
  // 空列表
  if (!(list?.length > 0)) {
    return tree
  }

  // 顶级
  const rootList = list.filter((item: any) => rootWhere && rootWhere(data, item))
  if (!(rootList?.length > 0)) {
    return tree
  }
  tree = tree.concat(rootList)

  // 子级
  tree.forEach((root: any) => {
    const rootChildList = list.filter((item: any) => childsWhere && childsWhere(root, item))
    if (!(rootChildList?.length > 0)) {
      return
    }
    rootChildList.forEach((item: any) => {
      const childList = listToTree(list, { rootWhere: childsWhere, childsWhere, addChilds }, item)
      addChilds && addChilds(item, childList)
    })
    addChilds && addChilds(root, rootChildList)
  })

  return tree
}

/**
* @description: 将树形列表转换为扁平化数据列表
* @example
toFlatList(tree, (data) => { return data['children'] }, list)
*/
export function toFlatList(tree: any, getChilds: any, flatList: any = [], noChildren = true) {
  tree.forEach((item: any) => {
    flatList.push(item)
    const children = getChilds(item)
    if (children?.length > 0) {
      toFlatList(children, getChilds, flatList, noChildren)
    }
    if (noChildren) {
      delete item.children
    }
  })
}

/**
* @description: 树形列表转列表无子级
* @example
treeToList(cloneDeep(tree))

treeToList(cloneDeep(tree), (data) => { return data['children'] })
*/
export function treeToList(
  tree: any = [],
  getChilds = (data: any) => {
    return data['children']
  }
) {
  const list = [] as any
  // 空树
  if (!(tree?.length > 0)) {
    return list
  }

  toFlatList(tree, getChilds, list)

  return list
}

/**
* @description: 树形列表过滤父级或者子级数据
* @example
filterTree(cloneDeep(tree), keyword)

filterTree(cloneDeep(tree), keyword, {
  children: 'children',
  filterWhere: (item: any, filterword: string) => {
    return item.name?.toLocaleLowerCase().indexOf(filterword) > -1
  },
})
*/
export function filterTree(tree: any = [], keyword: string, options = {}) {
  const { children, filterWhere } = Object.assign(
    {
      children: 'children',
      filterWhere: (item: any, word: string) => {
        return item.name?.toLocaleLowerCase().indexOf(word) > -1
      },
    },
    options || {}
  )

  return tree.filter((item: any) => {
    if (filterWhere(item, keyword)) {
      return true
    }

    if (item[children]) {
      item[children] = filterTree(item[children], keyword, { children, filterWhere })
      return item[children].length > 0
    }

    return false
  })
}

/**
* @description: 树形列表转列表包含子级
* @example
treeToListWithChildren(cloneDeep(tree))

treeToListWithChildren(cloneDeep(tree), (data) => { return data['children'] })
*/
export function treeToListWithChildren(
  tree = [],
  getChilds = (data: any) => {
    return data['children']
  }
) {
  const list = [] as any
  // 空树
  if (!(tree?.length > 0)) {
    return list
  }

  toFlatList(tree, getChilds, list, false)

  return list
}

/**
* @description: 获得自身所有父级列表
* @example
getParents(cloneDeep(items), self)
getParents(treeToList(cloneDeep(items), self))

const parents = getParents(cloneDeep(items), self, (item, self) => {
	return item.id === self.parentId
})
*/
export function getParents(
  list = [],
  self: any,
  parentWhere = (item: any, self: any) => {
    return item.id === self.parentId
  },
  parents = []
) {
  // 空列表
  if (!(list?.length > 0)) {
    return parents
  }

  if (!self) {
    return parents
  }

  const parent = list.find((item) => parentWhere && parentWhere(item, self))

  if (parent) {
    parents.unshift(parent)
    getParents(list, parent, parentWhere, parents)
  }

  return parents
}

/**
* @description: 获得自身所有父级列表包含自身
* @example
getParentsAndSelf(cloneDeep(items), self)
getParentsAndSelf(treeToList(cloneDeep(items)), self)

const parents = getParentsAndSelf(cloneDeep(items), self, {
  selfWhere: (item, self) => {
    return item.id === self.id
  },
  parentWhere: (item, self) => {
    return item.id === self.parentId
  }
})
*/
export function getParentsAndSelf(list = [], self: any, options = {}) {
  const { selfWhere, parentWhere } = Object.assign(
    {
      selfWhere: (item: any, self: any) => {
        return item.id === self.id
      },
      parentWhere: (item: any, self: any) => {
        return item.id === self.parentId
      },
    },
    options || {}
  )

  const parents = getParents(list, self, parentWhere)
  const me = list.find((item: any) => selfWhere && selfWhere(item, self))
  if (me) {
    parents.unshift(me)
  }

  return parents
}

export default {
  toTree: listToTree,
  toList: treeToList,
  toListWithChildren: treeToListWithChildren,
  getParents,
  getParentsAndSelf,
}
