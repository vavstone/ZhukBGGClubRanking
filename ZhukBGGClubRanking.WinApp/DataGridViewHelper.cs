using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZhukBGGClubRanking.Core;
using ZhukBGGClubRanking.Core.Model;

namespace ZhukBGGClubRanking.WinApp.Core
{
    public static class DataGridViewHelper
    {
        public static List<GridViewDataSourceWrapper> CreateDataSourceWrapper(List<RatingItem> items, List<Game> games)
        {
            var dataSourceWrapper = new List<GridViewDataSourceWrapper>();
            foreach (var ritem in items)
            {
                var dataSourceWrapperItem =
                    GridViewDataSourceWrapper.CreateFromCoreGame(ritem, games);
                dataSourceWrapper.Add(dataSourceWrapperItem);
            }
            return dataSourceWrapper.OrderBy(c => c.Rating).ThenBy(c => c.Game).ToList();
        }
    }
}
