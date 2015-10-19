using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Karabow.Steg.Core;
using System.Drawing;

namespace Karabow.Steg.GUI
{
    public class HelpTopicsButton : Button
    {
        FileInfo fileInfo;
        ToolTip ttpText;

        public HelpTopicsButton(string filePath)
        {
            fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists)
                throw new FileNotFoundException("The file\n" + fileInfo.FullName + "\ncould not be found");

            ttpText = new ToolTip();
            ttpText.BackColor = Color.White;
            ttpText.ForeColor = Color.Blue;
            ttpText.ToolTipIcon = ToolTipIcon.Info;
            ttpText.UseAnimation = true;
            ttpText.UseFading = true;

            this.Text = FileName;
            this.AutoEllipsis = true;
            this.Click += new EventHandler(HelpTopicsButton_Click);
            //this.MouseEnter += new EventHandler(HelpTopicsButton_MouseEnter);
            //this.MouseLeave += new EventHandler(HelpTopicsButton_MouseLeave);
        }

        void HelpTopicsButton_Click(object sender, EventArgs e)
        {
            try
            {
                currentNavigator.NavigateTo(fileInfo.FullName);
            }
            catch { }
        }

        void HelpTopicsButton_MouseEnter(object sender, EventArgs e)
        {
            ttpText.ToolTipTitle = FileName;
            string toolTipText =
                "File Location: " + FileLocation + "\n" +
                "File Size: " + FileSizeInBytes.ToString() + " bytes\n" +
                "Time Created: " + TimeCreated.DayOfWeek.ToString() + ", " +
                TimeCreated.Date.ToString().Substring(0, TimeCreated.Date.ToString().IndexOf(' ')) +
                " at exactly " + TimeCreated.TimeOfDay.ToString() + "\n" +
                "Time Last Accessed: " + TimeLastAccessed.ToString() + "\n" +
                "Time Last Modified: " + TimeLastModified.ToString() + "\n\n" +
                "Content: \n";

            string[] fileTextzz = FileText.Split('\n');
            if (fileTextzz.Length > 10)
            {
                for (int n = 0; n < 10; n++)
                {
                    toolTipText += fileTextzz[n] + "\n";
                }
                toolTipText += "...";
            }
            else
            {
                foreach (string txt in fileTextzz)
                {
                    toolTipText += txt + "\n";
                }
            }
            toolTipText = toolTipText.Trim('\n');

            ttpText.Show(toolTipText, this, this.Width / 2, this.Height, 10000);
        }

        void HelpTopicsButton_MouseLeave(object sender, EventArgs e)
        {
            ttpText.Hide(this);
        }

        #region PROPERTIES
        string FileName
        {
            get { return fileInfo.Name; }
        }

        string FileLocation
        {
            get { return fileInfo.Directory.FullName; }
        }

        long FileSizeInBytes
        {
            get { return fileInfo.Length; }
        }

        DateTime TimeCreated
        {
            get { return fileInfo.CreationTime; }
        }

        DateTime TimeLastAccessed
        {
            get { return fileInfo.LastAccessTime; }
        }

        DateTime TimeLastModified
        {
            get { return fileInfo.LastWriteTime; }
        }

        string FileText
        {
            get
            {
                string fileText = FileSystemServices.ReadTextFromFile(fileInfo.ToString());
                return fileText;
            }
        }

        Navigator currentNavigator;
        public Navigator CurrentNavigator
        {
            get
            {
                return currentNavigator;
            }
            set { currentNavigator = value; }
        }
        #endregion
    }
}
