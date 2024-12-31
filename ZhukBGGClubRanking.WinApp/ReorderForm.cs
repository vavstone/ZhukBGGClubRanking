using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using ZhukBGGClubRanking.Core;
using ZhukBGGClubRanking.Core.Model;
using ZhukBGGClubRanking.WinApp.Core;

namespace ZhukBGGClubRanking.WinApp
{
    public partial class ReorderForm : Form
    {
        public ReorderForm()
        {
            InitializeComponent();
            bwSaveRating.WorkerSupportsCancellation = true;
            bwSaveRating.WorkerReportsProgress = true;
            bwSaveRating.DoWork += BwSaveRating_DoWork;
            bwSaveRating.RunWorkerCompleted += BwSaveRating_RunWorkerCompleted;
        }

        public UsersRating RatingList { get; set; }
        public List<Game> NewGames { get; set; }
        public AppCache Cache { get; set; }
        public User CurrentUser { get; set; }
        private BackgroundWorker bwSaveRating = new BackgroundWorker();
        public HostingSettings Settings { get; set; }

       

        private void BwSaveRating_DoWork(object sender, DoWorkEventArgs e)
        {
            var options = e.Argument as UploadRatingPrmForBW;
            var result = new WebDataResultForBW();
            var reqResult = WebApiHandler.SaveUsersRating(
                options.HostingSettings.Url,
                options.HostingSettings.Login,
                options.HostingSettings.Password,
                JWTPrm.Token,
                options.RatingItems);

            if (reqResult.Result.StatusCode.ToString() == "OK")
            {
                result.Result = true;
            }
            else
            {
                result.Result = false;
                result.Message = reqResult.Result.StatusCode.ToString();
            }
            e.Result = result;
        }

        private void BwSaveRating_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var result = e.Result as WebDataResultForBW;
            if (result.Result)
            {
                this.DialogResult = DialogResult.OK;
                //this.Close();
            }
            else
            {
                MessageBox.Show(result.Message);
            }
        }

        private void ReorderForm_Load(object sender, EventArgs e)
        {
            LoadList();
            this.Text = string.Format("Рейтинг {0}", CurrentUser.Name);
            if (NewGames!=null && NewGames.Any())
            {
                AddNewGames();
                label1.Text = "В конец списка добавлены новые игры!!!";
            }
        }

        private void AddNewGames()
        {
            dragDropListBox1.Items.Add(" ");
            foreach (var game in NewGames.OrderBy(c => c.Name))
            {
                dragDropListBox1.Items.Add(game.Name);
            }
        }

        void LoadList()
        {
            dragDropListBox1.Items.Clear();
            if (RatingList != null)
            {
                foreach (var ratingItem in RatingList.Rating.RatingItems.OrderBy(c => c.RatingOrder))
                {
                    var game = Cache.Games.FirstOrDefault(c => c.Id == ratingItem.GameId);
                    dragDropListBox1.Items.Add(game.Name);
                }
            }
        }

        UsersRating GetUpdatedGameRatingListFromForm()
        {
            var result = new UsersRating();
            result.UserId = CurrentUser.Id;
            var counter = 1;
            foreach (var item in dragDropListBox1.Items)
            {
                if (!string.IsNullOrWhiteSpace(item.ToString()))
                {
                    var gameEng = Game.GetGameNameEngFromFormattedName(item.ToString());
                    var game = Cache.Games.FirstOrDefault(c => c.NameEng == gameEng);
                    result.Rating.RatingItems.Add(new RatingItem()
                        {GameId = game.Id, RatingOrder = counter++});
                }
            }
            result.CalculateWeightByRating();
            return result;
        }

        void SaveList()
        {
            if (MessageBox.Show(
                    string.Format("Вы действительно хотите сохранить новый порядок рейтинга?"),
                    "Подтверждение",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button2 ) == DialogResult.Yes)
            {
                var updatedList = GetUpdatedGameRatingListFromForm();

                //var ratingListFile = new GameRatingListFile();
                //ratingListFile.RatingList = updatedList;
                //ratingListFile.FileNameWithoutExt = updatedList.UserNames.FirstOrDefault();
                //ratingListFile.SaveToFile();
                bwSaveRating.RunWorkerAsync(new UploadRatingPrmForBW
                {
                    RatingItems = updatedList.Rating.RatingItems,
                    HostingSettings = Settings
                });
                

            }
        }

        

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
