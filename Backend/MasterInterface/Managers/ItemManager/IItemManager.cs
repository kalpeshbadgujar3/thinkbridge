using Helper;
using System.Threading.Tasks;

namespace MasterInterface
{
    public interface IItemManager
    {
        Task<string> GetAllItems();
    }
}
