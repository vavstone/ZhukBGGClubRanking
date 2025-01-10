using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Net;
using System.Windows.Forms;
using ZhukBGGClubRanking.Core;
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
            ShowHideHostingSettingsElements(false);
        }

        void ShowHideHostingSettingsElements(bool show)
        {
            lblHostingLogin.Visible = show;
            lblHostingUrl.Visible = show;
            lblHostingPassword.Visible = show;
            tbHostingLogin.Visible = show;
            tbHostingUrl.Visible = show;
            tbHostingPassword.Visible = show;
        }

        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                var result = e.Result as LoginResultForBW;
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    JWTPrm.Token = result.Token;
                    JWTPrm.UserName = result.UserName;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    if (result.StatusCode == HttpStatusCode.Moved)
                    {
                        ShowError("Необходима изменение настроек. Обратитесь к администратору системы.");
                        ShowHideHostingSettingsElements(true);
                    }
                    else
                    {
                        ShowError(CoreConstants.CommonUserErrorMessage);
                    }
                    
                }
            }
        }

        void ShowError(string message)
        {
            MessageBox.Show(message,"Возникла ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var options = e.Argument as LoginPrmForBW;
            var result = new LoginResultForBW();
            var reqResult = WebApiHandler.Login(
                options.HostingSettings.Url, 
                options.HostingSettings.Login,
                options.HostingSettings.Password,
                options.Login, 
                options.Password);
            result.StatusCode = reqResult.Result.StatusCode;
            if (reqResult.Result.StatusCode == HttpStatusCode.OK)
            {
                var tokenInfoString = reqResult.Result.Content.ReadAsStringAsync();
                var tokenInfo = JsonConvert.DeserializeObject<TokenInfo>(tokenInfoString.Result);
                result.Token = tokenInfo.access_token;
                result.UserName = tokenInfo.username;
            }
            e.Result = result;
        }

        private async void btSubmit_Click(object sender, EventArgs e)
        {
            var newHostingUrl = tbHostingUrl.Text.Trim();
            var newHostingLogin = tbHostingLogin.Text.Trim();
            var newHostingPassword = tbHostingPassword.Text.Trim();
            if (!string.IsNullOrWhiteSpace(newHostingUrl) || !string.IsNullOrWhiteSpace(newHostingLogin) ||
                !string.IsNullOrWhiteSpace(newHostingPassword))
            {
                UserSettings.Hosting.Url = newHostingUrl;
                UserSettings.Hosting.Login = newHostingLogin;
                UserSettings.Hosting.Password = newHostingPassword;
                UserSettings.SaveUserSettings();
            }
            if (!bw.IsBusy)
            {
                var prm = new LoginPrmForBW
                {
                    HostingSettings = UserSettings.Hosting,
                    Login = tbLogin.Text,
                    Password = tbPassword.Text.Trim()
                };
                bw.RunWorkerAsync(prm);
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            UserSettings = UserSettings.GetUserSettings();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void tbLogin_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }

    
}
