
namespace Admin.Core.Service.Admin.Permission.Input
{
    public class PermissionUpdateMenuInput : PermissionAddMenuInput
    {
        /// <summary>
        /// È¨ÏÞId
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// °æ±¾
        /// </summary>
        public long Version { get; set; }
    }
}
