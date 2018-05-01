using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;

namespace HTTPReqHelper
{
    public class ResponseInfo
    {
        public string Status { get; set; }
        public string Content { get; set; }
    }

    public class Websites
    {
        public string URL { get; set; }

    }
    public class WebRequestGetExample
    {
        public static void Main()
        {
            // Create a request for the URL. 

            List<string> webList = new List<string>();

            webList.Add("http://www.google.com");
            webList.Add("http://www.yahoo.com/123");
            webList.Add("http://www.lowyat.net");
            webList.Add("http://www.youtubexxxblablabla.com/123");
            webList.Add("http://www.faxcebook.com");

            foreach(var url in webList)
            {
                ResponseInfo resp = Browse(url);
                Console.WriteLine($"{url} : {resp.Status}");
            }

            Console.ReadKey();
        }

        public static ResponseInfo Browse(string url)
        {

            string body = "";
            int statusCd = 0;

            ResponseInfo httpResp = new ResponseInfo();

            WebRequest request = WebRequest.Create(url);
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

                // Clean up the streams and the response.
                reader.Close();
                response.Close();

            }
            catch (WebException we)
            {
                statusCd = -1;
                HttpWebResponse resp = (HttpWebResponse)we.Response;
                if (resp != null)
                {
                    HttpStatusCode code = resp.StatusCode;
                    statusCd = (int)code;
                }

            }

            httpResp.Content = body;
            httpResp.Status = statusCd.ToString();
            return httpResp;
        }
    }
}
