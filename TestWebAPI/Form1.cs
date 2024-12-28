using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZhukBGGClubRanking.Core.Code;
using ZhukBGGClubRanking.Core.Model;

namespace TestWebAPI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void btCreateUser_Click(object sender, EventArgs e)
        {

        }

        private async void btGetTestString_Click(object sender, EventArgs e)
        {
            var res = await TestWebApi.GetTestString(tbUrl.Text, tbLogin.Text, tbPassword.Text);
            tbTestResult.Text = res;
        }

        private async void btGetTestStringAuth_Click(object sender, EventArgs e)
        {
            //var res = await TestWebApi.GetTestStringAuth(tbUrl.Text, tbLogin.Text, tbPassword.Text);
            //tbTestResult.Text = res;
        }

        private async void btLogin_Click(object sender, EventArgs e)
        {
            var res = await TestWebApi.Login(tbUrl.Text, tbLogin.Text, tbPassword.Text, tbUserName.Text, tbUserPassword.Text);
            var tokenInfo = JsonConvert.DeserializeObject<TokenInfo>(res);
            if (tokenInfo != null ) { tbLoginResult.Text = tokenInfo.access_token; }
            
        }

        private async void btTestWithJwc_Click(object sender, EventArgs e)
        {
            var res = await TestWebApi.GetTestStringAuth(tbUrl.Text, tbLogin.Text, tbPassword.Text, tbLoginResult.Text);
            tbWithTokenResult.Text = res;
        }

        private async void btGetGameCollection_Click(object sender, EventArgs e)
        {
            try
            {
                var res = await TestWebApi.GetGames(tbUrl.Text, tbLogin.Text, tbPassword.Text, tbLoginResult.Text);
                var str = new StringBuilder();
                foreach (var game in res.ToList())
                {
                    str.AppendLine(game.ToString());
                }

                textBox2.Text = str.ToString();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private async void btUploadRatingFile_Click(object sender, EventArgs e)
        {
            var games = await TestWebApi.GetGames(tbUrl.Text, tbLogin.Text, tbPassword.Text, tbLoginResult.Text);
            var fileFullName = tbPathToRatingFile.Text;
            var csvArray = Csv.CsvReader.ReadFromText(System.IO.File.ReadAllText(fileFullName));
            var ratingItems = new List<RatingItem>();
            foreach (var arItem in csvArray)
            {
                var game = games.FirstOrDefault(c => c.NameEng == arItem[1]);
                if (game != null)
                {
                    ratingItems.Add(new RatingItem {GameId = game.Id, RatingOrder = Int32.Parse(arItem[0]) });
                }
            }
            var res = await TestWebApi.SaveUsersRating(tbUrl.Text, tbLogin.Text, tbPassword.Text, tbLoginResult.Text, ratingItems);




            
        }

        private async void btShowRatings_Click(object sender, EventArgs e)
        {
            var ratings = await TestWebApi.GetUserActualRatings(tbUrl.Text, tbLogin.Text, tbPassword.Text, tbLoginResult.Text);
            var games = await TestWebApi.GetGames(tbUrl.Text, tbLogin.Text, tbPassword.Text, tbLoginResult.Text);
            var resStr = new StringBuilder();
            resStr.AppendLine("Пользователь - Игра - Рейтинг");
            foreach (var rating in ratings.OrderBy(c => c.UserId))
            {
                foreach (var ratingItem in rating.Rating.RatingItems)
                {
                    var game = games.FirstOrDefault(c => c.Id == ratingItem.GameId);
                    resStr.AppendLine(rating.UserId + " - " + game + " - " + ratingItem.RatingOrder);
                }
                
            }
            tbRatingList.Text = resStr.ToString();
        }
    }

    
}
