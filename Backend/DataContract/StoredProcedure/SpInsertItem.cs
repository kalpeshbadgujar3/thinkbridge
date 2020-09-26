using MasterInterface;

namespace DataContract
{
    public class SpInsertItem : IspInsertItem
    {
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public decimal ItemPrice { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
    }
}
