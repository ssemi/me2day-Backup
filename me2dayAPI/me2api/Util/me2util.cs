using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Xml;
using System.Reflection;

namespace me2day.api.Util
{
    public class Me2Util
    {
        /// <summary>
        /// API METHOD 종류
        /// </summary>
        public enum API_METHOD_TYPE
        {
            ME2DAY_API_NOOP,
            ME2DAY_API_GET_FRIENDS,
            ME2DAY_API_GET_LATESTS,
            ME2DAY_API_GET_COMMENTS,
            ME2DAY_API_CREATE_POST,
            ME2DAY_API_CREATE_COMMENT,
            ME2DAY_API_DELETE_COMMENT,
            ME2DAY_API_GET_AUTH_URL,
            ME2DAY_API_GET_POSTS,
            ME2DAY_API_GET_PERSON

        }

        private static IDictionary<API_METHOD_TYPE, string> m_url_dict;
        private static Random rand;
        private static string API_BASE;
        private static char[] NONCE_ARRAY;

        static Me2Util()
        {
            rand = new Random(DateTime.Now.Millisecond);
            API_BASE = "http://me2day.net/api/";
            NONCE_ARRAY = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };

            m_url_dict = new Dictionary<API_METHOD_TYPE, string>();

            m_url_dict.Add(API_METHOD_TYPE.ME2DAY_API_NOOP, "noop");
            m_url_dict.Add(API_METHOD_TYPE.ME2DAY_API_GET_FRIENDS, "get_friends");
            m_url_dict.Add(API_METHOD_TYPE.ME2DAY_API_GET_LATESTS, "get_latests");
            m_url_dict.Add(API_METHOD_TYPE.ME2DAY_API_GET_POSTS, "get_posts");
            m_url_dict.Add(API_METHOD_TYPE.ME2DAY_API_GET_COMMENTS, "get_comments");
            m_url_dict.Add(API_METHOD_TYPE.ME2DAY_API_CREATE_POST, "create_post");
            m_url_dict.Add(API_METHOD_TYPE.ME2DAY_API_CREATE_COMMENT, "create_comment");
            m_url_dict.Add(API_METHOD_TYPE.ME2DAY_API_DELETE_COMMENT, "delete_comment");
            m_url_dict.Add(API_METHOD_TYPE.ME2DAY_API_GET_AUTH_URL, "get_auth_url");
            m_url_dict.Add(API_METHOD_TYPE.ME2DAY_API_GET_PERSON, "get_person");
        }
      
       
        /// <summary>
        /// 8자리 임이의 문자열을 만들어 냅니다.
        /// </summary>
        /// <returns></returns>
        private static String GetNonce()
        {
            StringBuilder sb = new StringBuilder();
            
            for (int i = 0; i < 8; i++)
            {
                sb.Append(NONCE_ARRAY[rand.Next(0,NONCE_ARRAY.Length)]); 
            }

            return sb.ToString();
        }

        /// <summary>
        /// ME2 API 인증 암호를 생성 합니다.
        /// </summary>
        /// <param name="key">사용자 API KEY</param>
        /// <returns></returns>
        public static String GetAuthPassword(string key)
        {
            StringBuilder sb = new StringBuilder();
            string nonce = GetNonce();
            MD5 digest = MD5.Create();

            sb.Append(nonce);
            byte[] hash = digest.ComputeHash(System.Text.Encoding.Default.GetBytes(String.Format("{0}{1}", nonce, key)));
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// me2DAY API 주소를 구해 옵니다
        /// </summary>
        /// <param name="type"></param>
        /// <param name="param">사용자 id 등의 url 파라메터</param>
        /// <returns></returns>
        public static Uri GetAPIUrl(API_METHOD_TYPE type, params object[] param)
        {
            string url_base = String.Format("{0}{1}", API_BASE, m_url_dict[type]);

            if (param.Length > 0)
            {
                foreach (object o in param)
                {
                    url_base += "/" + o.ToString();
                }
            }
        
            return new Uri(url_base);
        }

        /// <summary>
        /// me2 Error XML문서를 분석 합니다
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static Me2Error ParseError(XmlDocument xml)
        {
            XmlNode elem_error = xml.SelectSingleNode("//error");
            if (elem_error != null)
            {
                Me2Error error = new Me2Error();
                error.Code = Convert.ToInt32(elem_error.SelectSingleNode("code").InnerText);
                error.Message = elem_error.SelectSingleNode("message").InnerText;
                error.Description = elem_error.SelectSingleNode("description").InnerText;

                return error;
            }
            else
                return null;

        }

        /// <summary>
        /// XML 형태로 떨어지는 문서에서 모델 객체를 생성 합니다.
        /// </summary>
        /// <typeparam name="T">can be from Comment,Person,Post,Tag</typeparam>
        /// <param name="root"></param>
        /// <returns></returns>
        public static T FromXml<T>(XmlNode root) // where T : new()
        {
            T model = Activator.CreateInstance<T>();
            //T model = new T();
            Type type = model.GetType();

            foreach (XmlNode elem in root.ChildNodes)
            {
                PropertyInfo pi = type.GetProperty(elem.Name);
                if (pi != null)
                {
                    object setValue = elem.InnerText;
                    if (pi.PropertyType == typeof(Int32))
                    {
                        setValue = Convert.ToInt32(elem.InnerText);

                    }
                    else if (pi.PropertyType == typeof(DateTime))
                    {
                        setValue = Convert.ToDateTime(elem.InnerText);
                    }

                    //pi.GetSetMethod().Invoke(model, new object[] { setValue });
                    pi.SetValue(model, setValue, null);
                }
            }

            return model;

        }

    }
}
