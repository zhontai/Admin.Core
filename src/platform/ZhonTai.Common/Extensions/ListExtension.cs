using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ZhonTai.Common.Extensions;

public static class ListExtension
{
    /// <summary>
    /// 将列表转换为树形结构
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="list">数据</param>
    /// <param name="rootWhere">根条件</param>
    /// <param name="childsWhere">节点条件</param>
    /// <param name="addChilds">添加子节点</param>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static List<T> ToTree<T>(this List<T> list, Func<T, T, bool> rootWhere, Func<T, T, bool> childsWhere, Action<T, IEnumerable<T>> addChilds, T entity = default)
    {
        var treelist = new List<T>();
        //空树
        if (list == null || list.Count == 0)
        {
            return treelist;
        }
        if (!list.Any(e => rootWhere(entity, e)))
        {
            return treelist;
        }

        //树根
        if (list.Any(e => rootWhere(entity, e)))
        {
            treelist.AddRange(list.Where(e => rootWhere(entity, e)));
        }

        //树叶
        foreach (var item in treelist)
        {
            if (list.Any(e => childsWhere(item, e)))
            {
                var nodedata = list.Where(e => childsWhere(item, e)).ToList();
                foreach (var child in nodedata)
                {
                    //添加子集
                    var data = list.ToTree(childsWhere, childsWhere, addChilds, child);
                    addChilds(child, data);
                }
                addChilds(item, nodedata);
            }
        }

        return treelist;
    }

    /// <summary>
    /// 添加子级列表到平级列表
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="list">平级列表</param>
    /// <param name="getChilds">获得子级列表的方法</param>
    /// <param name="entity">子级对象</param>
    public static void AddListWithChilds<T>(List<T> list, Func<T, List<T>> getChilds, T entity = default)
    {
        var childs = getChilds(entity);
        if (childs != null && childs.Count > 0)
        {
            list.AddRange(childs);
            foreach (var child in childs)
            {
                AddListWithChilds(list, getChilds, child);
            }
        }
    }

    /// <summary>
    /// 将树形列表转换为平级列表
    /// tree.ToPlainList((a) => a.Children);
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="tree">树形列表</param>
    /// <param name="getChilds">获得子级列表的方法</param>
    /// <param name="entity">数据对象</param>
    /// <returns></returns>
    public static List<T> ToPlainList<T>(this List<T> tree, Func<T, List<T>> getChilds, T entity = default)
    {
        var list = new List<T>();
        if (tree == null || tree.Count == 0)
        {
            return list;
        }

        foreach (var item in tree)
        {
            list.Add(item);
            AddListWithChilds(list, getChilds, item);
        }

        return list;
    }

    /// <summary>
    /// 深度克隆
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="list">列表</param>
    /// <returns></returns>
    public static List<T> Clone<T>(this List<T> list)
    {
        return JsonConvert.DeserializeObject<List<T>>(JsonConvert.SerializeObject(list));
    }
}