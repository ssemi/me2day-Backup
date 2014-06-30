﻿using System;
using System.Collections.Generic;
using System.Text;
using me2day.api;
using me2day.api.Util;
using me2day.api.Model;

namespace expo
{
    class expo
    {
        private static Me2API api;

        static void usage()
        {
            Console.WriteLine("===================");
            Console.WriteLine("1. 최근글 가져오기");
            Console.WriteLine("2. 글 작성");
            Console.WriteLine("3. 친구 보기");
            Console.WriteLine("4. 그만 하기");
            Console.WriteLine("===================");
            Console.Write(">> ");
        }

        static void getLatests()
        {
            List<Post> posts = api.getLatest(api.UserID);

            foreach (Post post in posts)
            {
                Console.WriteLine(post.body);
                Console.WriteLine(String.Format("{0}, {1}", post.pubDate, post.commentsCount ));
            }
        }

        static void writePost()
        {
            Console.WriteLine("본문을 입력하세요");
            string body = Console.ReadLine().Trim();
            Console.WriteLine("태그를 입력하세요");
            string tag = Console.ReadLine().Trim();
            Console.WriteLine("아이콘 번호를 입력하세요");
            Int32 icon_index = Convert.ToInt32(Console.ReadLine().Trim());
            
            Post post = api.createPost(api.UserID, body, new List<String>(tag.Split(new char[]{' '})), icon_index);
            Console.WriteLine("pubDate:: {0}", post.pubDate);

        }

        static void getFriends()
        {
            List<Person> friends = api.GetFriends(api.UserID, Me2API.FRIENDS_SCOPE.ALL);

            foreach (Person person in friends)
            {
                Console.WriteLine("{0}, {1}", person.id, person.nickname);
            }

        }

        static void Main(string[] args)
        {

            try
            {

                Console.WriteLine("Me2DAY 아이디를 입력하세요");
                string id = Console.ReadLine().Trim();

                Console.WriteLine("Me2DAY API 키를 입력하세요");
                string api_key = Console.ReadLine().Trim();

                api = new Me2API();
                api.UserID = id;
                api.AppKey = "828e442ceea1ecdbc7db09d644ae69e9";
                api.ApiKey = api_key;

                while (true)
                {

                    usage();
                    int cmd = Convert.ToInt32(Console.ReadLine().Trim());

                    if (cmd == 1)
                        getLatests();
                    else if (cmd == 2)
                        writePost();
                    else if (cmd == 3)
                        getFriends();
                    else
                        break;
                }


            }
            catch (Me2Exception e)
            {
                Console.WriteLine("에러가 발생했습니다:: " + e.Error.Code);
                Console.WriteLine(e.Error.Message);
                Console.WriteLine(e.Error.Description);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
