using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Channels
{
    class Helper
    {
        public static List<Channel> channels;
        public static List<Channel> Favorites;
        public static List<string> categories;

        public static string makePlayableLink(string link)
        {
            string playable = link;
            string temp = link;
            playable = "file:///" + temp.Replace("\\", "/");
            return playable;

        }

        public static bool addToFav(Button btn)
        {
            
            Channel temp = new Channel();
            int index = -1;
            index= Helper.channels.FindIndex(channel => (channel.name == btn.Text));
            if (index != -1)
            {
                temp.name = channels[index].name;
                temp.url = channels[index].url;
                Favorites.Add(temp);
                File.AppendAllText(Environment.CurrentDirectory + "//Favorites.ini", Environment.NewLine + temp.name + " = " + temp.url);  
                return true;
            }
            else
                return false;
            

        }

        

        public static void verifyAceStreamIsRunning()
        {
            var runningProcessByName = Process.GetProcessesByName("ace_engine");
            if (runningProcessByName.Length == 0)
            {
                string path =  Environment.CurrentDirectory + "\\Ace Stream\\ace_engine.exe";
                Process.Start(path);
            }

        }

        public static  string getAfteLogin()
        {
            string html;

            var request = WebRequest.Create("http://torrent-tv.ru/auth.php") as HttpWebRequest;
            var container = new CookieContainer();
            request.CookieContainer = container;
            string postData = "email=" + "alexandercarabet@gmail.com" + "&password=" + "car307072447"+ "&enter=;
            byte[] data = Encoding.ASCII.GetBytes(postData);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            var response = (HttpWebResponse)request.GetResponse();

            request = WebRequest.Create("http://torrent-tv.ru/channels.php") as HttpWebRequest;
            request.CookieContainer = container;
            request.Method = "GET";
            response = (HttpWebResponse)request.GetResponse();
            string authorizedGetString;
            using (var stream = response.GetResponseStream())
            {
                using (var streamReader = new StreamReader(stream))
                {
                    authorizedGetString = streamReader.ReadToEnd();
                }
            }
            return authorizedGetString;
                //CookieContainer cc = new CookieContainer();
                //HttpWebResponse response;
                //HttpWebRequest http = WebRequest.Create("http://torrent-tv.ru/auth.php") as HttpWebRequest;
                //http.KeepAlive = true;
                //http.Method = "POST";
                //http.ContentType = "application/x-www-form-urlencoded";

            //http.CookieContainer = cc;

            //string postData = "email=" + "alexandercarabet@gmail.com" + "&password=" + "car307072447";
            //byte[] dataBytes = UTF8Encoding.UTF8.GetBytes(postData);
            //http.ContentLength = dataBytes.Length;
            //using (Stream postStream = http.GetRequestStream())
            //{
            //    postStream.Write(dataBytes, 0, dataBytes.Length);
            //}

            //// Probably want to inspect the http.Headers here first
            //http = WebRequest.Create("http://torrent-tv.ru/channels.php") as HttpWebRequest;
            //http.Method = "GET";
            //HttpWebResponse httpResponse = http.GetResponse() as HttpWebResponse;
            //Stream temp=httpResponse.GetResponseStream();
            ////http.GetRequestStream();
            //http.CookieContainer = cc;



            //HttpWebResponse httpResponse2 = http.GetResponse() as HttpWebResponse;
            //using (var textReader = new StreamReader(temp, Encoding.UTF8, true))
            //    return html = textReader.ReadToEnd();
            ////html = httpResponse2.ToString();
            //using (var client = new WebClient())
            //{
            //    var values = new NameValueCollection
            //{
            //    { "email", "alexandercarabet@gmail.com" },
            //    { "password", "car307072447" }
            //    };
            //    client.UploadValues("http://torrent-tv.ru/auth.php", values);
            //    using (var stream = client.OpenRead("http://torrent-tv.ru/channels.php"))
            //    using (var textReader = new StreamReader(stream, Encoding.UTF8, true))
            //    {
            //        // Authenticate
            //        html=textReader.ReadToEnd();
            //    }

            //        // Download desired page
            //        return html;

            //}
        }

        public static void getChannelList()
        {
            string htmlCode= getAfteLogin();
            categories = new List<string>();
            //const Int32 BufferSize = 128;
            //using (var fileStream = File.OpenRead(Environment.CurrentDirectory+ "\\TTV.txt"))
            //using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            //{
            //    htmlCode = streamReader.ReadToEnd();

            

            //}
            using (var client = new WebClient())
            using (var stream = client.OpenRead("http://torrent-tv.ru/channels.php"))
            using (var textReader = new StreamReader(stream, Encoding.UTF8, true))
            {
                string name;
                string category="";
                Channel temp = new Channel();
                channels.Clear();
                string url;
                //htmlCode = textReader.ReadToEnd();
                htmlCode = htmlCode.Substring(htmlCode.IndexOf("channels-submenu"));
                htmlCode = htmlCode.Substring(0,htmlCode.IndexOf("films.php"));
                string[] result = htmlCode.Split(new[] { '\r', '\n' });
                
                for(int i=0;i<result.Length;i++)
                {
                    if(result[i].Contains("torrent-online.php"))
                    {
                        url = result[i].Substring(result[i].IndexOf("on=")+3);
                        url = url.Substring(0, url.IndexOf('\"'));
                        name= result[i].Substring(result[i].IndexOf(">")+1);
                        name = name.Substring(0,name.IndexOf("<"));
                        temp.name = name;
                        temp.url = "http://127.0.0.1:6878/ace/getstream?url=http%3A%2F%2Fcontent.asplaylist.net%2F" + url + ".acelive";
                        temp.category += category;
                        channels.Add(temp);
                        temp = new Channel();
                    }
                    if(result[i].Contains("<ul>")&& result[i-1].Contains("href="))
                    {
                        category = result[i - 1].Substring(result[i - 1].IndexOf(">") + 1);
                        category = category.Substring(0, category.IndexOf("<"));
                        categories.Add(category);

                    }

                }


            }

        }
        public static void loadChannels(string path)
        {
           
            channels = new List<Channel>();
            Favorites = new List<Channel>();

            Channel temp;
            const Int32 BufferSize = 128;
            using (var fileStream = File.OpenRead(path))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line;

                while ((line = streamReader.ReadLine()) != null)
                {
                    try
                    {
                        temp = new Channel();
                        temp.name = line.Substring(0, line.IndexOf("=")).TrimEnd();
                        temp.name = temp.name.TrimStart();
                        temp.url = line.Substring(line.IndexOf("=") + 1);
                        temp.url = temp.url.TrimStart();
                        temp.url = temp.url.TrimEnd();
                        channels.Add(temp);
                    }
                    catch { }
                }


            }

            using (var fileStream = File.OpenRead(path.Replace("Channels","Favorites")))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line;

                while ((line = streamReader.ReadLine()) != null)
                {
                    try
                    {
                        temp = new Channel();
                        temp.name = line.Substring(0, line.IndexOf("=")).TrimEnd();
                        temp.name = temp.name.TrimStart();
                        temp.url = line.Substring(line.IndexOf("=") + 1);
                        temp.url = temp.url.TrimStart();
                        temp.url = temp.url.TrimEnd();
                        Favorites.Add(temp);
                    }
                    catch { }
                }


            }
            getChannelList();

        }

        internal static bool remFromFav(Button pressedBtn)
        {
            Channel temp = new Channel();
            string text = "";
            int i;
            int index = -1;
            index = Helper.Favorites.FindIndex(channel => (channel.name == pressedBtn.Text));
            if (index != -1)
            {
                Favorites.RemoveAt(index);
                //temp.name = channels[index].name;
                //temp.url = channels[index].url;
                //Favorites.Add(temp);
                //File.AppendAllText(Environment.CurrentDirectory + "//Favorites.ini", Environment.NewLine + temp.name + " = " + temp.url);
                for(i=0;i<Favorites.Count-1;i++)
                {
                    
                    text += Favorites[i].name + " = " + Favorites[i].url+Environment.NewLine;
                    
                }
                try
                {
                    text += Favorites[i].name + " = " + Favorites[i].url;
                }
                catch { }
                File.WriteAllText(Environment.CurrentDirectory + "//Favorites.ini", text);

                return true;
            }
            else
                return false;
        }
    }
}
