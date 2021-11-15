namespace ZhonTai.Plate.Admin.Service.Permission.Dto
{
    public class PermissionUpdateGroupInput : PermissionAddGroupInput
    {
        /// <summary>
        /// 权限Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public long Version { get; set; }
    }
}