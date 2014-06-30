using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using me2day.api;
using System.Xml;

namespace me2dayBackup
{
    public partial class me2dayLoginForm : Form
    {
        private static Me2API api;
        private static string me2Token = String.Empty;
        Form1 parentForm;

        public me2dayLoginForm(Form1 frm)
        {
            InitializeComponent();

            this.parentForm = frm;
            api = new Me2API();
            api.AppKey = "296a4e7d66d3b4f6f786af9b5517a048";
            
            me2dayAuth();
        }

        private void me2dayAuth()
        {
            XmlDocument xDocAuthResult = api.GetAuthDesktopUrl(api.AppKey);
            XmlNodeList xNodeURL = xDocAuthResult.GetElementsByTagName("url");
            XmlNodeList xNodeToken = xDocAuthResult.GetElementsByTagName("token");
            me2Token = xNodeToken[0].InnerText; // 토큰 저장
            Uri authURL = new Uri(xNodeURL[0].InnerText); // 인증주소 저장

            wbMe2dayLogin.Url = authURL;
        }

        private void wbMe2dayLogin_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            HtmlDocument doc = wbMe2dayLogin.Document;
            if (doc != null)
            {
                HtmlElementCollection eleCol = doc.All.GetElementsByName("X-ME2API-AUTH-RESULT");
                if (eleCol.Count > 0)
                {
                    HtmlElement ele = eleCol[0];
                    string content = ele.GetAttribute("content");
                    if (content.ToLower().Equals("accepted"))
                    {
                        XmlDocument xDocFullToken = api.getSessionKey(me2Token);
                        XmlNodeList xNodeUserID = xDocFullToken.GetElementsByTagName("user_id");
                        XmlNodeList xNodeFullToken = xDocFullToken.GetElementsByTagName("full_auth_token");

                        string me2dayUserId = xNodeUserID[0].InnerText;
                        parentForm.Controls["tbMe2dayID"].Text = me2dayUserId;

                        try
                        {
                            XmlDocument ret = api.request(new Uri(String.Format("http://me2day.net/api/get_person/{0}.xml", me2dayUserId)), false, null);

                            if (ret != null)
                            {
                                // 미투데이 등록일 설정
                                string registered = ret.SelectSingleNode("//registered").InnerText;
                                parentForm.Controls["tbMe2dayJoinDate"].Text = registered.Substring(0, 10);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }

                        Application.OpenForms["me2dayLoginForm"].Close();
                    }
                }
            }
        }

        

    }
}
