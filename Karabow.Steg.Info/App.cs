using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Karabow.Steg.Info
{
    public static class App
    {
        public static string Name
        {
            get { return "Steg Studio"; }
        }

        public static string FullName
        {
            get { return Company.Name + " " + App.Name + " " + App.Version.ToString(2); }
        }

        public static Version Version
        {
            get { return new Version(1, 0, 0, 0); }
        }

        public static string WebAddress
        {
            get { return "www.Steg-Studio.org"; }
        }
    }

    public static class Company
    {
        public static string Name
        {
            get { return "Frank's"; }
        }

        public static string WebAddress
        {
            get { return "1990hackaholic.blogspot.com"; }
        }
    }

    public static class NeededDirectories
    {
        public static string WebPagesFolder
        {
            get
            {
                return Application.StartupPath + "\\Web Pages";
            }
        }
    }

    public static class NeededFiles
    {
        public static string HomePage
        {
            get
            {
                return NeededDirectories.WebPagesFolder + "\\Home.html";
            }
        }

        public static string SteganographyPage
        {
            get
            {
                return NeededDirectories.WebPagesFolder + "\\steganography.html";
            }
        }

        public static string ComputerSecurityPage
        {
            get
            {
                return NeededDirectories.WebPagesFolder + "\\Computer Security.html";
            }
        }

        public static string CryptographyPage
        {
            get
            {
                return NeededDirectories.WebPagesFolder + "\\Cryptography.html";
            }
        }

        public static string EncryptionPage
        {
            get
            {
                return NeededDirectories.WebPagesFolder + "\\Encryption.html";
            }
        }

        public static string StegStudioPage
        {
            get
            {
                return NeededDirectories.WebPagesFolder + "\\Steg Studio.html";
            }
        }

        public static string Enc_Std_FeaturesPage
        {
            get
            {
                return NeededDirectories.WebPagesFolder + "\\Encryption Studio_Features.html";
            }
        }

        public static string Enc_Std_AccountsPage
        {
            get
            {
                return NeededDirectories.WebPagesFolder + "\\Encryption Studio_Accounts.html";
            }
        }

        public static string Enc_Std_Encrypt
        {
            get
            {
                return NeededDirectories.WebPagesFolder + "\\Encryption Studio_Encrypting.html";
            }
        }

        public static string Enc_Std_Decrypt
        {
            get
            {
                return NeededDirectories.WebPagesFolder + "\\Encryption Studio_Decrypting.html";
            }
        }

        public static string Enc_Std_WhatsNewPage
        {
            get
            {
                return NeededDirectories.WebPagesFolder + "\\Encryption Studio_New.html";
            }
        }

        public static string Enc_Std_ShortcomingsPage
        {
            get
            {
                return NeededDirectories.WebPagesFolder + "\\Encryption Studio_Shortcomings.html";
            }
        }

        public static string Enc_Std_ShortcutPage
        {
            get
            {
                return NeededDirectories.WebPagesFolder + "\\Encryption Studio_Shortcut.html";
            }
        }

        public static string Enc_Std_SourceCodePage
        {
            get
            {
                return NeededDirectories.WebPagesFolder + "\\Encryption Studio_SourceCode.html";
            }
        }

        public static string LicensePage
        {
            get
            {
                return NeededDirectories.WebPagesFolder + "\\License.html";
            }
        }

        public static string CompanyPage
        {
            get
            {
                return NeededDirectories.WebPagesFolder + "\\Company.html";
            }
        }
    }
}
