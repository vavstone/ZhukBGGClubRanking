using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oli.Controls;
using ZhukBGGClubRanking.Core;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ZhukBGGClubRanking.WinApp
{
    public partial class ReorderForm : Form
    {
        public ReorderForm()
        {
            InitializeComponent();
            dragDropBindingSource.DataSource = dragDropListBox1;
        }

        public GameRatingList RatingList { get; set; }

        public List<GameRating> NewGames { get; set; }


        private void ReorderForm_Load(object sender, EventArgs e)
        {
            LoadList();
            this.Text = string.Format("Рейтинг {0}", RatingList.UserNames.FirstOrDefault());
            if (NewGames!=null && NewGames.Any())
            {
                AddNewGames();
                label1.Text = "В конец списка добавлены новые игры!!!";
            }
        }

        private void AddNewGames()
        {
            dragDropListBox1.Items.Add(" ");
            foreach (var game in NewGames.OrderBy(c => c.Rating))
            {
                dragDropListBox1.Items.Add(game.Game);
            }
        }

        void LoadList()
        {
            dragDropListBox1.Items.Clear();
            foreach (var game in RatingList.GameList.OrderBy(c => c.Rating))
            {
                dragDropListBox1.Items.Add(game.Game);
            }
        }

        GameRatingList GetUpdatedGameRatingListFromForm()
        {
            var result = new GameRatingList();
            result.UserNames = RatingList.UserNames;
            var counter = 1;
            foreach (var item in dragDropListBox1.Items)
            {
                if (!string.IsNullOrWhiteSpace(item.ToString()))
                    result.GameList.Add(new GameRating {Game = item.ToString(), Rating = counter++});
            }
            result.CalculateWeightByRating();
            return result;
        }

        void SaveList()
        {
            if (MessageBox.Show(
                    string.Format("Вы действительно хотите сохранить новый порядок рейтинга {0}?",RatingList.UserNames.FirstOrDefault()),
                    "Подтверждение",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button2 ) == DialogResult.Yes)
            {
                var updatedList = GetUpdatedGameRatingListFromForm();
                var ratingListFile = new GameRatingListFile();
                ratingListFile.File = updatedList;
                ratingListFile.FileNameWithoutExt = updatedList.UserNames.FirstOrDefault();
                ratingListFile.SaveToFile();
                this.Close();

            }
        }

        //ListViewItem heldDownItem;
        //Point heldDownPoint;
        
        //private void listView1_MouseDown_1(object sender, MouseEventArgs e)
        //{
        //    //listView1.AutoArrange = false;
        //    heldDownItem = listView1.GetItemAt(e.X, e.Y);
        //    if (heldDownItem != null)
        //    {
        //        heldDownPoint = new Point(e.X - heldDownItem.Position.X,
        //            e.Y - heldDownItem.Position.Y);
        //    }
        //}

        //private void listView1_MouseUp_1(object sender, MouseEventArgs e)
        //{
        //    heldDownItem = null;
        //    //listView1.AutoArrange = true;   
        //}

        //private void listView1_MouseMove_1(object sender, MouseEventArgs e)
        //{
        //    if (heldDownItem != null)
        //    {
        //        heldDownItem.Position = new Point(e.Location.X - heldDownPoint.X,
        //            e.Location.Y - heldDownPoint.Y);
        //    }
        //}

        private void button1_Click(object sender, EventArgs e)
        {
            SaveList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadList();
            label1.Text = "";
        }
    }
}
