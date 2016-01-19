using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Karabow.Steg.Core;
using Karabow.Steg.Info;

namespace Karabow.Steg.GUI
{
    public class ExtendedTabPage : TabPage
    {
        int tabSelectedNum = 0;
        /// <summary>
        /// Helps me to get the last extendedtabpage selected in a extendedtabcontrol
        /// </summary>
        public int TabSelectedNumber
        {
            get { return tabSelectedNum; }
            set { tabSelectedNum = value; }
        }

        /// <summary>
        /// gets the form that contains the tab page
        /// </summary>
        protected WinForm Window
        {
            get { return (WinForm)this.FindForm(); }
        }
    }

    public class StartTabPage : ExtendedTabPage
    {
        Label lblCompName, lblAppName, lblAppVersion;

        SplitContainer splitMainSplitCont;
        SplitContainer splitBrowserFromTabs;
        SplitContainer splitBrowser;

        TabControl tbctHelpTopics;
        TabPage tbpgHelpTopics;

        Navigator navigator;
        Viewer viewer;

        public StartTabPage()
        {
            //viewer
            viewer = new Viewer();
            viewer.Dock = DockStyle.Fill;
            viewer.Navigated += new WebBrowserNavigatedEventHandler(viewer_Navigated);

            //navigator
            navigator = new Navigator();
            navigator.Dock = DockStyle.Fill;
            navigator.CurrentBrowser = viewer;
            navigator.BackColor = new Button().BackColor; //just so that it is not Gray
            try
            {
                navigator.NavigateTo(Info.NeededFiles.HomePage);
            }
            catch { }

            //splitBrowser
            splitBrowser = new SplitContainer();
            splitBrowser.SplitterDistance = 25;
            splitBrowser.IsSplitterFixed = true;
            splitBrowser.Orientation = Orientation.Horizontal;
            splitBrowser.Dock = DockStyle.Fill;
            splitBrowser.BackColor = Color.Black;
            splitBrowser.Panel1.Controls.Add(navigator);
            splitBrowser.Panel2.Controls.Add(viewer);

            //tbpgHelpTopics
            tbpgHelpTopics = new TabPage();
            tbpgHelpTopics.Text = "Help Topics";
            tbpgHelpTopics.AutoScroll = true;

            //tbctHelpTopics
            tbctHelpTopics = new TabControl();
            tbctHelpTopics.Dock = DockStyle.Fill;
            tbctHelpTopics.Controls.Add(tbpgHelpTopics);
            tbctHelpTopics.BackColor = Color.Gray;

            //splitBrowserFromTabs
            splitBrowserFromTabs = new SplitContainer();
            splitBrowserFromTabs.Size = new Size(500, 500);
            splitBrowserFromTabs.Panel1MinSize = 50;
            splitBrowserFromTabs.SplitterDistance = 224;
            splitBrowserFromTabs.Panel2MinSize = 150;
            splitBrowserFromTabs.Dock = DockStyle.Fill;
            splitBrowserFromTabs.Orientation = Orientation.Vertical;
            splitBrowserFromTabs.Panel1.Controls.Add(tbctHelpTopics);
            splitBrowserFromTabs.Panel2.Controls.Add(splitBrowser);
            splitBrowserFromTabs.BackColor = Color.Black;

            //splitMainSplitCont
            splitMainSplitCont = new SplitContainer();
            splitMainSplitCont.Dock = DockStyle.Fill;
            splitMainSplitCont.IsSplitterFixed = true;
            splitMainSplitCont.Orientation = Orientation.Horizontal;
            splitMainSplitCont.Panel1.BackgroundImage = Properties.Resources.SplashScreen1;
            splitMainSplitCont.Panel1.BackgroundImageLayout = ImageLayout.Stretch;
            splitMainSplitCont.SplitterDistance = 150;
            splitMainSplitCont.BackColor = Color.Black;

            //splitMainSplitCont (2)
            //lblCompName
            lblCompName = new Label();
            lblCompName.AutoSize = true;
            lblCompName.Text = Company.Name;
            lblCompName.TextAlign = ContentAlignment.BottomRight;
            lblCompName.Font = new Font("Comic Sans MS", 13.0F, FontStyle.Regular);
            lblCompName.BackColor = Color.Transparent;

            //lblAppName
            lblAppName = new Label();
            lblAppName.AutoSize = true;
            lblAppName.Text = App.Name;
            lblAppName.TextAlign = ContentAlignment.MiddleRight;
            lblAppName.Font = new Font("Comic Sans MS", 30.0F, FontStyle.Bold);
            lblAppName.BackColor = Color.Transparent;

            //lblAppVersion
            lblAppVersion = new Label();
            lblAppVersion.AutoSize = true;
            lblAppVersion.Text = App.Version.ToString();
            lblAppVersion.TextAlign = ContentAlignment.TopRight;
            lblAppVersion.Font = new Font("Comic Sans MS", 20.0F, FontStyle.Regular);
            lblAppVersion.BackColor = Color.Transparent;

            splitMainSplitCont.Panel1.Controls.AddRange(new Control[] 
            {
                lblCompName, lblAppName, lblAppVersion
            });
            splitMainSplitCont.Panel2.Controls.Add(splitBrowserFromTabs);

            this.Controls.Add(splitMainSplitCont);
            this.Text = "Start Page";
            this.Paint += new PaintEventHandler(StartTabPage_Paint);
            LoadHelpTopics();
        }

        void StartTabPage_Paint(object sender, PaintEventArgs e)
        {
            //splitMainSplitCont
            splitMainSplitCont.SplitterDistance = 150;

            //lblCompName
            lblCompName.Location = new Point(splitMainSplitCont.Panel1.Width - lblCompName.Width, 30);

            //lblAppName
            lblAppName.Location = new Point(splitMainSplitCont.Panel1.Width - lblAppName.Width,
                lblCompName.Location.Y + lblCompName.Height);

            //lblAppVersion
            lblAppVersion.Location = new Point(splitMainSplitCont.Panel1.Width - lblAppVersion.Width,
                lblAppName.Location.Y + lblAppName.Height);

            //splitBrowser
            splitBrowser.SplitterDistance = 25;

            //splitBrowserFromTabs
            splitBrowserFromTabs.SplitterDistance = 230;
        }

        void viewer_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            navigator.Text = e.Url.ToString();
        }

        public void BrowserGoBack()
        {
            if (this.Browser.CanGoBack)
                this.Browser.GoBack();
        }
        public void BrowserGoForward()
        {
            if (this.Browser.CanGoForward)
                this.Browser.GoForward();
        }
        public void BrowserGoHome()
        {
            navigator.NavigateTo(Info.NeededFiles.HomePage);
        }
        public void BrowserRefresh()
        {
            this.Browser.Refresh();
        }
        public void BrowserStop()
        {
            this.Browser.Stop();
        }

        public void LoadHelpTopics()
        {
            tbpgHelpTopics.Controls.Clear();
            try
            {
                HelpTopicsButton btnHomePage = new HelpTopicsButton(NeededFiles.HomePage);
                btnHomePage.TextAlign = ContentAlignment.MiddleLeft;

                HelpTopicsButton btnHtmlPage = new HelpTopicsButton(NeededFiles.SteganographyPage);
                btnHtmlPage.TextAlign = ContentAlignment.MiddleLeft;

                HelpTopicsButton btnHtmlViewerPage = new HelpTopicsButton(NeededFiles.StegStudioPage);
                btnHtmlViewerPage.TextAlign = ContentAlignment.MiddleLeft;

                HelpTopicsButton btnLicensePage = new HelpTopicsButton(NeededFiles.LicensePage);
                btnLicensePage.TextAlign = ContentAlignment.MiddleLeft;

                HelpTopicsButton btnCLSyPage = new HelpTopicsButton(NeededFiles.CompanyPage);
                btnCLSyPage.TextAlign = ContentAlignment.MiddleLeft;

                tbpgHelpTopics.Controls.Add(btnHomePage);
                tbpgHelpTopics.Controls.Add(btnHtmlPage);
                tbpgHelpTopics.Controls.Add(btnHtmlViewerPage);
                tbpgHelpTopics.Controls.Add(btnLicensePage);
                tbpgHelpTopics.Controls.Add(btnCLSyPage);

                if (tbpgHelpTopics.HasChildren)
                {
                    tbpgHelpTopics.Controls[0].Location = new Point(2, 2);
                    tbpgHelpTopics.Controls[0].Size = new Size(220, 30);
                    if (tbpgHelpTopics.Controls[0] is HelpTopicsButton)
                    {
                        (tbpgHelpTopics.Controls[0] as HelpTopicsButton).CurrentNavigator = navigator;
                    }

                    for (int n = 1; n < tbpgHelpTopics.Controls.Count; n++)
                    {
                        tbpgHelpTopics.Controls[n].Location = new Point
                            (2,
                            tbpgHelpTopics.Controls[n - 1].Location.Y +
                            tbpgHelpTopics.Controls[n - 1].Height + 2);
                        tbpgHelpTopics.Controls[n].Size = new Size(220, 30);
                        if (tbpgHelpTopics.Controls[n] is HelpTopicsButton)
                        {
                            (tbpgHelpTopics.Controls[n] as HelpTopicsButton).CurrentNavigator = navigator;
                        }
                    }
                }
            }
            catch { }
        }

        WebBrowser Browser
        {
            get { return viewer; }
        }
    }

    public class WorkPage : ExtendedTabPage
    {
        protected SplitContainer splitMain;
        protected Panel pnlControlPanel;
        protected Button btnSelectFile, btnSelectFolder;
        protected HintedTextBox txtSource, txtPassword;
        protected CheckBox chckDeleteSource, chckShowPassword;

        protected ToolStripContainer tscListViewCont;
        protected ToolStrip stripListViewToolBar;
        protected ToolStripTextBox searchBox;
        protected ToolStripButton btnSaveLog;
        protected ListView listView;

        protected OpenFileDialog openDialog;
        protected SaveFileDialog saveDialog;
        protected FolderBrowserDialog folderDialog;

        public Steganographer Steganographer;

        protected XmlDocument logDocument;

        protected int offset = 2;

        public WorkPage()
        {
            //logDocument
            InitLogDocument();

            //steg
            Steganographer = new Steganographer();
            Steganographer.Feedback += Steganographer_Feedback;

            //folder
            folderDialog = new FolderBrowserDialog();
            folderDialog.ShowNewFolderButton = true;

            //save
            saveDialog = new SaveFileDialog();
            saveDialog.Title = App.Name + " - Save";
            saveDialog.DefaultExt = ".bmp";
            saveDialog.AddExtension = true;
            saveDialog.Filter =
                "BITMAP Image (.bmp)|*.bmp|PNG File (.png)|*.png|JPG file (.jpg)|*.jpg" +
                "|JPEG file (.jpeg)|*.jpeg|HTML file (.html)|*.html|HTML file (.htm)|*.htm|XML file (.xml)|*.xml|All files|*.*";

            //open
            openDialog = new OpenFileDialog();
            openDialog.Title = App.Name + " - Open";
            openDialog.DefaultExt = ".*";
            openDialog.AddExtension = true;
            openDialog.Multiselect = false;
            openDialog.Filter =
                "All files|*.*|BITMAP Image (.bmp)|*.bmp|PNG File (.png)|*.png|JPG file (.jpg)|*.jpg" +
                "|JPEG file (.jpeg)|*.jpeg|Text file (.txt)|*.txt|HTML file (.html)|*.html|" +
                "HTML file (.htm)|*.htm";

            //btnSelectFile
            btnSelectFile = new Button()
            {
                Text = "Select File...",
                Location = new Point(offset, offset)
            };
            btnSelectFile.Click += btnSelectFile_Click;

            //btnSelectFolder
            btnSelectFolder = new Button() { Text = "Select Folder..." };
            btnSelectFolder.Click += btnSelectFolder_Click;

            //txtSource
            txtSource = new HintedTextBox("enter source file(s)")
            {
                Location = new Point(offset, btnSelectFile.Location.Y + btnSelectFile.Height + offset)
            };

            //txtPassword
            txtPassword = new HintedTextBox("enter password")
            {
                IsPasswordBox = true,
            };
            //a hack to make the hint show as normal text, not password chars:
            //after setting its IsPasswordBox = true, call HideHint(), then ShowHint()
            txtPassword.HideHint();
            txtPassword.ShowHint();

            //chckDeleteSource
            chckDeleteSource = new CheckBox()
            {
                Checked = false,
                Text = "Delete source files",
                TextAlign = ContentAlignment.MiddleLeft
            };

            //chckShowPassword
            chckShowPassword = new CheckBox()
            {
                Checked = false,
                Text = "Show password",
                TextAlign = ContentAlignment.MiddleLeft
            };
            chckShowPassword.CheckedChanged += chckShowPassword_CheckedChanged;

            //pnlControlPanel
            pnlControlPanel = new Panel();
            pnlControlPanel.BackColor = SystemColors.Control;
            pnlControlPanel.Dock = DockStyle.Fill;
            pnlControlPanel.Controls.AddRange(new Control[] { btnSelectFile, btnSelectFolder, txtSource });

            //listView
            listView = new ListView();
            listView.AllowColumnReorder = false;
            listView.CheckBoxes = false;
            listView.Dock = DockStyle.Fill;
            listView.FullRowSelect = true;
            listView.MultiSelect = false;
            listView.ShowItemToolTips = true;
            listView.View = View.Details;
            listView.Columns.Add("%", 20, HorizontalAlignment.Left);
            listView.Columns.Add("File", 100, HorizontalAlignment.Left);
            listView.Columns.Add("Message", 100, HorizontalAlignment.Left);

            //searchBox
            searchBox = new ToolStripTextBox() { };
            searchBox.ToolTipText = "search";
            searchBox.KeyUp += searchBox_KeyUp;

            //btnSaveLog
            btnSaveLog = new ToolStripButton()
            {
                //Image = Properties.Resources.Save,
                //ToolTipText = "Save Log"

                Text = "Save Log"
            };
            btnSaveLog.Click += btnSaveLog_Click;

            //stripListViewToolBar
            stripListViewToolBar = new ToolStrip();
            stripListViewToolBar.Dock = DockStyle.Fill;
            stripListViewToolBar.Items.Add(searchBox);
            stripListViewToolBar.Items.Add(new ToolStripSeparator());
            stripListViewToolBar.Items.Add(btnSaveLog);

            //tscListView
            tscListViewCont = new ToolStripContainer();
            tscListViewCont.Dock = DockStyle.Fill;
            tscListViewCont.TopToolStripPanel.Controls.Add(stripListViewToolBar);
            tscListViewCont.ContentPanel.Controls.Add(listView);

            //splitMain
            splitMain = new SplitContainer();
            splitMain.Dock = DockStyle.Fill;
            splitMain.BackColor = Color.Black;
            splitMain.IsSplitterFixed = true;
            splitMain.Orientation = Orientation.Vertical;
            splitMain.Panel1.Controls.Add(pnlControlPanel);
            splitMain.Panel2.Controls.Add(tscListViewCont);
            splitMain.SplitterDistance = 3;

            //this
            this.Controls.Add(splitMain);
            this.SizeChanged += WorkPage_Paint;
        }

        XmlNode CurrentParentNode = null;
        void AddEvent(FeedbackEventArgs eventArgs)
        {
            if (eventArgs != null)
            {
                XmlElement currentNode = null;

                if (CurrentParentNode == null)
                    CurrentParentNode = logDocument.LastChild;

                ListViewItem item = new ListViewItem();
                item.Text = eventArgs.WorkProgress.ToString();

                ListViewItem.ListViewSubItem fileItem = new ListViewItem.ListViewSubItem();
                fileItem.Text = Path.GetFileName(eventArgs.FilePath);

                ListViewItem.ListViewSubItem msgItem = new ListViewItem.ListViewSubItem();
                msgItem.Text = eventArgs.Message;

                if (eventArgs.Type == FeedbackType.Begin)
                {
                    currentNode = logDocument.CreateElement("File");
                    currentNode.SetAttribute("Path", eventArgs.FilePath);
                    CurrentParentNode.AppendChild(currentNode);
                    CurrentParentNode = currentNode;
                }
                else if (eventArgs.Type == FeedbackType.End)
                {
                    CurrentParentNode = CurrentParentNode.ParentNode;
                }
                else if (eventArgs.Type == FeedbackType.Error)
                {
                    item.ForeColor = Color.Red;

                    currentNode = logDocument.CreateElement("Error");
                    currentNode.SetAttribute("Progress", eventArgs.WorkProgress.ToString());
                    currentNode.InnerText = eventArgs.Message;
                    CurrentParentNode.AppendChild(currentNode);
                }
                else if (eventArgs.Type == FeedbackType.Information || eventArgs.Type == FeedbackType.Question)
                {
                    currentNode = logDocument.CreateElement(eventArgs.Type.ToString());
                    currentNode.SetAttribute("Progress", eventArgs.WorkProgress.ToString());
                    currentNode.InnerText = eventArgs.Message;
                    CurrentParentNode.AppendChild(currentNode);
                }
                else if (eventArgs.Type == FeedbackType.Warning)
                {
                    item.ForeColor = Color.Blue;

                    currentNode = logDocument.CreateElement("Warning");
                    currentNode.SetAttribute("Progress", eventArgs.WorkProgress.ToString());
                    currentNode.InnerText = eventArgs.Message;
                    CurrentParentNode.AppendChild(currentNode);
                }
                else if (eventArgs.Type == FeedbackType.Progress)
                {
                    currentNode = logDocument.CreateElement("Progress");
                    currentNode.SetAttribute("Progress", eventArgs.WorkProgress.ToString());
                    currentNode.SetAttribute("Message", eventArgs.Message);
                    CurrentParentNode.AppendChild(currentNode);
                }

                item.SubItems.Add(fileItem);
                item.SubItems.Add(msgItem);

                //item.Tag = eventArgs;
                listView.Items.Add(item);
            }
        }
        protected void ClearEvents()
        {
            InitLogDocument();
            listView.Items.Clear();
        }
        protected string[] GetSourceFiles()
        {
            List<string> sourceFiles = new List<string>();

            string sourcePath = txtSource.Text.Trim();

            if (File.Exists(sourcePath))
            {
                sourceFiles.Add(sourcePath);
                return sourceFiles.ToArray();
            }

            string dirPath = null, searchPattern = null;
            if (Directory.Exists(sourcePath))
            {
                dirPath = sourcePath;
                searchPattern = "*.*";
            }
            else
            {
                dirPath = sourcePath.Substring(0, sourcePath.LastIndexOf('\\'));
                if (!Directory.Exists(dirPath))
                    throw new Exception("Source file(s) could not be found.");
                searchPattern = sourcePath.Substring(sourcePath.LastIndexOf('\\') + 1);
            }

            DirectoryInfo dir = new DirectoryInfo(dirPath);
            var matchingFiles = dir.GetFiles(searchPattern, SearchOption.TopDirectoryOnly);
            sourceFiles.AddRange(matchingFiles.Where(f => f.Exists).Select(f => f.FullName));
            return sourceFiles.ToArray();
        }
        internal void HighlightEvents(string searchTerm)
        {
            foreach (ListViewItem item in listView.Items)
            {
                item.BackColor = Color.White;
                if (searchTerm != null && searchTerm.Trim() != string.Empty)
                {
                    foreach (ListViewItem.ListViewSubItem subItem in item.SubItems)
                    {
                        if (subItem.Text.ToLower().Contains(searchTerm.ToLower()))
                        {
                            item.BackColor = Color.Yellow;
                            break;
                        }
                    }
                }
            }
        }
        void InitLogDocument()
        {
            logDocument = new XmlDocument();
            var root = logDocument.CreateElement("Log");
            logDocument.AppendChild(root);
            CurrentParentNode = null;
        }

        void WorkPage_Paint(object sender, EventArgs e)
        {
            splitMain.SplitterDistance = this.Width / 3;

            btnSelectFile.Width = btnSelectFolder.Width = (pnlControlPanel.Width - (offset * 3)) / 2;

            btnSelectFolder.Location = new Point(btnSelectFile.Location.X + btnSelectFile.Width + offset, offset);

            txtSource.Width = pnlControlPanel.Width - (offset * 2);

            txtPassword.Width = ((pnlControlPanel.Width - (offset * 3)) * 5) / 7;

            chckShowPassword.Width = ((pnlControlPanel.Width - (offset * 3)) * 2) / 7;

            listView.Columns[0].Width = (listView.Width * 1) / 20;
            listView.Columns[1].Width = (listView.Width * 7) / 20;
            listView.Columns[2].Width = (listView.Width * 12) / 20;
        }

        void btnSaveLog_Click(object sender, EventArgs e)
        {
            if (logDocument.LastChild.HasChildNodes)
            {
                if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    logDocument.Save(saveDialog.FileName);
                }
            }
        }
        void btnSelectFile_Click(object sender, EventArgs e)
        {
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                txtSource.HideHint(); //a hack
                txtSource.Text = openDialog.FileName;
            }
        }
        void btnSelectFolder_Click(object sender, EventArgs e)
        {
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                txtSource.HideHint(); //a hack
                txtSource.Text = folderDialog.SelectedPath + "\\*.*";
            }
        }
        void chckShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.IsPasswordBox = !chckShowPassword.Checked;
        }
        void searchBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                HighlightEvents(searchBox.Text);
            }
        }
        void Steganographer_Feedback(object sender, FeedbackEventArgs e)
        {
            AddEvent(e);

            //Action<FeedbackEventArgs> action = new Action<FeedbackEventArgs>(AddEvent);
            //action.BeginInvoke(e, null, this);

            //await Task.Run(new Action<FeedbackEventArgs>(AddEvent));
        }
    }

    public class DecryptionPage : WorkPage
    {
        Button btnDecrypt;

        public DecryptionPage()
        {
            //btnDecrypt
            btnDecrypt = new Button()
            {
                BackColor = Color.Coral,
                ForeColor = Color.White,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Text = "Decrypt"
            };
            btnDecrypt.Click += btnDecrypt_Click;

            //pnlControlPanel
            pnlControlPanel.Controls.AddRange(new Control[] { txtPassword, chckShowPassword, chckDeleteSource, btnDecrypt });

            //this
            this.Text = "Decryption Page";
            this.SizeChanged += DecryptionPage_Paint;
        }

        void Decrypt()
        {
            try
            {
                if (txtSource.Text == null || txtSource.Text.Trim() == string.Empty)
                    throw new Exception("Please specify file(s) to decrypt.");

                if (string.IsNullOrEmpty(txtPassword.Text))
                    throw new Exception("Please specify the password for the encryption.");

                //get source files
                //for each source file
                //get the embedded file
                //if (delete source file), delete source file
                //save embedded file (ask user to choose file name)
                string[] sourceFiles = GetSourceFiles();
                if (sourceFiles.Length == 0)
                    throw new Exception("Source file(s) could not be found.");

                for (int index = 0; index < sourceFiles.Length; ++index)
                {
                    byte[] embeddedFile = Steganographer.Dec(sourceFiles[index], txtPassword.Text);
                    if (embeddedFile == null)
                        throw new Exception("Decryption Failed.");

                    if (chckDeleteSource.Checked)
                        File.Delete(sourceFiles[index]);

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        FileSystemServices.WriteBytesToFile(saveDialog.FileName, embeddedFile);
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox msg = new MsgBox(ex.Message, MessageType.Error, ResponseButtons.OK);
                msg.ShowDialog();
            }
        }

        void DecryptionPage_Paint(object sender, EventArgs e)
        {
            txtPassword.Location = new Point(offset, txtSource.Location.Y + txtSource.Height + offset);

            chckShowPassword.Location = new Point(txtPassword.Location.X + txtPassword.Width + offset, txtPassword.Location.Y);

            chckDeleteSource.Location = new Point(offset, txtPassword.Location.Y + txtPassword.Height + offset);
            chckDeleteSource.Width = txtPassword.Width;

            btnDecrypt.Width = chckShowPassword.Width;
            btnDecrypt.Location = new Point(chckDeleteSource.Location.X + chckDeleteSource.Width + offset, chckDeleteSource.Location.Y);
        }

        void btnDecrypt_Click(object sender, EventArgs e)
        {
            ClearEvents();
            Decrypt();
        }
    }

    public class EncryptionPage : WorkPage
    {
        Button btnEncrypt;
        HintedTextBox txtConfirmPwd;
        PictureBox picMaskImage;

        //encryption/decryption values:
        string maskImagePath;

        public EncryptionPage()
        {
            //picMaskImage
            picMaskImage = new PictureBox()
            {
                BackColor = Color.White,
                BackgroundImageLayout = ImageLayout.Stretch,
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
                Location = new Point(offset, txtSource.Location.Y + txtSource.Height + offset)
            };
            picMaskImage.Click += picMaskImage_Click;

            //chckShowPassword
            chckShowPassword.CheckedChanged += chckShowPassword_CheckedChanged;

            //txtConfirmPwd
            txtConfirmPwd = new HintedTextBox("confirm password")
            {
                IsPasswordBox = true,
            };
            //a hack to make the hint show as normal text, not password chars:
            //after setting its IsPasswordBox = true, call HideHint(), then ShowHint()
            txtConfirmPwd.HideHint();
            txtConfirmPwd.ShowHint();

            //btnEncrypt
            btnEncrypt = new Button()
            {
                BackColor = Color.Coral,
                ForeColor = Color.White,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Text = "Encrypt"
            };
            btnEncrypt.Click += btnEncrypt_Click;

            //pnlControlPanel
            pnlControlPanel.Controls.AddRange(new Control[] { picMaskImage,
                chckDeleteSource, txtPassword, chckShowPassword, txtConfirmPwd, btnEncrypt });

            //this
            this.Text = "Encryption Page";
            this.SizeChanged += EncryptionPage_Paint;
        }

        void Encrypt()
        {
            try
            {
                if (txtSource.Text == null || txtSource.Text.Trim() == string.Empty)
                    throw new Exception("Please specify file(s) to encrypt.");

                if (maskImagePath == null || maskImagePath.Trim() == string.Empty)
                    throw new Exception("Please click the large picture box to select the picture to use for masking.");

                if (string.IsNullOrEmpty(txtPassword.Text))
                    throw new Exception("Please specify the password for the encryption.");
                if (string.IsNullOrEmpty(txtConfirmPwd.Text))
                    throw new Exception("Please confirm the password for the encryption.");
                if (txtConfirmPwd.Text != txtPassword.Text)
                    throw new Exception("You supplied two different passwords.");

                //get source files
                //for each source file
                //generate the modified image
                //if (delete source file), delete source file
                //save modified image with automatically gened name (if file with name exists, raise alarm, ask user to choose)

                string[] sourceFiles = GetSourceFiles();
                if (sourceFiles.Length == 0)
                    throw new Exception("Source file(s) could not be found.");

                for (int index = 0; index < sourceFiles.Length; ++index)
                {
                    Bitmap bitmap = Steganographer.Enc(sourceFiles[index], maskImagePath, txtPassword.Text);
                    if (bitmap == null)
                        throw new Exception("Encryption Failed.");

                    if (chckDeleteSource.Checked)
                        File.Delete(sourceFiles[index]);

                    string filePath = sourceFiles[index];
                    string autoGenedName = filePath.Substring(0, filePath.LastIndexOf('\\')) + "\\" +
                        Path.GetFileNameWithoutExtension(sourceFiles[index]) + Path.GetExtension(maskImagePath);
                    if (File.Exists(autoGenedName))
                    {
                        if (saveDialog.ShowDialog() == DialogResult.OK)
                        {
                            bitmap.Save(saveDialog.FileName);//, ImageFormat.Png);
                        }
                    }
                    else
                    {
                        bitmap.Save(autoGenedName);
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox msg = new MsgBox(ex.Message, MessageType.Error, ResponseButtons.OK);
                msg.ShowDialog();
            }
        }

        void EncryptionPage_Paint(object sender, EventArgs e)
        {
            picMaskImage.Width = picMaskImage.Height = chckDeleteSource.Width = txtSource.Width;

            chckDeleteSource.Location = new Point(offset, picMaskImage.Location.Y + picMaskImage.Height + offset);

            txtConfirmPwd.Width = txtPassword.Width;
            txtPassword.Location = new Point(offset, chckDeleteSource.Location.Y + chckDeleteSource.Height + offset);

            chckShowPassword.Location = new Point(txtPassword.Location.X + txtPassword.Width + offset, txtPassword.Location.Y);

            txtConfirmPwd.Location = new Point(offset, txtPassword.Location.Y + txtPassword.Height + offset);

            btnEncrypt.Width = chckShowPassword.Width;
            btnEncrypt.Location = new Point(txtConfirmPwd.Location.X + txtConfirmPwd.Width + offset, txtConfirmPwd.Location.Y);
        }
        void btnEncrypt_Click(object sender, EventArgs e)
        {
            ClearEvents();
            Encrypt();
        }
        void picMaskImage_Click(object sender, EventArgs e)
        {
            try
            {
                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    picMaskImage.BackgroundImage = Image.FromFile(openDialog.FileName);
                    maskImagePath = openDialog.FileName;
                }
            }
            catch (Exception ex)
            {
                MsgBox msg = new MsgBox(ex.Message, MessageType.Error, ResponseButtons.OK);
                msg.ShowDialog();
            }
        }
        void chckShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtConfirmPwd.IsPasswordBox = !chckShowPassword.Checked;
        }
    }
}