using Helper;
using System.Threading.Tasks;

namespace MasterInterface
{
    public interface IDatabaseManager
    {
        Task<string> GetAllRecords(StoredProcedure nameOfStoredProcedure);
        Task<int> AddRecord(StoredProcedure nameOfStoredProcedure, IStoredProcedure storedProcedureObject);
    }
}
