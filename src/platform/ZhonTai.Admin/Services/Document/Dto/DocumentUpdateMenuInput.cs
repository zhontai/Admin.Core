namespace ZhonTai.Admin.Services.Document.Dto
{
    public class DocumentUpdateMenuInput : DocumentAddMenuInput
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