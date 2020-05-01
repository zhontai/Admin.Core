
namespace Admin.Core.Service.Admin.Document.Input
{
    public class DocumentUpdateContentInput
    {
        /// <summary>
        /// ±àºÅ
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Ãû³Æ
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// ÄÚÈÝ
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Html
        /// </summary>
        public string Html { get; set; }

        /// <summary>
        /// °æ±¾
        /// </summary>
        public long Version { get; set; }
    }
}
