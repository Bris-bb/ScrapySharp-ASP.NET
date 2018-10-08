using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace UtilScrap
{
    public partial class Form1 : Form
    {
        string url = null;
        bool flg_start = false, flg_end = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate(textBox1.Text);
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            timer1.Enabled = false;
            timer1.Enabled = true;
            url = e.Url.ToString();
            textBox2.AppendText("timer reset"+ "\n");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            textBox2.AppendText(url);
            if (url.Contains("Login"))
            {
                //when login
                webBrowser1.Document.GetElementById("Username").InnerText = ConfigurationSettings.AppSettings["UserName"]; // "carermate"; //txtUserName.Text.ToString();
                webBrowser1.Document.GetElementById("Password").InnerText = ConfigurationSettings.AppSettings["Password"]; //  "321?Test"; // txtPass.Text.ToString();
                webBrowser1.Document.GetElementById("Login").InvokeMember("click");
            }
            else if (url.Contains("Search") && flg_start && !flg_end)
            {
                //when scrap
                flg_end = true;
                this.Invoke(new MethodInvoker(delegate
                {
                    richTxt1.Text = richTxt1.Text + "starting..." + Environment.NewLine;
                }));

                //Initializing htmlagility document
                var doc = webBrowser1.Document;
                var tbl = doc.GetElementById("tblVerificationResults");


                
                var tds = tbl.GetElementsByTagName("tr")[1].GetElementsByTagName("td");

                if (tds != null)
                {
                    for (int i = 0; i < tds.Count; i++)
                    {
                        this.Invoke(new MethodInvoker(delegate
                        {
                            var txt = tds[i].InnerText;
                            richTxt1.Text = richTxt1.Text + "title: " + txt + Environment.NewLine;
                        }));
                    }
                }
            }
            else if (url.Contains("Search") && !flg_start && !flg_end)
            {
                //when verify
                webBrowser1.Document.GetElementById("Criteria_0__FamilyName").InnerText = ConfigurationSettings.AppSettings["FamilyName"]; //  "BAROVS"; //txtUserName.Text.ToString();
                webBrowser1.Document.GetElementById("Criteria_0__BirthDate").SetAttribute("value", ConfigurationSettings.AppSettings["BirthDate"]); //  "01/02/1991");
                webBrowser1.Document.GetElementById("Criteria_0__AuthorisationNumber").InnerText = ConfigurationSettings.AppSettings["AuthorisationNumber"]; //  "WWC0368776E"; // txtPass.Text.ToString();
                webBrowser1.Document.GetElementById("Verify").InvokeMember("click");
                flg_start = true;
            }
            
            textBox2.AppendText("load complete" + "\n");
        }
         
    }
}
