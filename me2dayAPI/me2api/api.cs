using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Net;
using System.IO;
using me2day.api.Util;
using System.Web;
using me2day.api.Model;
using System.Web.Configuration;

namespace me2day.api
{
    public class Me2API
    {
        /// <summary>
        /// 미투데이 사용자 이름
        /// </summary>
        public String UserID { get; set; }
        /// <summary>
        /// 미투데이 사용자 API Key
        /// </summary>
        public String ApiKey { get; set; }

        /// <summary>
        /// 미투데이 APP Key
        /// </summary>
        public String AppKey { get; set; }

        public enum FRIENDS_SCOPE
        {
            ALL,
            CLOSE,
            FAMILY,
            MYTAG
        }

        protected string GetParamString(IDictionary<string, string> param)
        {
            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<string, string> item in param)
            {
                if (sb.Length != 0)
                    sb.Append("&");

                sb.Append(item.Key);
                sb.Append("=");
                sb.Append(HttpUtility.UrlEncode(item.Value,Encoding.UTF8));
            }

            return sb.ToString();

        }

        public XmlDocument request(Uri url, bool authRequest, IDictionary<string,string> param)
        {
            try
            {
                XmlDocument doc = null;
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);

                if (authRequest)
                {
                    req.Credentials = new NetworkCredential(UserID, Me2Util.GetAuthPassword(ApiKey));
                }

                if (!String.IsNullOrEmpty(AppKey))
                {
                    req.Headers.Add("me2_application_key", AppKey);
                }

                req.ContentType = "application/x-www-form-urlencoded";
                req.KeepAlive = false;

                if (param != null)
                {
                    req.Method = "POST";
                    Stream output = req.GetRequestStream();
                    StreamWriter output_writer = new StreamWriter(output);
                    output_writer.Write(GetParamString(param));
                    output_writer.Flush();
                    output_writer.Close();
                    output.Close();
                }

                HttpWebResponse rep = (HttpWebResponse)req.GetResponse();
                Stream input = rep.GetResponseStream();
                StreamReader input_reader = new StreamReader(input, Encoding.UTF8);

                doc = new XmlDocument();
                doc.Load(input_reader);

                input_reader.Close();
                input.Close();

                return doc;


            }
            catch (WebException we)
            {
                if (((HttpWebResponse)we.Response).StatusCode == HttpStatusCode.InternalServerError)
                {
                    Stream input = we.Response.GetResponseStream();
                    StreamReader input_reader = new StreamReader(input, Encoding.UTF8);

                    XmlDocument doc = new XmlDocument();
                    doc.Load(input_reader);

                    input_reader.Close();
                    input.Close();

                    throw new Me2Exception(Me2Util.ParseError(doc));
                }
                else
                    throw we;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 인증 테스트용
        /// </summary>
        /// <returns></returns>
        public bool noop()
        {
            Uri url = Me2Util.GetAPIUrl(Me2Util.API_METHOD_TYPE.ME2DAY_API_NOOP);
            try
            {
                XmlDocument doc = request(url, true, null);

                return Me2Util.ParseError(doc).Code == 0;
            }
            catch (WebException e)
            {
                if (((HttpWebResponse)e.Response).StatusCode == HttpStatusCode.Unauthorized)
                {
                    return false;
                }

                throw e;
            }
                
        }

        /// <summary>
        /// 쉬운 인증
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void GetAuth(string applicationKey)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("akey", applicationKey);
            Uri url = Me2Util.GetAPIUrl(Me2Util.API_METHOD_TYPE.ME2DAY_API_GET_AUTH_URL);
            try
            {
                XmlDocument doc = request(url, false, param);
                if (doc != null && doc.SelectSingleNode("//url") != null)
                {
                    System.Web.HttpContext.Current.Response.Redirect(doc.SelectSingleNode("//url").InnerText);
                }
            }
            catch (WebException e)
            {
                if (((HttpWebResponse)e.Response).StatusCode == HttpStatusCode.Unauthorized)
                {
                }

                throw e;
            }
        }

        /// <summary>
        /// 데스크톱 기반 쉬운인증
        /// </summary>
        /// <param name="applicationKey"></param>
        public XmlDocument GetAuthDesktopUrl(string applicationKey)
        {
            string auth_url = String.Empty;
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("akey", applicationKey);
            Uri url = Me2Util.GetAPIUrl(Me2Util.API_METHOD_TYPE.ME2DAY_API_GET_AUTH_URL);
            return request(url, false, param);
        }


        /// <summary>
        /// 토큰으로 세션키를 받아오는 메소드
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public XmlDocument getSessionKey(string token)
        {
            return request(new Uri("http://me2day.net/api/get_full_auth_token.xml?token=" + token), false, null);
        }


        /// <summary>
        /// 지정한 사용자의 친구들 목록을 가져 옵니다. ( mytag: 인증 필요. 본인의 것만 가능 )
        /// </summary>
        /// <param name="id"></param>
        /// <param name="scope"></param>
        /// <param name="mytag"></param>
        /// <returns></returns>
        public List<Person> GetFriends(string id,FRIENDS_SCOPE scope, params object[] mytag)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            bool bMustAuth = false;

            if (scope == FRIENDS_SCOPE.ALL)
                param = null;
            else if (scope != FRIENDS_SCOPE.MYTAG)
            {
                param.Add("scope", Enum.GetName(typeof(FRIENDS_SCOPE), scope).ToLower());
            }
            else
            {
                param.Add("scope", "mytag[" + mytag[0].ToString() + "]");
                bMustAuth = true;
            }

            XmlDocument ret = request(Me2Util.GetAPIUrl(Me2Util.API_METHOD_TYPE.ME2DAY_API_GET_FRIENDS, id), bMustAuth, param);


            List<Person> result = new List<Person>();

            XmlNodeList friends = ret.SelectNodes("//person");
            foreach (XmlNode personNode in friends)
            {
                Person person = Me2Util.FromXml<Person>(personNode);
                result.Add(person);
            }

            return result;

        }

        /// <summary>
        /// 해당 사용자의 최근글을 가져옵니다.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Post> getLatest(string id)
        {
            XmlDocument ret = request(Me2Util.GetAPIUrl(Me2Util.API_METHOD_TYPE.ME2DAY_API_GET_LATESTS, id), false, null);

            List<Post> result = new List<Post>();

            XmlNodeList posts = ret.SelectNodes("//post");
            foreach (XmlNode postNode in posts)
            {   
                Post post = Me2Util.FromXml<Post>(postNode);

                if (postNode.SelectNodes("tags").Count > 0)
                {
                    foreach (XmlNode tagNode in postNode.SelectNodes("tags//tag"))
                    {
                        Tag tag = Me2Util.FromXml<Tag>(tagNode);
                        post.Tags.Add(tag);
                    }
                }

                Person author = Me2Util.FromXml<Person>(postNode.SelectSingleNode("author"));
                post.Author = author;
                result.Add(post);
            }

            return result;

        }

        /// <summary>
        /// 해당 사용자의 글을 가져옵니다.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Post> getPosts(string id, Dictionary<string, string> param)
        {
            XmlDocument ret = request(Me2Util.GetAPIUrl(Me2Util.API_METHOD_TYPE.ME2DAY_API_GET_POSTS, id), false, param);

            List<Post> result = new List<Post>();

            XmlNodeList posts = ret.SelectNodes("//post");
            foreach (XmlNode postNode in posts)
            {
                Post post = Me2Util.FromXml<Post>(postNode);

                if (postNode.SelectNodes("tags").Count > 0)
                {
                    foreach (XmlNode tagNode in postNode.SelectNodes("tags//tag"))
                    {
                        Tag tag = Me2Util.FromXml<Tag>(tagNode);
                        post.Tags.Add(tag);
                    }
                }

                Person author = Me2Util.FromXml<Person>(postNode.SelectSingleNode("author"));
                post.Author = author;
                result.Add(post);
            }

            return result;

        }

        /// <summary>
        /// 지정된 글의 퍼머링크에 달려 있는 댓글을 가져 옵니다.
        /// </summary>
        /// <param name="permalink"></param>
        /// <returns></returns>
        public List<Comment> getComments(string permalink)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("post_id", permalink);

            XmlDocument ret = request(Me2Util.GetAPIUrl(Me2Util.API_METHOD_TYPE.ME2DAY_API_GET_COMMENTS), false, param);
            List<Comment> result = new List<Comment>();

            XmlNodeList comments = ret.SelectNodes("//comment");

            foreach (XmlNode commentNode in comments)
            {
                Comment comment = Me2Util.FromXml<Comment>(commentNode);
                Person author = Me2Util.FromXml<Person>(commentNode.SelectSingleNode("author"));
                comment.Author = author;
                result.Add(comment);
            }

            return result;
        }

        /// <summary>
        /// 글을 작성합니다.
        /// </summary>
        /// <param name="body">본문</param>
        /// <param name="tags">태그</param>
        /// <param name="icon">아이콘 번호(1~12)</param>
        /// <returns></returns>
        public Post createPost(string id,string body, List<string> tags, int icon)
        {

            if (icon > 12 || icon <= 0)
                throw new ArgumentOutOfRangeException("아이콘 번호는 1-12 사이여야 합니다.");

            if (String.IsNullOrEmpty(body))
                throw new ArgumentNullException("본문을 입력하셔야 합니다."); 

            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("post[body]", body);
            param.Add("post[tags]", String.Join(" ", tags.ToArray()));
            param.Add("post[icon_index]", icon.ToString());

            XmlDocument ret = request(Me2Util.GetAPIUrl(Me2Util.API_METHOD_TYPE.ME2DAY_API_CREATE_POST, id), true, param);
            return Me2Util.FromXml<Post>(ret.SelectSingleNode("//post"));

        }

        /// <summary>
        /// 댓글을 기록합니다. 성공/실패 여부만을 리턴합니다.
        /// </summary>
        /// <param name="post_id"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public bool createComment(string post_id, string comment)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("body", comment);
            param.Add("post_id", post_id);

            Me2Error result = Me2Util.ParseError(request(Me2Util.GetAPIUrl(Me2Util.API_METHOD_TYPE.ME2DAY_API_CREATE_COMMENT), true, param));
            return result.Code == 0;

        }

        /// <summary>
        /// 댓글을 삭제합니다. 성공/실패 여부만을 리턴합니다.
        /// </summary>
        /// <param name="comment_id"></param>
        /// <returns></returns>
        public bool deleteComment(string comment_id)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("comment_id", comment_id);

            Me2Error result = Me2Util.ParseError(request(Me2Util.GetAPIUrl(Me2Util.API_METHOD_TYPE.ME2DAY_API_DELETE_COMMENT), true, param));
            return result.Code == 0;
        }



    }
}
