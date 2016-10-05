using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HTTPReqHelper
{
    public class ResponseInfo
    {
        public string StatusCode { get; set; }

        public string StatusDesc { get; set; }
        public string Content { get; set; }

        public string NewLocation { get; set; }
    }

    public class Websites
    {
        public string URL { get; set; }

    }
    public class WebRequestGetExample
    {
        public static void Test()
        {
            // Create a request for the URL. 

            List<string> webList = new List<string>();

            webList.Add("http://www.google.com");
            webList.Add("http://www.yahoo.com/123");
            webList.Add("http://www.lowyat.net");
            webList.Add("http://www.youtubexxxmunyau.com/123");
            webList.Add("http://www.faxcebook.com");

            foreach (var url in webList)
            {
                ResponseInfo resp = Browse(url);
                Console.WriteLine($"{url} : {resp.StatusDesc}");
            }

            Console.ReadKey();
        }

     
        public static ResponseInfo Browse(string url)
        {

            string body = "";
            int statusCd = 0;
            string statusDesc = "";
            string location = "";

            ResponseInfo httpResp = new ResponseInfo();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "HEAD";
            request.ContentLength = 0;
            request.ContentType = "application/x-www-form-urlencoded";
            request.AllowAutoRedirect = false;

            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.
            try
            {
                WebResponse response = request.GetResponse();
                // Display the status.

                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                body = responseFromServer;
                statusCd = (int)((HttpWebResponse)response).StatusCode;
                statusDesc = (((HttpWebResponse)response).StatusDescription).ToString();

                if((statusCd>300)&&(statusCd<310))
                {
                    location = response.Headers["Location"];
                }

                // Clean up the streams and the response.
                reader.Close();
                response.Close();

            }
            catch (WebException we)
            {
                statusCd = -1;
                statusDesc = "Request Timeout";
                HttpWebResponse resp = (HttpWebResponse)we.Response;
                if (resp != null)
                {
                    HttpStatusCode code = resp.StatusCode;
                    statusCd = (int)code;
                    statusDesc = resp.StatusDescription.ToString();
                }

            }

            httpResp.Content = body;
            httpResp.StatusCode = statusCd.ToString();
            httpResp.StatusDesc = statusDesc;
            httpResp.NewLocation = location;

            return httpResp;
        }
    }
}