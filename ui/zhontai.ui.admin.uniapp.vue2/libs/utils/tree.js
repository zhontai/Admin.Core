export function listToTree(list = [], rootWhere, childsWhere, addChilds, data) {
	let tree = []
	//空列表
	if (!(list?.length > 0))
	{
	  return tree
	}
	
	//顶级
	let rootList = list.filter(item => rootWhere && rootWhere(data, item))
	if (!(rootList?.length > 0))
	{
	  return tree
	}
	tree = tree.concat(rootList)
	
	//子级
	tree.forEach(root => {
		let rootChildList = list.filter(item => childsWhere && childsWhere(root, item))
		if (!(rootChildList?.length > 0))
		{
			return
		}
		rootChildList.forEach(item => {
			let childList = listToTree(list, childsWhere, childsWhere, addChilds, item)
			addChilds && addChilds(item, childList)
		})
		addChilds && addChilds(root, rootChildList)
	})
	
	return tree
}

export function addListWithChilds(list = [], data, getChilds){
	let childs = getChilds(data)
	if (childs?.length > 0)
	{
	  list = list.concat(childs)
	  childs.forEach(child => {
	  	addListWithChilds(list, child, getChilds)
	  }) 
	}
	return list
}

export function treeToList(tree = [], getChilds) {
	let list = []
	//空树
	if (!(tree?.length > 0))
	{
	  return list
	}
	
	tree.forEach(root => {
		list = addListWithChilds(list, root, getChilds)
		list.push(root)
	})
	return list
}
/**
* @description: 获得所有父级
* @example 
const parents = getParents(items, self, (item, self) => { 
	return item.id === self.parentId 
})
*/
export function getParents(list = [], self, parentWhere, parents = []) {
	//空列表
	if (!(list?.length > 0))
	{
	  return parents
	}
	
	if(!self){
		return parents
	}
	
	const parent = list.find(item => parentWhere && parentWhere(item, self))

	if(parent){
		parents.unshift(parent)
		getParents(list, parent, parentWhere, parents)
	}
	
	return parents
}

export default{
	toTree: listToTree,
	toList: treeToList,
	getParents
}