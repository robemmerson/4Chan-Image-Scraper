using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace _4Chan_Scraper
{
    class Program
    {
        private static ChanEntities db = new ChanEntities();
        private static string chanDir = string.Format("{0}\\4Chan", Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

        static void Main(string[] args)
        {
            if (!Directory.Exists(chanDir)) { Directory.CreateDirectory(chanDir); }

            GetBoard("wg");
        }

        private static void GetBoard(string boardName)
        {
            if (!Directory.Exists(chanDir + "\\" + boardName)) { Directory.CreateDirectory(chanDir + "\\" + boardName); }

            var req = WebRequest.Create("https://a.4cdn.org/" + boardName + "/threads.json");
            var res = req.GetResponse();
            var resStream = res.GetResponseStream();
            using (var sr = new StreamReader(resStream))
            {
                var jsonString = sr.ReadToEnd();
                var allPages = JsonConvert.DeserializeObject<Board4Page[]>(jsonString);

                foreach(var page in allPages)
                {
                    Console.WriteLine("Processing Page: " + page.page);
                    Console.WriteLine();

                    foreach(var thread in page.threads)
                    {
                        try
                        {
                            GetThread(boardName, thread.no);
                        }
                        catch (Exception ex) {}
                    }
                }
            }
            res.Close();
        }

        private static void GetThread(string boardName, string threadNumber)
        {
            var req = WebRequest.Create(string.Format("https://a.4cdn.org/{0}/thread/{1}.json", boardName, threadNumber));
            var res = req.GetResponse();
            var resStream = res.GetResponseStream();
            using (var sr = new StreamReader(resStream))
            {
                var jsonString = sr.ReadToEnd();
                var allPosts = JsonConvert.DeserializeObject<Thread4>(jsonString);

                foreach (var post in allPosts.posts)
                {
                    Console.WriteLine("Processing Post: " + post.no);

                    if (db.DbTheads.Any(x => x.ThreadNumber.Equals(post.no))) { continue; }

                    if (!string.IsNullOrEmpty(post.filename))
                    {
                        var client = new WebClient();
                        client.DownloadFile(string.Format("http://i.4cdn.org/{0}/{1}{2}", boardName, post.tim, post.ext), string.Format("{0}\\{1}\\{2}{3}", chanDir, boardName, post.tim, post.ext));
                        db.DbTheads.Add(new DbThread() { ThreadNumber = post.no, DateAdded = DateTime.Now });
                        db.SaveChanges();
                    }
                }

                Console.WriteLine();
                Console.WriteLine();
            }
            res.Close();
        }
    }
}
