using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZhukBGGClubRanking.WinApp
{
    public class DataGridViewCustom:DataGridView
    {
        public DataGridViewCustom()
        {
            AutoGenerateColumns = false;
            AllowUserToAddRows = false;
            AllowUserToDeleteRows = false;
            ShowEditingIcon = false;
            RowHeadersVisible = false;

            RowsDefaultCellStyle.SelectionBackColor = Color.LightGray;
            RowsDefaultCellStyle.SelectionForeColor = Color.Black;

            MouseWheel += DataGridView_MouseWheel;
            MouseEnter += DataGridView_MouseEnter;
            //grid.ColumnHeaderMouseClick += Grid_ColumnHeaderMouseClick;
        }

        private void DataGridView_MouseEnter(object sender, EventArgs e)
        {
            var grid = sender as DataGridView;
            grid.Focus();
        }

        void DataGridView_MouseWheel(object sender, MouseEventArgs e)
        {
            var grid = sender as DataGridView;
            int currentIndex = grid.FirstDisplayedScrollingRowIndex;
            int scrollLines = SystemInformation.MouseWheelScrollLines;

            if (e.Delta > 0)
            {
                grid.FirstDisplayedScrollingRowIndex = Math.Max(0, currentIndex - scrollLines);
            }
            else if (e.Delta < 0)
            {
                if (grid.Rows.Count > (currentIndex + scrollLines))
                    grid.FirstDisplayedScrollingRowIndex = currentIndex + scrollLines;
            }
        }
    }
}
