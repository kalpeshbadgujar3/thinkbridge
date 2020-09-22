using Helper;
using System.Threading.Tasks;
using System.Web;

namespace MasterInterface
{
    public interface IItemManager
    {
        Task<string> GetAllItems();

        Task<bool> AddItem(IspInsertItem ispInsertItem);

        Task<bool> UploadImage(HttpPostedFile postedFile);
    }
}
