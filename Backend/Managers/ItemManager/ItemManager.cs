using Helper;
using MasterInterface;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

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
            return await _databaseManager.GetAllRecords(StoredProcedure.spGetAllItems);
        }

        public async Task<bool> AddItem(IspInsertItem spInsertItem)
        {
            return await _databaseManager.AddRecord(StoredProcedure.spInsertItem, spInsertItem);
        }

        public async Task<bool> UploadImage(HttpPostedFile postedFile)
        {
            var filePath = HttpContext.Current.Server.MapPath(string.Format("{0}{1}", "~", "/uploads/images/"));

            DirectoryInfo directory = new DirectoryInfo(filePath);
            if (!directory.Exists)
            {
                directory.Create();
            }

            //var files = new List<string>();
            //files.Add(filePath);

            postedFile.SaveAs(filePath);

            return true;
        }
    }
}
