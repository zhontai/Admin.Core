
namespace Admin.Core.Service.Admin.Document.Input
{
    public class DocumentUpdateGroupInput : DocumentAddGroupInput
    {
        /// <summary>
        /// ±àºÅ
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// °æ±¾
        /// </summary>
        public long Version { get; set; }
    }
}
