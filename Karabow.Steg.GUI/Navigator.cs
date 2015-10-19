using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Karabow.Steg.Info;

namespace Karabow.Steg.GUI
{
    /// <summary>
    /// represents a browser navigator bar
    /// </summary>
    public partial class Navigator : UserControl
    {
        Button btnBrowse;
        Button btnGo;
        ComboBox cbxUrl;

        ToolTip ttpText;

        /// <summary>
        /// creates an instance
        /// </summary>
        public Navigator()
        {
            InitializeComponent();

            //lblUrl
            btnBrowse = new Button();
            btnBrowse.Text = "...";
            //btnBrowse.Font = new Font("Times New Roman", 12.0F, FontStyle.Bold);
            btnBrowse.Click += new EventHandler(btnBrowse_Click);
            btnBrowse.MouseEnter += new EventHandler(btnBrowse_MouseEnter);
            btnBrowse.MouseLeave += new EventHandler(btnBrowse_MouseLeave);

            //cbxUrl
            cbxUrl = new ComboBox();
            cbxUrl.AutoCompleteMode = AutoCompleteMode.Suggest;
            cbxUrl.AutoCompleteSource = AutoCompleteSource.AllSystemSources;
            cbxUrl.KeyUp += new KeyEventHandler(cbxUrl_KeyUp);
            cbxUrl.MouseEnter += new EventHandler(cbxUrl_MouseEnter);
            cbxUrl.MouseLeave += new EventHandler(cbxUrl_MouseLeave);
            cbxUrl.TextChanged += new EventHandler(cbxUrl_MouseLeave);

            //btnGo
            btnGo = new Button();
            btnGo.Image = Properties.Resources.NavigateTo;
            btnGo.Click += new EventHandler(btnGo_Click);
            btnGo.MouseEnter += new EventHandler(btnGo_MouseEnter);
            btnGo.MouseLeave += new EventHandler(btnGo_MouseLeave);

            //ttpText
            ttpText = new ToolTip();
            ttpText.BackColor = Color.Yellow;
            ttpText.ForeColor = Color.Blue;
            ttpText.ToolTipIcon = ToolTipIcon.Info;
            //ttpText.ToolTipTitle = "View All Tags";
            ttpText.UseAnimation = true;
            ttpText.UseFading = true;
            //ttpText.IsBalloon = true;

            //this
            this.Controls.Add(btnBrowse);
            this.Controls.Add(cbxUrl);
            this.Controls.Add(btnGo);
            this.Paint += new PaintEventHandler(Navigator_Paint);
        }

        void cbxUrl_MouseEnter(object sender, EventArgs e)
        {
            ttpText.ToolTipTitle = "Chosen Url";
            ttpText.Show(cbxUrl.Text, cbxUrl, 0, cbxUrl.Height, 5000);
        }

        void cbxUrl_MouseLeave(object sender, EventArgs e)
        {
            ttpText.Hide(cbxUrl);
        }

        void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Title = App.Name + " - Open";
            open.DefaultExt = ".html";
            open.AddExtension = true;
            open.Filter =
                "HTML file (.html)|*.html|HTML file (.htm)|*.htm|All files|*.*";
            if (open.ShowDialog() == DialogResult.OK)
            {
                cbxUrl.Text = open.FileName;
            }
        }

        void btnBrowse_MouseEnter(object sender, EventArgs e)
        {
            ttpText.ToolTipTitle = "Select A Url";
            ttpText.Show(
                "Click me to select a url you wish to navigate to.\n" +
                "The url you selected should appear in the combobox by my side.",
                btnBrowse, 0, btnBrowse.Height, 10000);
        }

        void btnBrowse_MouseLeave(object sender, EventArgs e)
        {
            ttpText.Hide(btnBrowse);
        }

        void btnGo_Click(object sender, EventArgs e)
        {
            Navigate();
        }

        void btnGo_MouseEnter(object sender, EventArgs e)
        {
            ttpText.ToolTipTitle = "Navigate To Selected Url";
            ttpText.Show(
                "Click me to navigate to the selected url.\n" +
                "Please ensure you have selected a url before clicking me\n" +
                "(the name of your selected url should appear in the combobox by my side).",
                btnGo, 0, btnGo.Height, 10000);
        }

        void btnGo_MouseLeave(object sender, EventArgs e)
        {
            ttpText.Hide(btnGo);
        }

        void cbxUrl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Navigate();
        }

        public void Navigate()
        {
            if (cbxUrl.Text.Trim() == "")
                return;

            this.CurrentBrowser.Navigate(cbxUrl.Text);
        }

        public void NavigateTo(string url)
        {
            cbxUrl.Text = url;
            Navigate();
        }

        void Navigator_Paint(object sender, PaintEventArgs e)
        {
            //cbxUrl
            cbxUrl.Size = new Size(this.Width - (30 + 30), this.Height);
            cbxUrl.Location = new Point(btnBrowse.Location.X + btnBrowse.Width, this.Height - cbxUrl.Height);

            //btnGo
            btnGo.Size = new Size(30, cbxUrl.Height);
            btnGo.Location = new Point(this.Width - btnGo.Width, cbxUrl.Location.Y);

            //lblUrl
            btnBrowse.Location = new Point(0, cbxUrl.Location.Y);
            btnBrowse.Size = new Size(30, cbxUrl.Height);
        }

        #region PROPERTIES
        Viewer viewer = new Viewer();

        public Viewer CurrentBrowser
        {
            get { return viewer; }
            set { viewer = value; }
        }

        public new string Text
        {
            get { return cbxUrl.Text; }
            set
            {
                if (cbxUrl.Items.Contains(value))
                    cbxUrl.Items.Remove(value);
                cbxUrl.Items.Insert(0, value);
                cbxUrl.Text = value;
            }
        }
        #endregion
    }
}
