using Helper;
using MasterInterface;
using System.Threading.Tasks;

namespace Managers
{
    public class ItemManager : IItemManager
    {
        IDatabaseManager _databaseManager;

        public ItemManager(IDatabaseManager databaseManager)
        {
            _databaseManager = databaseManager;
        }

        public async Task<string> GetAllItems()
        {
            return await _databaseManager.GetAllItems(StoredProcedure.spGetAllItems);
        }
    }
}
