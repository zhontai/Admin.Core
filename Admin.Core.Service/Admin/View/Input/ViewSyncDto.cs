namespace Admin.Core.Service.Admin.View.Input
{
    public class ViewSyncDto
    {
        /// <summary>
        /// 视图命名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Path { get; set; }
        
        /// <summary>
        /// 父级路径
        /// </summary>
        public string ParentPath { get; set; }
    }
}
