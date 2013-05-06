using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace NFusion
{
    class Program
    {
        protected static CookieWebClient wc = new CookieWebClient();

        static void Main(string[] args)
        {
            wc.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.97 Safari/537.11";
            //wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            //wc.Headers[HttpRequestHeader.Accept] = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            //wc.Headers[HttpRequestHeader.AcceptEncoding] = "gzip,deflate,sdch";
            //wc.Headers[HttpRequestHeader.AcceptLanguage] = "en-GB,en-US;q=0.8,en;q=0.6";
            //wc.Headers[HttpRequestHeader.AcceptCharset] = "ISO-8859-1,utf-8;q=0.7,*;q=0.3";

            Stream read = null;
            string url = "http://data.bank.hexun.com/lccp/AllLccp.aspx";
            try
            {
                read = wc.OpenRead(url);
            }
            catch (ArgumentException)
            {
                read = wc.OpenRead(HttpUtility.HtmlEncode(url));
            }

            

            //StreamReader sr = new StreamReader(read, System.Text.Encoding.Default);
            //string resStr = sr.ReadToEnd();
            //Console.WriteLine(resStr);


            HtmlDocument hdoc = new HtmlDocument();
            HtmlNode.ElementsFlags.Remove("option");
            HtmlNode.ElementsFlags.Remove("select");

            hdoc.Load(read, true);

            foreach (var script in hdoc.DocumentNode.Descendants("script").ToArray())
                script.Remove();
            foreach (var style in hdoc.DocumentNode.Descendants("style").ToArray())
                style.Remove();

            string innerText = hdoc.DocumentNode.InnerText;

           
            //foreach (HtmlNode form in hdoc.DocumentNode.SelectNodes("//div[3]/table/tbody/tr[td]"))
            //{
            //    HtmlAttribute att = form.Attributes["action"];
            //    Console.WriteLine(att.Value);
            //}


            ////Remove CSS styles, if any found
            //resStr = Regex.Replace(resStr, "<style(.| )*?>*</style>", "");
            ////Remove script blocks
            //resStr = Regex.Replace(resStr, "<script(.| )*?>*</script>", "");
            //// Remove all images
            //resStr = Regex.Replace(resStr, "<img(.| )*?/>", "");
            //// Remove all HTML tags, leaving on the text inside.
            //resStr = Regex.Replace(resStr, "<(.| )*?>", "");
            //// Remove all extra spaces, tabs and repeated line-breaks
            //resStr = Regex.Replace(resStr, "(x09)?", "");
            //resStr = Regex.Replace(resStr, "(x20){2,}", " ");
            //resStr = Regex.Replace(resStr, "(x0Dx0A)+", " ");




        }
    }

    public class CookieWebClient : WebClient
    {

        public CookieContainer m_container = new CookieContainer();
        public WebProxy proxy = null;

        protected override WebRequest GetWebRequest(Uri address)
        {
            try
            {
                ServicePointManager.DefaultConnectionLimit = 1000000;
                WebRequest request = base.GetWebRequest(address);
                request.Proxy = proxy;

                HttpWebRequest webRequest = request as HttpWebRequest;
                webRequest.Pipelined = true;
                webRequest.KeepAlive = true;
                if (webRequest != null)
                {
                    webRequest.CookieContainer = m_container;
                }

                return request;
            }
            catch
            {
                return null;
            }
        }
    }
}
