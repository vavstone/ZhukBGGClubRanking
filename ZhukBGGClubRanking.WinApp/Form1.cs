using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using ZhukBGGClubRanking.Core;

namespace ZhukBGGClubRanking.WinApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public BGGCollection CommonCollection { get; set; }

        public GameRatingList GetRatingInOpenedTab()
        {
            var currentTab = tabControl1.SelectedTab.Text;
            return usersRatingListFiles.FirstOrDefault(c=>c.File.UserNames.Contains(currentTab)).File;
        }

        private List<GameRatingListFile> usersRatingListFiles;

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadUsersRatings();
            SetButton2Text();
            PrepareDataGrid(dataGridView1);
            LoadCommonCollection();
        }

        void LoadCommonCollection()
        {
            CommonCollection = BGGCollection.LoadFromFile();
        }

        void LoadUsersRatings()
        {
            usersRatingListFiles = GameRatingListFile.LoadFromFolder();
            tabControl1.TabPages.Clear();
            checkedListBox1.Items.Clear();
            foreach (var item in usersRatingListFiles)
            {
                checkedListBox1.Items.Add(item.FileNameWithoutExt, true);
                var tabPage = new TabPage();
                tabPage.Text = item.FileNameWithoutExt;
                tabControl1.TabPages.Add(tabPage);
                var grid = new DataGridView();
                grid.Dock = DockStyle.Fill;
                PrepareDataGrid(grid);
                grid.DataSource = item.File.GameList.OrderBy(c => c.Rating).ToList();
                tabPage.Controls.Add(grid);
            }
        }

        public void PrepareDataGrid(DataGridView grid)
        {
            grid.AutoGenerateColumns = false;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.ShowEditingIcon = false;
            //var colName = new DataGridViewTextBoxColumn();
            var colName = new DataGridViewLinkColumn();
            colName.Width = 306;
            colName.HeaderText = "Название";
            colName.DataPropertyName = "Game";
            colName.TrackVisitedState = false;
            colName.LinkBehavior = LinkBehavior.HoverUnderline;
            colName.LinkColor = Color.Black;
            colName.ReadOnly = false;
            grid.Columns.Add(colName);
            var colRate = new DataGridViewTextBoxColumn();
            colRate.Width = 60;
            colRate.HeaderText = "Рейтинг";
            colRate.DataPropertyName = "Rating";
            colRate.ReadOnly = true;
            grid.Columns.Add(colRate);
            AddCommentsColumn(grid);
            grid.CellContentClick += Grid_CellContentClick;
            grid.RowsDefaultCellStyle.SelectionBackColor = Color.LightGray;
            grid.RowsDefaultCellStyle.SelectionForeColor = Color.Black;
            //TODO
            //AddImagesColumn(grid);

        }

        private void Grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = sender as DataGridView;
            if (e.ColumnIndex == 0)
            {
                if (dataGridView1.Columns[e.ColumnIndex] is DataGridViewLinkColumn && e.RowIndex != -1)
                {
                    
                    string cellContent = grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    var url = Settings.UrlForGameBGGId(CommonCollection.GetItemByName(cellContent).ObjectId);
                    ProcessStartInfo sInfo = new ProcessStartInfo(url);
                    Process.Start(sInfo);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var checkedUserNames = checkedListBox1.CheckedItems.OfType<string>().ToList();
            var checkedLists = usersRatingListFiles.Where(c=>checkedUserNames.Contains(c.File.UserNames.First())).Select(c => c.File).ToList();
            var result = GameRatingList.CalculateAvarageRating(checkedLists);
            result.SetBGGCollection(CommonCollection);
            dataGridView1.DataSource = result.GameList.OrderBy(c => c.Rating).ToList();
            //TODO
            //AddImagesToDataGrid(dataGridView1);
        }

        void AddImagesColumn(DataGridView dataGridView)
        {
            var iconColumn = new DataGridViewImageColumn();
            iconColumn.Name = "Pic";
            iconColumn.Width = 120;
            dataGridView.Columns.Add(iconColumn);
        }

        void AddImagesToDataGrid(DataGridView dataGridView)
        {
            for (int row = 0; row < dataGridView1.Rows.Count - 1; row++)
            {
                var uri =
                    "https://cf.geekdo-images.com/Naw8y8J_s-8cvq1GoTON6w__thumb/img/ieH8A28KJe3truXqQe1nDXSpxUE=/fit-in/200x150/filters:strip_icc()/pic7416519.jpg";
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(uri);
                myRequest.Method = "GET";
                HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
                Image img = Image.FromStream(myResponse.GetResponseStream());
                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(img, new Size(50, 50));
                myResponse.Close();
                ((DataGridViewImageCell)dataGridView1.Rows[row].Cells[2]).Value = bmp;
            }
        }

        void AddCommentsColumn(DataGridView dataGridView)
        {
            var col = new DataGridViewTextBoxColumn();
            col.Name = "Владеют";
            col.DataPropertyName = "BGGComments";
            col.Width = 130;
            dataGridView.Columns.Add(col);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ReorderForm reorderForm = new ReorderForm();
            reorderForm.RatingList = GetRatingInOpenedTab();
            var otherCollections = usersRatingListFiles
                .Where(c => c.File.UserNames.FirstOrDefault() != reorderForm.RatingList.UserNames.FirstOrDefault())
                .Select(c => c.File).ToList();
            var newGamesInOthersColl = reorderForm.RatingList.GetGamesNotInCollectionButExistingInOthers(otherCollections);
            if (newGamesInOthersColl.Count > 0)
            {
                if (MessageBox.Show(string.Format("В других коллекциях найдены игры ({0} штук), отсуствующие в вашем рейтинге. Добавить их?",newGamesInOthersColl.Count),
                        "Подтверждение добавления",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    reorderForm.NewGames = newGamesInOthersColl;
                }
            }
            reorderForm.ShowDialog(this);
            LoadUsersRatings();
        }

        void SetButton2Text()
        {
            if (tabControl1.SelectedTab != null)
            {
                var selectedTabText = tabControl1.SelectedTab.Text;
                button2.Text = "Пересмотреть рейтинг " + selectedTabText;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetButton2Text();
        }
    }
}
