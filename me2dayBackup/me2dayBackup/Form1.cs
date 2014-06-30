using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Xml;
using me2day.api;
using me2day.api.Util;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Linq;
using System.Diagnostics;

namespace me2dayBackup
{
    public partial class Form1 : Form
    {
        #region 변수
        private static Me2API api;
        
        public static DateTime fromdate = DateTime.Now;// Convert.ToDateTime("2012-12-15");
        public static Dictionary<string, string> param = null;

        private static String _currentDirectory = String.Empty;
        private static String _dataFolder = String.Empty;
        private static String _outputFolder = String.Empty;
        private static String _outputFolderHtml = String.Empty;
        private static String _outputFolderProfile = String.Empty;
        private static String _outputFolderPhotos = String.Empty;

        private static DirectoryInfo di = null;
        private static Array _allYear = null;
        private static string _previousMonth = String.Empty;

        private static bool isSavePhoto = false;
        #endregion

        public Form1()
        {
            InitializeComponent();
            
            tbRecentDate.Text = fromdate.ToString("yyyy-MM-dd");
            api = new Me2API();
            api.AppKey = "296a4e7d66d3b4f6f786af9b5517a048";


            #if DEBUG
            btnDebug.Visible = false;
            #endif
        }

        #region 기본 정보 셋팅
        /// <summary>
        /// 기본 정보 세팅
        /// </summary>
        private void InitializeSetting()
        {
            api.UserID = tbMe2dayID.Text;
            fromdate = Convert.ToDateTime(tbRecentDate.Text);

            _currentDirectory = Environment.CurrentDirectory;

            if (File.Exists(_currentDirectory + "\\appkey.txt"))
            {
                api.AppKey = GetFileContents(_currentDirectory + "\\appkey.txt").Trim();
            }

            _dataFolder = _currentDirectory + "\\data\\" + api.UserID;
            if (!Directory.Exists(_dataFolder))
                Directory.CreateDirectory(_dataFolder);

            // output Directory Create
            _outputFolder = _currentDirectory + "\\output\\" + api.UserID;
            _outputFolderHtml = _outputFolder + "\\html";
            _outputFolderProfile = _outputFolder + "\\profile";
            _outputFolderPhotos = _outputFolder + "\\photos";


            if (!Directory.Exists(_outputFolder))
                Directory.CreateDirectory(_outputFolder);
            if (!Directory.Exists(_outputFolderHtml))
                Directory.CreateDirectory(_outputFolderHtml);
            if (!Directory.Exists(_outputFolderProfile))
                Directory.CreateDirectory(_outputFolderProfile);
            if (!Directory.Exists(_outputFolderPhotos))
                Directory.CreateDirectory(_outputFolderPhotos);

            di = new DirectoryInfo(_dataFolder);
        }
        #endregion

        #region Comments Async Process
        async Task<string> ProcessCommentURLAsync(string permalink, HttpClient client)
        {
            string url = String.Format("{0}?post_id={1}&items_per_page=1000", Me2Util.GetAPIUrl(Me2Util.API_METHOD_TYPE.ME2DAY_API_GET_COMMENTS, api.UserID), permalink);
            HttpResponseMessage response = await client.GetAsync(url);
            Stream stream = await response.Content.ReadAsStreamAsync();

            XmlDocument doc = null;
            if (stream != null)
            {
                StreamReader input_reader = new StreamReader(stream, Encoding.UTF8);
                doc = new XmlDocument();
                doc.Load(input_reader);
                input_reader.Close();
            }
            
            string file = String.Format("{0}\\data\\{1}\\{1}_{2}_{3}.xml", _currentDirectory, api.UserID, fromdate.ToString("yyyyMMdd"), permalink);
            doc.PreserveWhitespace = true;
            doc.Save(file);
            //StatusMsg.Text += String.Format("{0}\r\n", url);
            
            return url;
        }

        async Task getCommentsAsync(List<string> permalinks)
        {
            using (HttpClient client = new HttpClient())
            {
                IEnumerable<Task<string>> downloadTasksQuery =
                    from permalink in permalinks select ProcessCommentURLAsync(permalink, client);

                Task<string>[] downloadTasks = downloadTasksQuery.ToArray();
                await Task.WhenAll(downloadTasks);
            }
        }
        #endregion

        #region GET me2day Data
        private async void getMe2dayData()
        {
            try
            {
                param = new Dictionary<string, string>();
                param.Add("from", fromdate.AddDays(1).ToString("yyyyMMdd"));
                param.Add("to", fromdate.ToString("yyyyMMdd"));

                XmlDocument ret = api.request(Me2Util.GetAPIUrl(Me2Util.API_METHOD_TYPE.ME2DAY_API_GET_POSTS, api.UserID), false, param);
                XmlNodeList posts = ret.SelectNodes("//post");

                if (posts.Count > 0)
                {
                    string file = String.Format("{0}\\data\\{1}\\{1}_{2}.xml", _currentDirectory, api.UserID, fromdate.ToString("yyyyMMdd"));
                    ret.PreserveWhitespace = true;
                    ret.Save(file);

                    // 상태 표시
                    StatusMsg.Text += String.Format("Processing - {1}({2}개의 글)\r\n", api.UserID, fromdate.ToString("yyyy'년' MM'월' dd'일'"), posts.Count);

                    #region Comment XML Saved
                    List<string> permalinks = new List<string>();
                    for (int i = 0; i < posts.Count; i++)
                    {
                        permalinks.Add(posts[i].SelectSingleNode("post_id").InnerText);
                    }
                    await getCommentsAsync(permalinks);
                    permalinks = null;

                    #region 동기 방식
                    //for (int i = 0; i < posts.Count; i++)
                    //{
                    //    string permalink = posts[i].SelectSingleNode("post_id").InnerText;

                    //    param2 = new Dictionary<string, string>();
                    //    param2.Add("post_id", permalink);

                    //    string file2 = String.Format("{0}\\data\\{1}\\{1}_{2}_{3}.xml", _currentDirectory, api.UserID, fromdate.AddDays(-1).ToString("yyyyMMdd"), permalink);
                    //    XmlDocument ret2 = api.request(Me2Util.GetAPIUrl(Me2Util.API_METHOD_TYPE.ME2DAY_API_GET_COMMENTS, api.UserID), false, param2);
                    //    ret2.PreserveWhitespace = true;
                    //    ret2.Save(file2);
                    //}
                    #endregion
                    #endregion
                }

                fromdate = fromdate.AddDays(-1);

                // 가입일 이전은 Stop
                if (fromdate.ToString("yyyyMMdd").Equals(Convert.ToDateTime(tbMe2dayJoinDate.Text).AddDays(-1).ToString("yyyyMMdd")))
                {
                    //MakeHTML();
                    btnsEnabled(true);
                    timer1.Stop();

                    StatusMsg.Text += "\r\n\r\n데이터 백업이 완료되었습니다\n";
                }
            }
            catch (Me2Exception ex)
            {
                StatusMsg.Text += String.Format("Error Descriotion : {0}\r\nError Code : {1}\r\nError Message : {2}", ex.Error.Description , ex.Error.Code ,ex.Error.Message);
                btnsEnabled(true);
                timer1.Stop();
            }
            catch (Exception ex)
            {
                StatusMsg.Text += ex.ToString();
                btnsEnabled(true);
                timer1.Stop();
            }
        }
        #endregion
        
        #region 1. 미투데이 로그인
        private void btnMe2dayLogin_Click(object sender, EventArgs e)
        {
            Form f = new me2dayLoginForm(this);
            f.Show();
        }
        #endregion

        #region 2. 백업하기 이벤트
        private void btnMe2dayBackup_Click(object sender, EventArgs e)
        {
            StatusMsg.Text = string.Empty;
            if (String.IsNullOrEmpty(tbMe2dayID.Text))
            {
                MessageBox.Show("먼저 미투데이 로그인 해주세요", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (String.IsNullOrEmpty(tbMe2dayJoinDate.Text))
            {
                MessageBox.Show("미투 생성일을 입력해주세요", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                InitializeSetting();

                btnsEnabled(false);

                timer1.Interval = 4000;
                timer1.Start();
            }
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            getMe2dayData();
        }
        #endregion

        #region 백업하기 - XML 저장 프로세스
        /// <summary>
        /// 템플릿 읽기
        /// </summary>
        /// <returns></returns>
        private string LoadHtmlTemplate()
        {
            return GetFileContents(String.Format("{0}\\template.html", _currentDirectory));
        }

        /// <summary>
        /// 기본 정보 저장 및 치환
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        private string TemplateReplaceBasicInfo(string template)
        {
            if (!string.IsNullOrEmpty(template))
            {
                try
                {
                    // 기본 사용자 정보 불러오기
                    XmlDocument ret = api.request(Me2Util.GetAPIUrl(Me2Util.API_METHOD_TYPE.ME2DAY_API_GET_PERSON, api.UserID), false, null);

                    // 프로필 사진 저장
                    string profileurl = ret.SelectSingleNode("//face").InnerText;
                    string fileName = "profile.png";
                    DownloadRemoteImageFile(profileurl, _outputFolderProfile + "\\" + fileName);

                    // 기본 사진 아이콘 저장
                    DownloadRemoteImageFile("http://static1.me2day.com/images/portable/pa_me2photo.png", _outputFolderProfile + "\\pa_me2photo.png");

                    // 스타일 파일 복사
                    if (!File.Exists(_outputFolderHtml + "\\style.css"))
                        File.Copy(_currentDirectory + "\\style.css", _outputFolderHtml + "\\style.css");


                    // 인덱스 파일 복사
                    if (!File.Exists(_outputFolderHtml + "\\index.html") && !File.Exists(_outputFolder + "\\index.html"))
                        File.Copy(_currentDirectory + "\\index.html", _outputFolder + "\\index.html");

                    string indexHTML = GetFileContents(String.Format("{0}\\index.html", _outputFolder));
                    indexHTML = indexHTML.Replace("[##_me2dayfile_##]", String.Format("me2day_{0}_{1}.html", api.UserID, DateTime.Now.Year));
                    using (StreamWriter sw = File.CreateText(_outputFolder + "\\index.html"))
                    {
                        sw.Write(indexHTML);
                    }

                    // 치환 변수 생성
                    string nickname = ret.SelectSingleNode("//nickname").InnerText;
                    string description = ret.SelectSingleNode("//description").InnerText;

                    template = template.Replace("[##_USERID_##]", api.UserID);
                    template = template.Replace("[##_NICKNAME_##]", nickname);
                    template = template.Replace("[##_DESCRIPTION_##]", description);
                    template = template.Replace("[##_CREATEDATETIME_##]", DateTime.Now.ToString("yyyy-MM-dd tt hh':'mm':'ss"));
                }
                catch (Exception ex)
                {
                    StatusMsg.Text = ex.ToString();
                }
            }
            return template;
        }
        
        /// <summary>
        /// 전체 년도 구하기
        /// </summary>
        /// <returns></returns>
        private Array getArrayTotalYear()
        {
            List<string> tYear = new List<string>();
            string searchFiles = String.Format("{0}_????????.xml", api.UserID);
            foreach (FileInfo f in di.GetFiles(searchFiles)) // 전체 파일 검색
            {
                if (f.Name.IndexOf("_") > 0)
                {
                    string me2dateFile = f.Name.Split('_')[f.Name.Split('_').Length-1];
                    if (!string.IsNullOrEmpty(me2dateFile) && me2dateFile.Length > 4)
                    {
                        string thisYear = me2dateFile.Substring(0, 4);

                        if (!tYear.Contains(thisYear))
                            tYear.Add(thisYear);
                    }
                }
            }
            tYear.Reverse(); // 최신이 더 먼저 나오도록
            return tYear.ToArray(); 
        }
        
        /// <summary>
        /// 좌측 메뉴 치환
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        private string TemplateReplaceLeftMenu(string template)
        {
            if (!string.IsNullOrEmpty(template))
            {
                try
                {
                    string leftMenu = string.Empty;
                    foreach (string strYear in _allYear)
                    {
                        leftMenu += string.Format(@"<a href=""me2day_{0}_{1}.html"">{1}</a>", api.UserID, strYear);
                    }
                    template = template.Replace("[##_LEFTMENU_##]", leftMenu);
                }
                catch (Exception ex)
                {
                    StatusMsg.Text = ex.ToString();
                }
            }
            return template;
        }
        

        /// <summary>
        /// 컨텐츠 영역 HTML 만들기
        /// </summary>
        /// <param name="xmlFile"></param>
        /// <returns></returns>
        private string makeContentsHTML(string xmlFile)
        {
            StringBuilder sb = new StringBuilder();
            string thisMonth = string.Empty;
            string[] monthNames = {"", "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC"};
            try
            {
                if (File.Exists(xmlFile))
                {
                    XmlDocument ret = new XmlDocument();
                    ret.Load(xmlFile);

                    XmlNodeList posts = ret.SelectNodes("//post");
                    thisMonth = getFileNameMonth(xmlFile);

                    #region 달마다 표시 해주기
                    if (string.IsNullOrEmpty(_previousMonth) || !_previousMonth.Equals(thisMonth))
                    {
                        if (!string.IsNullOrEmpty(_previousMonth))
                        {
                            sb.AppendLine("    </div>");
                            sb.AppendLine("</div>");
                        }
                        sb.AppendLine("<div class=\"list_view\">");
                        sb.AppendFormat("    <h3 class=\"sec_month\"><strong>{0}</strong> - {1}</h3>{2}", monthNames[Convert.ToInt32(thisMonth)], thisMonth, Environment.NewLine);
                        sb.AppendLine("    <div class=\"monthly_view\">");
                    }
                    #endregion

                    #region 포스트 부분
                    if (posts.Count > 0)
                    {
                        string postsHtml = string.Empty;
                        string listTemplate = GetFileContents(String.Format("{0}\\template-list.html", _currentDirectory));
                        for (int i = 0; i < posts.Count; i++)
                        {
                            string post_id = posts[i].SelectSingleNode("post_id").InnerText;
                            string plink = posts[i].SelectSingleNode("plink").InnerText;
                            string body = posts[i].SelectSingleNode("body").InnerText;
                            string tagText = posts[i].SelectSingleNode("tagText") != null ? posts[i].SelectSingleNode("tagText").InnerText : string.Empty;
                            string commentsCount = posts[i].SelectSingleNode("commentsCount").InnerText;
                            string metooCount = posts[i].SelectSingleNode("metooCount").InnerText;
                            string commentClosed = posts[i].SelectSingleNode("commentClosed").InnerText;

                            #region 사진 저장
                            string photoUrl = posts[i].SelectSingleNode("media/me2photo/photoUrl") != null ? posts[i].SelectSingleNode("media/me2photo/photoUrl").InnerText : string.Empty;

                            string photoFileHtml = String.Empty;
                            if (!string.IsNullOrEmpty(photoUrl))
                            {
                                string photoFile = string.Empty;
                                if (photoUrl.IndexOf("/") > -1)
                                    photoFile = photoUrl.Split('/')[photoUrl.Split('/').Length - 1];

                                photoFile = photoFile.IndexOf('?') > -1 ? photoFile.Substring(0, photoFile.IndexOf('?')) : photoFile;
                                photoUrl = photoUrl.IndexOf('?') > -1 ? photoUrl.Substring(0, photoUrl.IndexOf('?')) : photoUrl;
                                if (isSavePhoto) DownloadRemoteImageFile(photoUrl, _outputFolderPhotos + "\\" + photoFile); // 사진 저장
                                photoFileHtml = String.Format(@"<a class=""me2photo"" href=""../photos/{0}"" onclick=""imageview('{1}', this);return false;""><img width=""30"" src=""../profile/pa_me2photo.png"" alt=""[me2photo]"" /></a>", photoFile, post_id);
                            }
                            #endregion

                            #region 코멘트 부분
                            string commentsHtml = string.Empty;

                            string commentFile = xmlFile.Replace(".xml", string.Format("_{0}.xml", post_id));
                            if (File.Exists(commentFile))
                            {
                                XmlDocument retCmt = new XmlDocument();
                                retCmt.Load(commentFile);

                                if (retCmt != null)
                                {
                                    XmlNodeList comments = retCmt.SelectNodes("//comment");

                                    if (comments != null && comments.Count > 0)
                                    {
                                        commentsHtml += string.Format("<div id=\"comment-{0}\" class=\"comments hfeed\" style=\"display:none;\">", post_id);
                                        for (int j = 0; j < comments.Count; j++)
                                        {
                                            string commentsBody = comments[j].SelectSingleNode("body").InnerText;
                                            string commentsPubDate = comments[j].SelectSingleNode("pubDate").InnerText;
                                            DateTime commentsPublishdate = Convert.ToDateTime(commentsPubDate);
                                            string commentsAuthorId = comments[j].SelectSingleNode("author/id").InnerText;
                                            string commentsAuthorNick = comments[j].SelectSingleNode("author/nickname").InnerText;

                                            commentsHtml += String.Format(@"<div class=""comment hentry""><span class=""author""><span class=""profile fn"">{0}</span></span><span class=""entry-title entry-content"">{1}</span><div><abbr class=""time published"" title=""{2}"">{3}</abbr></div></div>", commentsAuthorNick, commentsBody, commentsPubDate, commentsPublishdate.ToString("yyyy'년' MM'월' dd'일' tt hh:mm "));
                                        }
                                        commentsHtml += "</div>";
                                    }
                                }
                            }
                            #endregion

                            postsHtml += string.Format(@"
            <li class=""daily_posts"">
              <p class=""post"">
	            {6}
                <label>{0}</label>
	            <span class=""tags"">{1}</span>
	            <span class=""perm"">Metoo ({2}) <a href=""javascript:void(0);"" onclick=""toggle('comment-{5}');"">Comment ({3})</a></span>
              </p>
              <span id=""imageview-{5}""></span>
              {4}
            </li>
                    ", body, tagText, metooCount, commentClosed.ToLower().Equals("true") ? "closed" : commentsCount, commentsHtml, post_id, photoFileHtml);
                        }

                        string xmlDate = (xmlFile.IndexOf("_") > 0) ? xmlFile.Split('_')[xmlFile.Split('_').Length - 1].Replace(".xml", "") : string.Empty;
                        xmlDate = string.Format("{0}-{1}-{2}", xmlDate.Substring(0, 4), xmlDate.Substring(4, 2), xmlDate.Substring(6, 2));
                        DateTime xmlToDate = Convert.ToDateTime(xmlDate);

                        sb.AppendFormat("    <ul id=\"{0}\" class=\"archive_daily\">{1}", xmlDate, Environment.NewLine);
                        sb.Append("     <li class=\"daily_date\">");
                        sb.AppendFormat("    <span class=\"day_h\"><a href=\"http://me2day.net/{0}/{1}\">{2}일</a></span>{3}", api.UserID, String.Format("{0}/{1}/{2}", xmlToDate.ToString("yyyy"), xmlToDate.ToString("MM"), xmlToDate.ToString("dd")), xmlToDate.ToString("dd"), Environment.NewLine);
                        sb.AppendLine("         <ul class=\"daily_summ\">");
                        sb.AppendLine(postsHtml);
                        sb.AppendLine("          </ul>");
                        sb.AppendLine("     </li>");
                        sb.AppendLine("    </ul>");
                    }
                    #endregion

                    posts = null;
                    ret = null;
                    _previousMonth = thisMonth;
                }
            }
            catch (Exception ex)
            {
                StatusMsg.Text = ex.ToString();
            }
            return sb.ToString();
        }

        /// <summary>
        /// 파일에서 Month 가져오기
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private static string getFileNameMonth(string filename)
        {
            string thisMonth = string.Empty;
            if (filename.IndexOf("_") > 0)
            {
                string dataFile = filename.Split('_')[filename.Split('_').Length - 1];
                if (!string.IsNullOrEmpty(dataFile) && dataFile.Length > 4)
                {
                    thisMonth = dataFile.Substring(4, 2);
                }
            }
            return thisMonth;
        }

        /// <summary>
        /// 컨텐츠 영역 치환
        /// </summary>
        /// <param name="template"></param>
        private void TemplateReplaceContents(string template)
        {
            if (!string.IsNullOrEmpty(template))
            {
                try
                {
                    string tmpTemplate = string.Empty;
                    // 컨텐츠 영역은 각 연도별로 저장해야함
                    StatusMsg.Text += Environment.NewLine;
                    foreach (string strYear in _allYear)
                    {
                        tmpTemplate = template.Replace("[##_THISYEAR_##]", strYear);

                        List<string> tMonth = new List<string>(); // 연도별 Month
                        List<string> xmlFileList = new List<string>(); // 연도별 인스탄스 재생성
                        string contenttemplates = string.Empty;
                        string searchFiles = String.Format("{0}_{1}????.xml", api.UserID, strYear);
                        foreach (FileInfo f in di.GetFiles(searchFiles)) // 지정 년도 파일 검색
                        {
                            xmlFileList.Add(f.FullName);
                        }
                        xmlFileList.Reverse();

                        foreach (string xFile in xmlFileList)
                        {
                            contenttemplates += makeContentsHTML(xFile);
                        }
                        xmlFileList.Clear();

                        // 컨텐츠 영역 치환
                        contenttemplates += "</div>"; // 마지막에 닫아주자.-_-;
                        tmpTemplate = tmpTemplate.Replace("[##_CONTENTS_##]", contenttemplates);

                        // HTML 파일로 저장
                        string htmlFileName = String.Format("me2day_{0}_{1}.html", api.UserID, strYear);
                        using (StreamWriter sw = File.CreateText(_outputFolderHtml + "\\" + htmlFileName))
                        {
                            sw.Write(tmpTemplate);
                        }
                        StatusMsg.Text += String.Format("{0}년도 생성",strYear) + Environment.NewLine;
                    }
                }
                catch (Exception ex)
                {
                    StatusMsg.Text = ex.ToString();
                }
            }
        }

        private void MakeHTML()
        {
            _allYear = getArrayTotalYear();
            isSavePhoto = cbSaveImage.Checked;
            
            StatusMsg.Text += "\r\n변환을 시작합니다...\r\n";
            string _template = LoadHtmlTemplate();
            _template = TemplateReplaceBasicInfo(_template);
            _template = TemplateReplaceLeftMenu(_template);
            TemplateReplaceContents(_template);
            StatusMsg.Text += "\r\n\r\n끝!!!!";
            
            if (cbResultView.Checked)
                Process.Start("IEXPLORE.EXE", _outputFolder + "\\index.html");
        }
        
        #endregion
        
        #region Helper Functions
        /// <summary>
        /// 원격 서버의 이미지 파일 다운로드
        /// </summary>
        /// <param name="uri">Uri</param>
        /// <param name="fileName">저장 파일 위치 및 이름</param>
        private bool DownloadRemoteImageFile(string uri, string fileName)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            bool bImage = response.ContentType.StartsWith("image",
                StringComparison.OrdinalIgnoreCase);
            if ((response.StatusCode == HttpStatusCode.OK ||
                response.StatusCode == HttpStatusCode.Moved ||
                response.StatusCode == HttpStatusCode.Redirect) &&
                bImage)
            {
                using (Stream inputStream = response.GetResponseStream())
                using (Stream outputStream = File.OpenWrite(fileName))
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead;
                    do
                    {
                        bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                        outputStream.Write(buffer, 0, bytesRead);
                    } while (bytesRead != 0);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 지정한 경로의 파일을 읽어온다. 읽어올 파일의 내용이 ASCII 라면 UTF-8 로 변경한다.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileContents(string path)
        {
            if (File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(path, System.Text.Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            else
                return string.Empty;
        }
        #endregion

        #region 달력 이벤트
        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            tbMe2dayJoinDate.Text = JoinCalendar.SelectionStart.ToString("yyyy-MM-dd");
            tbMe2dayJoinDate.Focus();
            JoinCalendar.Visible = false;
        }

        private void tbMe2dayJoinDate_Click(object sender, EventArgs e)
        {
            JoinCalendar.Visible = true;
            RecentCalendar.Visible = false;
        }

        private void tbRecentDate_Click(object sender, EventArgs e)
        {
            RecentCalendar.Visible = true;
            JoinCalendar.Visible = false;
        }

        private void RecentCalendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            tbRecentDate.Text = RecentCalendar.SelectionStart.ToString("yyyy-MM-dd");
            tbRecentDate.Focus();
            RecentCalendar.Visible = false;
        }
        #endregion

        private void btnDebug_Click(object sender, EventArgs e)
        {
            tbMe2dayID.Text = "zoro";
            tbMe2dayID.ReadOnly = false;
            tbMe2dayJoinDate.Text = "2013-06-01";
            tbRecentDate.Text = "2013-06-03";
            timer1.Stop();
            btnsEnabled(true);
        }

        private void btnsEnabled(bool flag)
        {
            btnMe2dayLogin.Enabled = flag;
            btnMe2dayBackup.Enabled = flag;
            btnMe2dayExport.Enabled = flag;
        }

        private void btn_Export_Click(object sender, EventArgs e)
        {
            StatusMsg.Text = string.Empty;
            if (String.IsNullOrEmpty(tbMe2dayID.Text))
            {
                MessageBox.Show("먼저 미투데이 로그인 해주세요", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (String.IsNullOrEmpty(tbMe2dayJoinDate.Text))
            {
                MessageBox.Show("미투 생성일을 입력해주세요", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                InitializeSetting();

                btnsEnabled(false);
                MakeHTML();
                btnsEnabled(true);
            }
        }

    }   
}
