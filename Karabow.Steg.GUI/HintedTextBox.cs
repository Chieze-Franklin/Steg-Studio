using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Karabow.Steg.GUI
{
    public class HintedTextBox : TextBox
    {
        bool waterMarkActive = true, isPwdBox = false;

        public HintedTextBox(string hint) 
        {
            Hint = hint;

            this.GotFocus += HintedTextBox_GotFocus;
            this.LostFocus += HintedTextBox_LostFocus;

            ShowHint();
        }

        void HintedTextBox_GotFocus(object sender, EventArgs e)
        {
            HideHint();
        }

        void HintedTextBox_LostFocus(object sender, EventArgs e)
        {
            ShowHint();
        }

        void HintedTextBox_TextChanged(object sender, EventArgs e)
        {
            if (Text == string.Empty)
                ShowHint();
        }

        public void HideHint() 
        {
            if (waterMarkActive)
            {
                if (IsPasswordBox)
                    this.UseSystemPasswordChar = true;
                waterMarkActive = false;
                this.ForeColor = Color.Black;
                this.Text = "";
            }
        }
        public void ShowHint()
        {
            if (Text == null || Text == string.Empty)
            {
                if (Hint != null)
                {
                    this.UseSystemPasswordChar = false;
                    waterMarkActive = true;
                    this.ForeColor = Color.Gray;
                    this.Text = Hint;
                }
            }
        }

        public string Hint { get; set; }
        public bool IsPasswordBox 
        { 
            get { return isPwdBox; } 
            set 
            {
                isPwdBox = value;
                if(!waterMarkActive)
                    UseSystemPasswordChar = value;
            }
        }
        public override string Text
        {
            get
            {
                if (waterMarkActive)
                    return "";
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }
    }
}
