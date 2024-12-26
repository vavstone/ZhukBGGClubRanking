using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Windows.Forms;
using ZhukBGGClubRanking.Core;

namespace Utils
{
    public partial class Form1 : Form
    {
        public GamesNamesTranslateFile GamesTranslate { get; set; }
        public BGGCollection CommonCollection { get; set; }
        

        public bool TeseraPreferable = true;

        public Form1()
        {
            InitializeComponent();
            
        }

        private void btLoadGamesImages_Click(object sender, EventArgs e)
        {
            LoadGamesTranslate();
            LoadCommonCollection();
            LoadImages();
        }

        void LoadCommonCollection()
        {
            CommonCollection = BGGCollection.LoadFromFile(GamesTranslate);
        }

        void LoadGamesTranslate()
        {
            GamesTranslate = GamesNamesTranslateFile.LoadFromFile();
        }

        void LoadImages()
        {
            var imagesPath = Path.Combine(Application.StartupPath, "images");
            var smallImagesPath = Path.Combine(imagesPath, "small");
            var largeImagesPath = Path.Combine(imagesPath, "large");
            foreach (var item in CommonCollection.Items)
            {
                var smallImgUrl = item.Thumbnail;
                var largeImgUrl = item.Image;
                using (var client = new WebClient())
                {
                    string imgFileName = System.IO.Path.GetFileName(smallImgUrl);
                    var fileExt = Path.GetExtension(imgFileName);
                    var imgNewFileName = item.ObjectId + fileExt;
                    var fullName = Path.Combine(smallImagesPath, imgNewFileName);
                    client.DownloadFile(smallImgUrl, fullName);

                    imgFileName = System.IO.Path.GetFileName(largeImgUrl);
                    fileExt = Path.GetExtension(imgFileName);
                    imgNewFileName = item.ObjectId + fileExt;
                    fullName = Path.Combine(largeImagesPath, imgNewFileName);
                    client.DownloadFile(largeImgUrl, fullName);
                }
            }
        }
    }
}
