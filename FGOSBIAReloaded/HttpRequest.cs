using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;

namespace FGOSBIAReloaded
{
    internal class HttpRequest
    {
        public static string PhttpReq(string url, string parameters)
        {
            var hRequest = (HttpWebRequest) WebRequest.Create(url);
            hRequest.CookieContainer = new CookieContainer();

            hRequest.Accept = "gzip, identity";
            hRequest.UserAgent = "Dalvik/2.1.0 (Linux; U; Android 6.0.1; MI 6 Build/V417IR)";
            hRequest.ServicePoint.Expect100Continue = false;
            hRequest.KeepAlive = true;
            hRequest.Method = "POST";

            hRequest.ContentType = "application/x-www-form-urlencoded";

            hRequest.ContentLength = parameters.Length;

            var dataParsed = Encoding.UTF8.GetBytes(parameters);
            hRequest.GetRequestStream().Write(dataParsed, 0, dataParsed.Length);


            hRequest.Timeout = 5 * 1000;

            var response = (HttpWebResponse) hRequest.GetResponse();

            var myResponseStream = response.GetResponseStream();
            var myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            var retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }

        public static Stream GetXlsx()
        {
            var httpWebRequest =
                (HttpWebRequest) WebRequest.Create(
                    "https://raw.githubusercontent.com/ACPudding/ACPudding.github.io/master/fileserv/SvtInfoBasicxlsx.xlsx");
            httpWebRequest.Method = "GET";
            try
            {
                var response = httpWebRequest.GetResponse();
                var stream = response.GetResponseStream();
                return stream;
            }
            catch (Exception e)
            {
                MessageBox.Show("网络连接异常,请检查网络连接并重试.\r\n" + e, "网络连接异常", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
    }
}