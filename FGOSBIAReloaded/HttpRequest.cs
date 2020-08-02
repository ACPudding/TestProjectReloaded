using System.Net;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace FGOSBIAReloaded
{
    class HttpRequest
    {
        public static string PhttpReq(string url, string parameters)
        {

            var hRequest = (HttpWebRequest)WebRequest.Create(url);
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

            var response = (HttpWebResponse)hRequest.GetResponse();

            var myResponseStream = response.GetResponseStream();
            var myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            var retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }
    }
}
