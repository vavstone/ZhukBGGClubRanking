using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using ZhukBGGClubRanking.Core.Code;
using ZhukBGGClubRanking.WinApp.Core;

namespace ZhukBGGClubRanking.WinApp
{
    public partial class LoginForm : Form
    {
        public UserSettings UserSettings { get; set; }

        private BackgroundWorker bw = new BackgroundWorker();

        public LoginForm()
        {
            InitializeComponent();
            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = true;
            bw.DoWork += Bw_DoWork;
            bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
        }

        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var result = e.Result as LoginResultForBW;
            if (result.Result)
            {
                JWTPrm.Token = result.Token;
                JWTPrm.UserName = result.UserName;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                lblError.Text = result.Message;
            }
        }

        private async void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var options = e.Argument as LoginPrmForBW;
            var result = new LoginResultForBW();
            var reqResult = WebApiHandler.Login(
                options.HostingSettings.Url, 
                options.HostingSettings.Login,
                options.HostingSettings.Password,
                options.Login, 
                options.Password);

            if (reqResult.Result.StatusCode.ToString() == "OK")
            {
                result.Result = true;
                var tokenInfoString = await reqResult.Result.Content.ReadAsStringAsync();
                var tokenInfo = JsonConvert.DeserializeObject<TokenInfo>(tokenInfoString);
                result.Token = tokenInfo.access_token;
                result.UserName = tokenInfo.username;
            }
            else
            {
                result.Result = false;
                result.Message = reqResult.Result.StatusCode.ToString();
            }

            e.Result = result;
        }

        private async void btSubmit_Click(object sender, EventArgs e)
        {

            if (!bw.IsBusy)
            {
                var prm = new LoginPrmForBW
                {
                    HostingSettings = UserSettings.Hosting,
                    Login = tbLogin.Text,
                    Password = tbPassword.Text.Trim().ToLower()
                };
            
                bw.RunWorkerAsync(prm);
            }

            
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            UserSettings = UserSettings.GetUserSettings();
        }
    }

    public class LoginPrmForBW:WebPrmForBW
    {
        internal string Login { get; set; }
        internal string Password { get; set; }
    }

    internal class LoginResultForBW
    {
        internal bool Result { get; set; }
        internal string Token { get; set; }
        internal string UserName { get; set; }
        internal string Message { get; set; }
    }
}
