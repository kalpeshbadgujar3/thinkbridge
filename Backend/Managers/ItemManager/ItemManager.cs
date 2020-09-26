using Helper;
using MasterInterface;
using System.Collections.Generic;
using System.IO;
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

        /// <summary>
        /// Get All Items
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetAllItems()
        {
            return await _databaseManager.GetAllRecords(StoredProcedure.spGetAllItems);
        }

        /// <summary>
        /// Add item to DB
        /// </summary>
        /// <param name="spInsertItem"></param>
        /// <returns></returns>
        public async Task<int> AddItem(IspInsertItem spInsertItem)
        {
            return await _databaseManager.AddRecord(StoredProcedure.spInsertItem, spInsertItem);
        }

        /// <summary>
        /// Upload image to the server
        /// </summary>
        /// <param name="postedFile"></param>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public async Task<bool> UploadImage(HttpPostedFile postedFile, int itemID)
        {
            var filePath = HttpContext.Current.Server.MapPath(string.Format("{0}{1}", Constants.SpecialCharacterTilde, Constants.ImageUploadPath));

            DirectoryInfo directory = new DirectoryInfo(filePath);
            if (!directory.Exists)
            {
                directory.Create();
            }

            string fileUploadPathWithFileName = string.Format("{0}{1}{2}{3}{4}",
                                                                filePath,
                                                                Path.GetFileNameWithoutExtension(postedFile.FileName),
                                                                Constants.SpecialCharacterUnderscore,
                                                                itemID,
                                                                Path.GetExtension(postedFile.FileName));

            var files = new List<string>();
            files.Add(fileUploadPathWithFileName);
            postedFile.SaveAs(fileUploadPathWithFileName);

            return true;
        }
    }
}
