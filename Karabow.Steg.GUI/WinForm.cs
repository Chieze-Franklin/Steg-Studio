using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Karabow.Steg.Core;
using Karabow.Steg.Info;

namespace Karabow.Steg.GUI
{
    public partial class WinForm : Form
    {
        int vOff = 3;

        StatusStrip statusStrip;
        ToolStripProgressBar statusProgressBar;
        ToolStripStatusLabel statusLabel;

        MenuStrip menu;
        WinMenuStripItem taskMenu, taskEnc, taskDec, taskClose, taskCloseAll, taskExit,
            browserMenu, browserBack, browserForward, browserHome, browserStop, browserRefresh,
            helpMenu, helpUserGuide, helpAbout;

        SplitContainer splitMain;

        ExtendedTabControl workBench;
        ExtendedTabPage mainView;

        ToolTip ttpText;

        SaveFileDialog saveDialog;
        OpenFileDialog openDialog;

        public WinForm()
        {
            InitializeComponent();

            Init();
        }

        void Init()
        {
            #region//ttpText
            ttpText = new ToolTip();
            ttpText.BackColor = Color.Yellow;
            ttpText.ForeColor = Color.Blue;
            ttpText.ToolTipIcon = ToolTipIcon.Info;
            ttpText.UseAnimation = true;
            ttpText.UseFading = true;
            #endregion

            //save
            saveDialog = new SaveFileDialog();
            saveDialog.Title = App.Name + " - Save";
            saveDialog.DefaultExt = ".txt";
            saveDialog.AddExtension = true;
            saveDialog.Filter =
                "Text file (.txt)|*.txt|All files|*.*|BITMAP Image (.bmp)|*.bmp|PNG File (.png)|*.png|" +
                "JPG file (.jpg)|*.jpg|JPEG file (.jpeg)|*.jpeg|HTML file (.html)|*.html|" +
                "HTML file (.htm)|*.htm";

            //open
            openDialog = new OpenFileDialog();
            openDialog.Title = App.Name + " - Open";
            openDialog.DefaultExt = ".*";
            openDialog.AddExtension = true;
            openDialog.Filter =
                "All files|*.*|Text file (.txt)|*.txt|BITMAP Image (.bmp)|*.bmp|PNG File (.png)|*.png|" +
                "JPG file (.jpg)|*.jpg|JPEG file (.jpeg)|*.jpeg|HTML file (.html)|*.html|" +
                "HTML file (.htm)|*.htm";

            //this
            this.BackColor = Color.FromArgb(60, 60, 60);
            this.ForeColor = Color.Coral;
            CheckForNeededFilesAndDirectories();

            //taskNew
            taskEnc = new WinMenuStripItem();
            taskEnc.Text = "&Encryption";
            taskEnc.ShortcutKeys = Keys.Control | Keys.E;
            taskEnc.Image = Properties.Resources.New;
            taskEnc.Click += new EventHandler(taskEnc_Click);

            //taskOpen
            taskDec = new WinMenuStripItem();
            taskDec.Text = "&Decryption";
            taskDec.ShortcutKeys = Keys.Control | Keys.D;
            taskDec.Image = Properties.Resources.Open;
            taskDec.Click += new EventHandler(taskDec_Click);

            //taskClose
            taskClose = new WinMenuStripItem();
            taskClose.Text = "&Close";
            taskClose.Image = Properties.Resources.CloseCurrentDocument;
            taskClose.ShortcutKeys = Keys.Control | Keys.W;
            taskClose.Click += new EventHandler(taskClose_Click);

            //taskCloseAll
            taskCloseAll = new WinMenuStripItem();
            taskCloseAll.Text = "Close &All";
            taskCloseAll.Image = Properties.Resources.CloseAllDocuments;
            taskCloseAll.ShortcutKeys = Keys.Control | Keys.Shift | Keys.W;
            taskCloseAll.Click += new EventHandler(taskCloseAll_Click);

            ////taskSave
            //taskSave = new WinMenuStripItem();
            //taskSave.Text = "&Save";
            //taskSave.ShortcutKeys = Keys.Control | Keys.S;

            ////taskSaveAll
            //taskSaveAll = new WinMenuStripItem();
            //taskSaveAll.Text = "Save A&ll";

            ////taskSaveAs
            //taskSaveAs = new WinMenuStripItem();
            //taskSaveAs.Text = "Sa&ve As...";
            //taskSaveAs.ShortcutKeys = Keys.Control | Keys.Shift | Keys.S;

            //taskExit
            taskExit = new WinMenuStripItem();
            taskExit.Text = "E&xit";
            taskExit.ShortcutKeys = Keys.Control | Keys.Q;
            taskExit.Image = Properties.Resources.CloseApplication;
            taskExit.Click += new EventHandler(taskExit_Click);

            //taskMenu
            taskMenu = new WinMenuStripItem();
            taskMenu.Text = "&Task";
            taskMenu.DropDownItems.AddRange
                (new ToolStripItem[] 
                { 
                    taskEnc, taskDec, new ToolStripSeparator(), taskClose, taskCloseAll,
                    //new ToolStripSeparator(), taskSave, taskSaveAll, taskSaveAs,
                    new ToolStripSeparator(),
                    taskExit
                });

            //browserBack
            browserBack = new WinMenuStripItem();
            browserBack.Text = "&Back";
            browserBack.Image = Properties.Resources.GoBack;
            browserBack.Click += new EventHandler(browserBack_Click);
            //browserBack.ShortcutKeys = Keys.Back;

            //browserForward
            browserForward = new WinMenuStripItem();
            browserForward.Text = "&Forward";
            browserForward.Image = Properties.Resources.GoForward;
            browserForward.Click += new EventHandler(browserForward_Click);
            //browserForward.ShortcutKeys = Keys.BrowserForward;

            //browserHome
            browserHome = new WinMenuStripItem();
            browserHome.Text = "&Home";
            browserHome.Image = Properties.Resources.GoHome;
            browserHome.Click += new EventHandler(browserHome_Click);
            //browserHome.ShortcutKeys = Keys.Home;

            //browserStop
            browserStop = new WinMenuStripItem();
            browserStop.Text = "&Stop";
            browserStop.Image = Properties.Resources.Cancel;
            browserStop.Click += new EventHandler(browserStop_Click);
            //browserStop.ShortcutKeys = Keys.Escape;

            //browserRefresh
            browserRefresh = new WinMenuStripItem();
            browserRefresh.Text = "&Refresh";
            browserRefresh.ShortcutKeys = Keys.F5;
            browserRefresh.Image = Properties.Resources.Refresh;
            browserRefresh.Click += new EventHandler(browserRefresh_Click);

            //browserMenu
            browserMenu = new WinMenuStripItem();
            browserMenu.Text = "&Browser";
            browserMenu.DropDownItems.AddRange
                (new ToolStripItem[] 
                { 
                    browserBack, browserForward, browserHome, browserStop, browserRefresh
                });

            //helpUserGuide
            helpUserGuide = new WinMenuStripItem();
            helpUserGuide.Text = "&User Guide";
            helpUserGuide.ShortcutKeys = Keys.F1;
            helpUserGuide.Image = Properties.Resources.HelpTopics;
            helpUserGuide.Click += new EventHandler(helpUserGuide_Click);

            //helpAbout
            helpAbout = new WinMenuStripItem();
            helpAbout.Text = "&About " + Company.Name + " " + App.Name;
            helpAbout.ShortcutKeys = Keys.F2;
            helpAbout.Image = Properties.Resources.About;
            helpAbout.Click += new EventHandler(helpAbout_Click);

            //helpMenu
            helpMenu = new WinMenuStripItem();
            helpMenu.Text = "&Help";
            helpMenu.DropDownItems.AddRange(new ToolStripItem[] { helpUserGuide, helpAbout });

            //menu
            menu = new MenuStrip();
            menu.BackColor = this.BackColor;
            menu.Items.AddRange(new ToolStripItem[] { taskMenu, browserMenu, helpMenu });

            //statusLabel
            statusLabel = new ToolStripStatusLabel();
            statusLabel.Text = "Ready";

            //statusProgressBar
            statusProgressBar = new ToolStripProgressBar();
            statusProgressBar.Style = ProgressBarStyle.Continuous;
            statusProgressBar.Width = 800;
            //statusProgressBar.BackColor = this.BackColor;
            //statusProgressBar.ForeColor = Color.Green;

            //statusStrip
            statusStrip = new StatusStrip();
            statusStrip.BackColor = this.BackColor;
            statusStrip.ForeColor = this.ForeColor;
            statusStrip.Items.AddRange(new ToolStripItem[] { statusLabel, statusProgressBar });

            //workBench
            workBench = new ExtendedTabControl();
            workBench.Dock = DockStyle.Fill;

            //splitMain
            splitMain = new SplitContainer();
            splitMain.BackColor = Color.Black;
            splitMain.Panel1.BackColor = splitMain.Panel2.BackColor = this.BackColor;
            splitMain.Panel1Collapsed = true;
            splitMain.Panel2.Controls.Add(workBench);

            this.Icon = Properties.Resources.Icon;
            //this.Text = account.Name + " - " + App.FullName;
            this.Text = App.Name;
            this.Size = new Size(650, 650);
            this.MinimumSize = this.Size;
            //this.MaximizeBox = false;
            this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Controls.AddRange(new Control[] { menu, statusStrip, splitMain });
            this.Paint += new PaintEventHandler(WinForm_Paint);

            this.OpenStartTabPage();
        }

        void WinForm_Paint(object sender, PaintEventArgs e)
        {
            //splitMain
            splitMain.Location = new Point(0, menu.Location.Y + menu.Height);
            splitMain.Width = this.Width;
            splitMain.Height = this.Height - (menu.Height + statusStrip.Height + (11 * vOff));
        }

        /// <summary>
        /// Ensures certain needed files exist
        /// </summary>
        public static void CheckForNeededFilesAndDirectories()
        {
            FileSystemServices.CreateDirectory(NeededDirectories.WebPagesFolder, false);

            FileSystemServices.CreateFile(NeededFiles.HomePage, false, Properties.Resources.Home);

            FileSystemServices.CreateFile(NeededFiles.SteganographyPage, false, Properties.Resources.Steganography);

            FileSystemServices.CreateFile(NeededFiles.StegStudioPage, false, Properties.Resources.EncryptionStudio);

            FileSystemServices.CreateFile(NeededFiles.LicensePage, false, Properties.Resources.License);

            FileSystemServices.CreateFile(NeededFiles.CompanyPage, false, Properties.Resources.Company);
        }

        void HideToolTip(Control control)
        {
            ttpText.Hide(control);
        }
        void taskEnc_Click(object sender, EventArgs e)
        {
            OnEnc();
        }
        void taskDec_Click(object sender, EventArgs e)
        {
            OnDec();
        }
        void taskClose_Click(object sender, EventArgs e)
        {
            OnCloseCurrentDoc();
        }
        void taskCloseAll_Click(object sender, EventArgs e)
        {
            OnCloseAllDocs();
        }
        void taskExit_Click(object sender, EventArgs e)
        {
            OnCloseApplication();
        }
        void browserBack_Click(object sender, EventArgs e)
        {
            OnBack();
        }
        void browserForward_Click(object sender, EventArgs e)
        {
            OnForward();
        }
        void browserHome_Click(object sender, EventArgs e)
        {
            OnHome();
        }
        void browserStop_Click(object sender, EventArgs e)
        {
            OnStop();
        }
        void browserRefresh_Click(object sender, EventArgs e)
        {
            OnUpdate_Refresh();
        }
        void helpUserGuide_Click(object sender, EventArgs e)
        {
            OnHelpTopics();
        }
        void helpAbout_Click(object sender, EventArgs e)
        {
            OnAbout();
        }

        public void OnAbout()
        {
            MsgBox msg = new MsgBox
                (
                App.FullName + "\n" +
                "Version " + App.Version.ToString(4) + "\n" +
                /*(Convert.ToChar(169)).ToString() + " 2015 " + Company.Name + " Corporation\n" +
                "All rights reserved\n\n" +*/
                App.Name + " demonstrates the possibiblity and potentials\n" +
                "of steganography.\n" +
                "I think the ability to hide info in the last bit of a byte in a\n" +
                "picture is interesting. Since we are altering the last bit,\n" +
                "you tend not to notice any difference in the image with the info.\n" +
                "Of course you can always notice a difference in the size of the image."
                /*+ "Warning:\n" +
                "This computer program is protected by copyright laws\n" +
                "and international treaties. Unauthorized reproduction\n" +
                "or distribution of this program, or any portion of it,\n" +
                "may result in severe civil and criminal penalties, and\n" +
                "will be prosecuted to the maximum extent under the law"*/
                ,
                ""
                ,
                MessageType.Information, ResponseButtons.OK);
            msg.BannerImage = Properties.Resources.SplashScreen1;
            msg.ShowDialog();
        }
        public void OnBack()
        {
            if (this.CurrentEditorPage is StartTabPage)
            {
                (this.CurrentEditorPage as StartTabPage).BrowserGoBack();
            }
        }
        public void OnCloseApplication()
        {
            this.Close();
        }
        public void OnCloseAllDocs()
        {
            int n = workBench.TabPages.Count - 1; //-1 bcuz we dont want to touch the first page (the start page)
            while (n >= 0)
            {
                workBench.CurrentPage = workBench.TabPages[n];
                OnCloseCurrentDoc();
                n--;
            }
        }
        public void OnCloseCurrentDoc()
        {
            if (!(this.CurrentEditorPage is StartTabPage))
            {
                if (CurrentEditorPage is WorkPage)
                {
                    ((WorkPage)CurrentEditorPage).Steganographer.Feedback -= Steganographer_Feedback;
                }
                this.CurrentEditorPage.Parent.Controls.Remove(this.CurrentEditorPage);
            }
        }
        public void OnDec()
        {
            mainView = new DecryptionPage();
            workBench.Controls.Add(mainView);
            workBench.CurrentPage = mainView;
            if (workBench.CurrentPage is DecryptionPage)
            {
                ((DecryptionPage)workBench.CurrentPage).Steganographer.Feedback += Steganographer_Feedback;
            }
        }
        public void OnEnc()
        {
            mainView = new EncryptionPage();
            workBench.Controls.Add(mainView);
            workBench.CurrentPage = mainView;
            if (workBench.CurrentPage is EncryptionPage)
            {
                ((EncryptionPage)workBench.CurrentPage).Steganographer.Feedback += Steganographer_Feedback;
            }
        }
        public void OnForward()
        {
            if (this.CurrentEditorPage is StartTabPage)
            {
                (this.CurrentEditorPage as StartTabPage).BrowserGoForward();
            }
        }
        public void OnHelpTopics()
        {
            MsgBox msg = new MsgBox("For help topics, go to Start Page.", MessageType.Information,
                     ResponseButtons.OK);
            msg.BannerImage = Properties.Resources.SplashScreen1;
            msg.ShowDialog();
        }
        public void OnHome()
        {
            if (this.CurrentEditorPage is StartTabPage)
            {
                (this.CurrentEditorPage as StartTabPage).BrowserGoHome();
            }
        }
        public void OnStop()
        {
            if (this.CurrentEditorPage is StartTabPage)
            {
                (this.CurrentEditorPage as StartTabPage).BrowserStop();
            }
        }
        public void OnUpdate_Refresh()
        {
            if (this.CurrentEditorPage is StartTabPage)
            {
                (this.CurrentEditorPage as StartTabPage).BrowserRefresh();
            }
        }
        public void OpenStartTabPage()
        {
            mainView = new StartTabPage();
            workBench.Controls.Add(mainView);
            workBench.CurrentPage = mainView;
            if (workBench.CurrentPage is StartTabPage)
            {
                ((StartTabPage)workBench.CurrentPage).LoadHelpTopics();
            }
        }

        void Steganographer_Feedback(object sender, FeedbackEventArgs e)
        {
            statusLabel.Text = e.FilePath;
            if (e.Type == FeedbackType.Progress || e.Type == FeedbackType.Begin || e.Type == FeedbackType.End)
            {
                statusLabel.Text += "     " + e.Message + "     ";
                statusProgressBar.Value = e.WorkProgress;
            }
            else
            {
                MessageType msgType = MessageType.Error;
                if (e.Type == FeedbackType.Information)
                    msgType = MessageType.Information;
                else if (e.Type == FeedbackType.Question)
                    msgType = MessageType.Question;
                else if (e.Type == FeedbackType.Warning)
                    msgType = MessageType.Warning;
                MsgBox msg = new MsgBox(e.Message, msgType, ResponseButtons.OK);
                msg.ShowDialog();
            }
        }

        #region PROPERTIES

        public TabPage CurrentEditorPage
        {
            get
            {
                //try
                {
                    return workBench.CurrentPage;
                }
                //catch { return null; }
            }
        }
        #endregion
    }

    public class WinMenuStripItem : ToolStripMenuItem
    {
        public WinMenuStripItem()
        {
            this.BackColor = Color.FromArgb(60, 60, 60);
            this.ForeColor = Color.Coral;
            this.MouseLeave += new EventHandler(WinMenuStrip_MouseLeave);
            this.MouseEnter += new EventHandler(WinMenuStrip_MouseEnter);
        }

        void WinMenuStrip_MouseLeave(object sender, EventArgs e)
        {
            this.ForeColor = Color.Coral;
        }

        void WinMenuStrip_MouseEnter(object sender, EventArgs e)
        {
            this.ForeColor = Color.Black;
        }
    }
}
