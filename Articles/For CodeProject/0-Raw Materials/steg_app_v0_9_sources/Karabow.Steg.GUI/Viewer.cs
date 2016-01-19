using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Karabow.Steg.GUI
{
    public class Viewer : WebBrowser
    {
        public Viewer()
        {
            this.AllowWebBrowserDrop = true;
            this.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(Viewer_DocumentCompleted);
        }

        void Viewer_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            this.Refresh();
        }

        public void UpdateFromText(string text)
        {
            this.DocumentText = text;
        }

        public void UpdateFromUrl(Uri url)
        {
            this.Navigate(url);
        }

        public void UpdateFromUrl(string url)
        {
            this.Navigate(url);
        }
    }
}
