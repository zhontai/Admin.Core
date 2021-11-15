namespace ZhonTai.Plate.Admin.Service.Document.Dto
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