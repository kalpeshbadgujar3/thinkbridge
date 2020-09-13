using Helper;
using System.Threading.Tasks;

namespace MasterInterface
{
    public interface IDatabaseManager
    {
        Task<string> GetAllItems(StoredProcedure nameOfStoredProcedure);
    }
}
