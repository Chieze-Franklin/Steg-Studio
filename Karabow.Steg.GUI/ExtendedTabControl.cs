using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Karabow.Steg.GUI
{
    /// <summary>
    /// represents a customized tab control
    /// </summary>
    public class ExtendedTabControl : TabControl
    {
        TabPage currentPage;
        TabPage lastSelectedPage;
        int tabSelectedNum = 0;
        //ImageList imglst;

        #region METHODS
        /// <summary>
        /// creates an instance of the control
        /// </summary>
        public ExtendedTabControl()
        {

            this.HotTrack = true;
            //this.Appearance = TabAppearance.Buttons;
            this.ControlAdded += new ControlEventHandler(ExtendedTabControl_ControlAdded);
            this.ControlRemoved += new ControlEventHandler(ExtendedTabControl_ControlRemoved);
            this.SelectedIndexChanged += new EventHandler(ExtendedTabControl_SelectedIndexChanged);
            //this.KeyDown += new KeyEventHandler(ExtendedTabControl_KeyDown);
            //this.KeyUp += new KeyEventHandler(ExtendedTabControl_KeyUp);
        }

        void ExtendedTabControl_ControlAdded(object sender, ControlEventArgs e)
        {
            if (e.Control is TabPage)
            {
                int n = this.TabPages.IndexOf(e.Control as TabPage);
                try
                {
                    this.SelectedIndex = n; //this line calls the ExtendedTabControl_SelectedIndexChanged method
                    //and saves me the stress of repeating the codes in the method
                    if (this.FindForm() != null)
                    {
                        e.Control.BackColor = this.FindForm().BackColor;
                    }
                }
                catch
                { }
            }
        }

        void ExtendedTabControl_ControlRemoved(object sender, ControlEventArgs e)
        {
            //this.lastSelectedPage = null;
            try
            {
                this.TabPages.Remove(this.CurrentPage);
            }
            catch { }
            if (this.TabPages.Count > 0)
            {
                if (this.TabPages[0] is ExtendedTabPage)
                {
                    (this.TabPages[0] as ExtendedTabPage).TabSelectedNumber = 0;//this single line of code is what
                    //makes this method and the logic
                    //behind it to work.
                    //Funny enough, I got the idea 
                    //immediately after Tuesday's Shiloh
                    //2010 Night Service after all the
                    //hours of intellectual struggles
                    this.lastSelectedPage = this.TabPages[0];
                }
            }
            if (this.TabPages.Count > 1)
            {
                if (this.TabPages[0] is ExtendedTabPage)
                {
                    //(this.TabPages[0] as ExtendedTabPage).TabSelectedNumber = 0;//this single line of code is what
                    //                                                            //makes this method and the logic
                    //                                                            //behind it to work.
                    //                                                            //Funny enough, I got the idea 
                    //                                                            //immediately after Tuesday's Shiloh
                    //                                                            //2010 Night Service after all the
                    //                                                            //hours of intellectual struggles
                    //this.lastSelectedPage = this.TabPages[0];
                    for (int n = 1; n < this.TabPages.Count; n++)
                    {
                        if (this.TabPages[n] is ExtendedTabPage)
                        {
                            if ((this.TabPages[n] as ExtendedTabPage).TabSelectedNumber >
                                (this.lastSelectedPage as ExtendedTabPage).TabSelectedNumber)
                                this.lastSelectedPage = this.TabPages[n];
                        }
                    }
                }
            }

            if (this.lastSelectedPage != null)
            {
                int n = this.TabPages.IndexOf(this.lastSelectedPage);
                try
                {
                    this.SelectedIndex = n; //this line calls the ExtendedTabControl_SelectedIndexChanged method
                    //and saves me the stress of repeating the codes in the method
                }
                catch
                { }
            }
            //else
            //{
            //    int n = 0;
            //    try
            //    {
            //        this.SelectedIndex = n; //this line calls the ExtendedTabControl_SelectedIndexChanged method
            //        //and saves me the stress of repeating the codes in the method
            //    }
            //    catch
            //    { }
            //}
        }

        void ExtendedTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            int n = this.SelectedIndex;
            this.CurrentPage = this.TabPages[n];

            if (this.CurrentPage is ExtendedTabPage)
            {
                (this.CurrentPage as ExtendedTabPage).TabSelectedNumber = ++tabSelectedNum;
            }
        }
        #endregion

        #region PROPERTIES
        /// <summary>
        /// gets and sets the current tabpage on this control
        /// </summary>
        public TabPage CurrentPage
        {
            get { return currentPage; }
            set { currentPage = value; }
        }

        WinForm Window
        {
            get { return (WinForm)this.FindForm(); }
        }
        #endregion
    }
}
