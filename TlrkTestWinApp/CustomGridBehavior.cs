using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace TlrkTestWinApp
{
    public class CustomGridBehavior : BaseGridBehavior
    {
        List<GridViewRowInfo> selectedRows = new List<GridViewRowInfo>();

        public List<GridViewRowInfo> SelectedRows
        {
            get
            {
                return this.selectedRows;
            }
        }

        public override bool OnMouseUp(MouseEventArgs e)
        {
            //Execute the selection logic here.
            base.OnMouseDown(e);

            return base.OnMouseUp(e);
        }

        public override bool OnMouseDown(MouseEventArgs e)
        {
            selectedRows.Clear();

            selectedRows.AddRange(this.GridControl.SelectedRows);

            return true;
        }
    }
}
