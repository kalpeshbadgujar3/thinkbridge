namespace Helper
{
    /// <summary>
    /// Enum contains all the stored procedure names
    /// To ensure no sp name is hardcoded in application, if any sp renames; things can be handled with minimal changes
    /// </summary>
    public enum StoredProcedure
    {
        spGetAllItems,
        spInsertItem
    }
}
