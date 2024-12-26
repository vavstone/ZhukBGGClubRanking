using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZhukBGGClubRanking.Core;

namespace ZhukBGGClubRanking.WinApp
{
    public partial class SelectTablesColumns : Form
    {
        public TablesSettings Settings { get; set; }
        public bool UserTableSettingsChanged { get; set; }
        public bool AverageTableSettingsChanged { get; set; }

        TablesSettings _settings { get; set; }

        public SelectTablesColumns()
        {
            InitializeComponent();
        }

        public DialogResult CustomShow(TablesSettings tablesSettings)
        {
            Settings = tablesSettings;
            return this.ShowDialog();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
             Settings.UserRatingTable.ShowRating = cbxLeftTableRating.Checked;
             Settings.UserRatingTable.ShowUsersRating = cbxLeftTableUsersRating.Checked;
             Settings.UserRatingTable.ShowOwners = cbxLeftTableOwners.Checked;
             Settings.AverageRatingTable.ShowRating = cbxRightTableRating.Checked;
             Settings.AverageRatingTable.ShowUsersRating = cbxRightTableUsersRating.Checked;
             Settings.AverageRatingTable.ShowOwners = cbxRightTableOwners.Checked;
             UserTableSettingsChanged = Settings.UserRatingTable != _settings.UserRatingTable;
             AverageTableSettingsChanged = Settings.AverageRatingTable != _settings.AverageRatingTable;
             this.DialogResult = DialogResult.OK;
             this.Close();
        }

        private void btDiscard_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void SelectTablesColumns_Load(object sender, EventArgs e)
        {
            _settings = Settings.CreateCopy();
            cbxLeftTableRating.Checked = Settings.UserRatingTable.ShowRating;
            cbxLeftTableUsersRating.Checked = Settings.UserRatingTable.ShowUsersRating;
            cbxLeftTableOwners.Checked = Settings.UserRatingTable.ShowOwners;
            cbxRightTableRating.Checked = Settings.AverageRatingTable.ShowRating;
            cbxRightTableUsersRating.Checked = Settings.AverageRatingTable.ShowUsersRating;
            cbxRightTableOwners.Checked = Settings.AverageRatingTable.ShowOwners;
        }
    }
}
