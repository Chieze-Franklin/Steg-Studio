using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Karabow.Steg.GUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] accountPathAndPwds)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string accountPathAndPwd = "";
            //for (int i = 0; i < accountNames.Length; i++)
            //{
            //    accountName += accountNames[i];
            //}
            foreach (string s in accountPathAndPwds)
            {
                accountPathAndPwd += s;
            }

            if (accountPathAndPwd != string.Empty)
            {
                string accPath, accPwd;

                if (!accountPathAndPwd.Contains('*'))
                    goto here;

                accPath = accountPathAndPwd.Substring(0, accountPathAndPwd.IndexOf('*'));
                accPwd = accountPathAndPwd.Substring(accountPathAndPwd.IndexOf('*') + 1);
            }

        here:
            //SplashScreen splash = new SplashScreen(accountPathAndPwd);
            //Application.Run(splash);
            //if (splash.StartApplication)
            //{
            //    Application.Run(new WinForm(splash.Account));
            //}

            Application.Run(new WinForm());
        }
    }
}
