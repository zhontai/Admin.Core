using System;
using System.Collections.Generic;
using System.Linq;

namespace Admin.Core.Common.Extensions
{
    public static class ListExtensions
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
    }
}