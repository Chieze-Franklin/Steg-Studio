using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Karabow.Steg.Info;

namespace Karabow.Steg.GUI
{
    public partial class PopUpWindow : Form
    {
        Button btnOK;
        Button btnCancel;
        Button btnYes;
        Button btnNo;
        Button btnSave;

        TableLayoutPanel tblDivisions;
        PictureBox picBanner;
        Panel pnlBody;
        Panel pnlResponse;

        public PopUpWindow(MessageType messagetype, ResponseButtons responseButtons)
        {
            InitializeComponent();

            InitializeMyComponents();

            //this.ParentForm = parentForm;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowInTaskbar = false;
            this.Icon = Karabow.Steg.GUI.Properties.Resources.Icon;
            this.Text = App.Name;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.TopMost = true;
            this.AutoSize = true;

            //picBanner
            picBanner.Dock = DockStyle.Fill;
            picBanner.Image = this.BannerImage;
            picBanner.SizeMode = PictureBoxSizeMode.StretchImage;
            //picBanner.BackColor = Color.White;

            //pnlResponce
            //pnlResponse.Dock = DockStyle.Fill;
            pnlResponse.Anchor = AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left;
            //pnlResponse.BackColor = Color.Black;

            //pnlBody
            pnlBody.Dock = DockStyle.Fill;
            pnlBody.AutoSize = true;
            //pnlBody.BackColor = Color.Red;



            //tblDivisions
            //tblDivisions.Size = new Size(50, 50);
            tblDivisions.AutoSize = true;
            //tblDivisions.Height = picBanner.Height + pnlBody.Height + pnlResponse.Height + 8;
            tblDivisions.Dock = DockStyle.Fill;
            tblDivisions.RowCount = 3;
            tblDivisions.Controls.Add(picBanner, 0, 0);
            tblDivisions.Controls.Add(pnlBody, 0, 1);
            tblDivisions.Controls.Add(pnlResponse, 0, 2);
            //tblDivisions.BackColor = Color.Green;

            //this2
            this.Controls.Add(tblDivisions);
            //tblDivisions.Dock = DockStyle.Fill;
            //this.Height = tblDivisions.Height;// = picBanner.Height + pnlBody.Height + pnlResponse.Height + 8;
            //this.ControlAdded += new ControlEventHandler(PopUpWindow_ControlAdded);
            //this.Paint += new PaintEventHandler(tblDivisions_Paint);

            this.MsgResponse = Response.Close;
            SetMessageType(messagetype);
            SetResponseButtons(responseButtons);
        }

        void tblDivisions_Paint(object sender, PaintEventArgs e)
        {
            //tblDivisions.Dock = DockStyle.Fill;
            picBanner.Height = 50;
            //pnlResponse.Width = tblDivisions.Width;
            pnlResponse.Height = 30;
            //this.Height = picBanner.Height + pnlBody.Height + pnlResponse.Height + 8; //tblDivisions.Height;
        }

        void InitializeMyComponents()
        {
            btnOK = new Button();
            btnCancel = new Button();
            btnYes = new Button();
            btnNo = new Button();
            btnSave = new Button();

            tblDivisions = new TableLayoutPanel();
            picBanner = new PictureBox();
            pnlBody = new Panel();
            pnlResponse = new Panel();

            //btnOK
            btnOK.Text = "OK";
            btnOK.Click += new EventHandler(btnOK_Click);

            //btnCancel
            btnCancel.Text = "Cancel";
            btnCancel.Click += new EventHandler(btnCancel_Click);

            //btnYes
            btnYes.Text = "Yes";
            btnYes.Click += new EventHandler(btnYes_Click);

            //btnNo
            btnNo.Text = "No";
            btnNo.Click += new EventHandler(btnNo_Click);

            //btnSave
            btnSave.Text = "Save";
            btnSave.Click += new EventHandler(btnSave_Click);
        }

        public void AddToBody(Control control)
        {
            pnlBody.Controls.Add(control);
        }

        public void RemoveFromBody(Control control)
        {
            pnlBody.Controls.Remove(control);
            tblDivisions.Dock = DockStyle.None;
            this.Height = tblDivisions.Height = picBanner.Height + pnlBody.Height + pnlResponse.Height + 8;
            this.Width = tblDivisions.Width;
        }

        public bool ContainsInBody(Control control)
        {
            if (pnlBody.Controls.Contains(control))
                return true;
            else
                return false;
        }

        public void AddToResponse(Control control)
        {
            pnlResponse.Width += control.Width;
            pnlResponse.Controls.Add(control);
        }

        void btnNo_Click(object sender, EventArgs e)
        {
            MsgResponse = Response.No;
            this.Close();
        }

        void btnYes_Click(object sender, EventArgs e)
        {
            MsgResponse = Response.Yes;
            this.Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            MsgResponse = Response.Cancel;
            this.Close();
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            MsgResponse = Response.OK;
            this.Close();
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            MsgResponse = Response.Save;
            this.Close();
        }

        void SetResponseButtons(ResponseButtons responseButtons)
        {
            if (responseButtons == ResponseButtons.OK)
            {
                btnOK.Location = new Point(2, 2);
                this.AddToResponse(btnOK);
            }
            else if (responseButtons == ResponseButtons.OKCancel)
            {
                btnOK.Location = new Point(2, 2);
                btnCancel.Location = new Point(btnOK.Width + 4, 2);
                this.AddToResponse(btnOK);
                this.AddToResponse(btnCancel);
            }
            else if (responseButtons == ResponseButtons.SaveCancel)
            {
                btnSave.Location = new Point(2, 2);
                btnCancel.Location = new Point(btnSave.Width + 4, 2);
                this.AddToResponse(btnSave);
                this.AddToResponse(btnCancel);
            }
            else if (responseButtons == ResponseButtons.YesNo)
            {
                btnYes.Location = new Point(2, 2);
                btnNo.Location = new Point(btnYes.Width + 4, 2);
                this.AddToResponse(btnYes);
                this.AddToResponse(btnNo);
            }
            else if (responseButtons == ResponseButtons.YesNoCancel)
            {
                btnYes.Location = new Point(2, 2);
                btnNo.Location = new Point(btnYes.Width + 4, 2);
                btnCancel.Location = new Point(btnYes.Width + btnNo.Width + 6, 2);
                this.AddToResponse(btnYes);
                this.AddToResponse(btnNo);
                this.AddToResponse(btnCancel);
            }
        }

        void SetMessageType(MessageType msgType)
        {
            if (msgType == MessageType.Error)
            {
                this.BannerImage = Karabow.Steg.GUI.Properties.Resources.ErrorImage;
            }
            else if (msgType == MessageType.Information)
            {
                this.BannerImage = Karabow.Steg.GUI.Properties.Resources.InfoImage;
            }
            else if (msgType == MessageType.Warning)
            {
                this.BannerImage = Karabow.Steg.GUI.Properties.Resources.WarningImage;
            }
            else if (msgType == MessageType.Question)
            {
                this.BannerImage = Karabow.Steg.GUI.Properties.Resources.QuestionImage;
            }
        }

        public virtual void InitializeOtherComponents() { }

        //PROPERTIES
        Image bannerImage;
        public Image BannerImage
        {
            get { return bannerImage; }
            set { bannerImage = value; picBanner.Image = bannerImage; }
        }

        //PROPERTIES
        Response msgResponse;
        public Response MsgResponse
        {
            get { return msgResponse; }
            set { msgResponse = value; }
        }
    }

    public class DialogForm : PopUpWindow
    {
        public DialogForm(MessageType messageType, ResponseButtons responseButtons)
            : base(messageType, responseButtons)
        { }
    }

    public class MsgBox : DialogForm
    {
        Label lblInfo = new Label();
        Label lblMore = new Label();
        Button btnMore = new Button();
        #region Constructors
        public MsgBox(string info, string more, MessageType messagetype, ResponseButtons responseButtons)
            : base(messagetype, responseButtons)
        {

            //lblInfo
            lblInfo.AutoSize = true;
            lblInfo.Font = new Font("Microsoft Sans Serif", 10.0F, FontStyle.Regular);
            lblInfo.Text = info;
            lblInfo.Location = new Point(2, 2);

            //lblMore
            lblMore.AutoSize = true;
            lblMore.Font = new Font("Microsoft Sans Serif", 10.0F, FontStyle.Regular);
            lblMore.Text = more;
            //lblMore.Visible = false;
            //lblMore.Location = new Point(6, lblInfo.Height + 2);

            SetMore(more);

            //btnMore
            btnMore.Font = new Font("Times New Roman", btnMore.Font.Size, btnMore.Font.Style);
            btnMore.Text = "Show More" + Convert.ToChar(8595).ToString();
            btnMore.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMore.Location = new Point(this.Width - 100, 2);
            btnMore.Click += new EventHandler(btnMore_Click);


            //this
            this.AddToBody(lblInfo);
            //this.ShowDialog();
        }

        public MsgBox(string info, string more, MessageType messagetype)
            : this(info, more, messagetype, ResponseButtons.OK)
        { }

        public MsgBox(string info, string more, ResponseButtons responseButtons)
            : this(info, more, MessageType.Information, responseButtons)
        { }

        public MsgBox(string info, string more)
            : this(info, more, MessageType.Information, ResponseButtons.OK)
        { }

        public MsgBox(string info, MessageType messagetype, ResponseButtons responseButtons)
            : this(info, "", messagetype, responseButtons)
        { }

        public MsgBox(string info, MessageType messagetype)
            : this(info, "", messagetype, ResponseButtons.OK)
        { }

        public MsgBox(string info, ResponseButtons responseButtons)
            : this(info, "", MessageType.Information, responseButtons)
        { }

        public MsgBox(string info)
            : this(info, "", MessageType.Information, ResponseButtons.OK)
        { }
        #endregion

        void btnMore_Click(object sender, EventArgs e)
        {
            if (this.ContainsInBody(lblMore) == false)
            {
                //lblMore.Visible = true;
                lblMore.Location = new Point(2, lblInfo.Height + 2);
                this.AddToBody(lblMore);
                btnMore.Text = "Hide More" + Convert.ToChar(8593).ToString(); //this.Height = 500;
                //ReDrawWindow();
            }
            else
            {
                //lblMore.Visible = false;
                this.RemoveFromBody(lblMore);
                btnMore.Text = "Show More" + Convert.ToChar(8595).ToString();
                //ReDrawWindow();
            }
        }

        void SetMore(string strMore)
        {
            if (strMore.Trim() != "")
            {
                this.AddToResponse(btnMore);
            }
        }
    }

    public enum MessageType
    {
        Information,
        Question,
        Warning,
        Error
    };

    public enum ResponseButtons
    {
        OK,
        OKCancel,
        YesNo,
        YesNoCancel,
        SaveCancel
    }

    public enum Response
    {
        OK,
        Cancel,
        Yes,
        No,
        Save,
        Close
    };
}
