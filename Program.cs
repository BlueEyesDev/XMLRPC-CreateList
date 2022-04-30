using System;
using System.Threading.Tasks;
using System.Net;
using System.IO;
namespace XMLRPC_CreateList
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Shodan shodan = new Shodan(API:"xxxxxxxxx");
        
            Parallel.ForEach(shodan.GetList(Country:"FR"), Site =>
            {
                try {
                    if (Webrequest(Site) == "XML-RPC server accepts POST requests only.") {
                        File.AppendAllLines("List.txt", new string[] { Site });
                    }
                } catch (Exception e) {
                    Console.WriteLine(e.Message);
                }
              
            });
        }
        static string Webrequest(string url)
        {
            WebResponse httpResponse = null;
            try {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Method = "GET";
                httpWebRequest.Headers.Add("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.127 Safari/537.36");
                httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    return streamReader.ReadToEnd();
            } catch (WebException ex) {
                if (ex.Response != null)
                    httpResponse = ex.Response;
            }
            if (httpResponse != null)
                return new StreamReader(httpResponse.GetResponseStream()).ReadToEnd();
            else
                return String.Empty;
        }
    }
}
