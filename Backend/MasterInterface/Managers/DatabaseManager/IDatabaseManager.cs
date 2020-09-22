using Helper;
using System.Threading.Tasks;

namespace MasterInterface
{
    public interface IDatabaseManager
    {
        Task<string> GetAllRecords(StoredProcedure nameOfStoredProcedure);
        Task<bool> AddRecord(StoredProcedure nameOfStoredProcedure, IStoredProcedure storedProcedureObject);
    }
}
