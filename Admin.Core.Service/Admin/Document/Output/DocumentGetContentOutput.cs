
using Admin.Core.Service.Admin.Document.Input;
using Newtonsoft.Json;

namespace Admin.Core.Service.Admin.Document.Output
{
    public class DocumentGetContentOutput
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
        /// °æ±¾
        /// </summary>
        public long Version { get; set; }
    }
}
