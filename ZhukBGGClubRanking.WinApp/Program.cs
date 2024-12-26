using System;
using System.Runtime.ExceptionServices;
using System.Windows.Forms;

namespace ZhukBGGClubRanking.WinApp
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        [HandleProcessCorruptedStateExceptions]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

           

            AppDomain.CurrentDomain.FirstChanceException += FirstChanceExceptionEventHandler;
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionEventHandler;

            Application.Run(new Form1());
        }


        private static void UnhandledExceptionEventHandler(object sender, UnhandledExceptionEventArgs e)
        {
            //ShowErrorMessage(e.ExceptionObject as Exception);
            Console.WriteLine($"Возникла ошибка {e.ExceptionObject}");
        }
        private static void FirstChanceExceptionEventHandler(object sender, FirstChanceExceptionEventArgs e)
        {
            //ShowErrorMessage(e.Exception);
            Console.WriteLine($"Возникла ошибка {e.Exception}");
        }



        //static void ShowErrorMessage(Exception exception)
        //{
        //    var messageText = string.Format("Произошла ошибка. Свяжитесь с разработчиком и направьте скриншот ошибки. /r/n {0} /r/n {1]", exception.Message,
        //        exception.StackTrace);
        //    MessageBox.Show(messageText, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        //}
    }
}
