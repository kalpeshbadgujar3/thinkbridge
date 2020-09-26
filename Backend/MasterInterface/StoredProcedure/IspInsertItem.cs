namespace MasterInterface
{
    public interface IspInsertItem : IStoredProcedure
    {
        string ItemName { get; set; }
        string ItemDescription { get; set; }
        decimal ItemPrice { get; set; }
        string FileName { get; set; }
        string FileExtension { get; set; }
    }
}
