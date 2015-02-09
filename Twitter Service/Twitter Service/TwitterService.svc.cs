using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Twitter_Service
{

    public class TwitterService : ITwitterService
    {
        static string access_token;

        private static void _oauth_permissions()
        {

            string oauth_consumer_key = "";
            string oauth_consumer_secret = "";

            /* Step 1: Encode consumer key and secret */
            string encoded_consumer_token = oauth_consumer_key + ":" + oauth_consumer_secret;
            string base64_encoded_consumer_token = Base64Encode(encoded_consumer_token);

            /* Step 2: Obtain a bearer token 
             * POST oauth2/token 
             */

            String resource_url = "https://api.twitter.com/oauth2/token";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(resource_url);
            request.Headers.Add("Authorization", "Basic " + base64_encoded_consumer_token);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            string postBody = "grant_type=client_credentials";

            request.Headers.Add("Accept-Encoding", "gzip,deflate");
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (Stream stream = request.GetRequestStream())
            {
                byte[] content = Encoding.UTF8.GetBytes(postBody);
                stream.Write(content, 0, content.Length);
                stream.Close();
            }

            WebResponse response = request.GetResponse();
            System.Net.HttpWebResponse httpResponse = (System.Net.HttpWebResponse)response;
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {

                Stream respStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(respStream, Encoding.UTF8);
                String output = reader.ReadToEnd();

                /*
                * Output: {"token_type":"bearer","access_token":"AAAAAAAAAAAAAAAAAAAAAL5ObAAAAAAA%
                           2B3yUocglUbpIklXvJ1VitA%2BxCuA%3DEn0pgZXsXumgvfma5YkRFD2Aov59fNFOQjSSLty9B46RZ3H
                           FFb"}
                */

                dynamic stuff = JsonConvert.DeserializeObject(output);
                string token_type = stuff.token_type;
                access_token = stuff.access_token;
                Console.WriteLine("token_type: " + token_type + "\naccess_token: " + access_token);
                if (!"bearer".Equals(token_type))
                {
                    Console.WriteLine("ERROR: Token type is not bearer!");
                }
                reader.Close();
                respStream.Close();


            }
            else
            {
                throw new Exception(String.Format("Server error (HTTP {0}: {1}).", httpResponse.StatusCode, httpResponse.StatusDescription));
            }
            response.Close();
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public TwitterService()
        {
            _oauth_permissions();
        }
        string ITwitterService.searchByHashtag(string str)
        {
            String resource_url = "https://api.twitter.com/1.1/search/tweets.json?q=" + str + "&result_type=popular&count=10&lang=en";
            StringBuilder returnTrends = new StringBuilder();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(resource_url);
            request.Headers.Add("Authorization", "Bearer " + (access_token));
            request.Method = "GET";
            request.UserAgent = "Rudraksh";
            request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            request.Headers.Add("Accept-Encoding", "gzip,deflate");
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
            
            HttpWebResponse httpResponse = null;
            try
            {
                httpResponse = (System.Net.HttpWebResponse)request.GetResponse();
            }
            catch (Exception r)
            {
                throw r;
            }

            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                Stream respStream = httpResponse.GetResponseStream();
                StreamReader reader = new StreamReader(respStream, Encoding.UTF8);
                String output = reader.ReadToEnd();

                dynamic json = JsonConvert.DeserializeObject(output);
                JArray len = json.statuses;
                int length = len.Count;

                Console.WriteLine("POPULAR TWEETS");
                for (int i = 0; i < length; i++)
                {
                    string tweet = json.statuses[i].text;
                    string handle = json.statuses[i].user.screen_name;
                    returnTrends.Append("@" + handle + " :: " + tweet + "*****");
                }
            }
            else
            {
                throw new Exception(String.Format("Server error (HTTP {0}: {1}).", httpResponse.StatusCode, httpResponse.StatusDescription));
            }
            return returnTrends.ToString();
        }

        string ITwitterService.getLocationTrends(string woeid)
        {
            String resource_url = "https://api.twitter.com/1.1/trends/place.json?id=2471390";
            StringBuilder returnTrends = new StringBuilder();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(resource_url);
            request.Headers.Add("Authorization", "Bearer " + (access_token));
            request.Method = "GET";
            request.UserAgent = "Rudraksh";
            request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            request.Headers.Add("Accept-Encoding", "gzip,deflate");
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            
            HttpWebResponse httpResponse = null;
            try
            {
                httpResponse = (System.Net.HttpWebResponse)request.GetResponse();
            }
            catch (Exception r)
            {
                throw r;
            }

            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                Stream respStream = httpResponse.GetResponseStream();
                StreamReader reader = new StreamReader(respStream, Encoding.UTF8);
                String output = reader.ReadToEnd();

                dynamic json = JsonConvert.DeserializeObject(output);
                JArray len = json[0].trends;
                int length = len.Count;

                Console.WriteLine("TRENDING NOW at Phoenix");
                for (int i = 0; i < length; i++)
                {
                    string name = json[0].trends[i].name;
                    returnTrends.Append(name + "*****");
                }
            }
            else
            {
                throw new Exception(String.Format("Server error (HTTP {0}: {1}).", httpResponse.StatusCode, httpResponse.StatusDescription));
            }
            return returnTrends.ToString();
        }

        string ITwitterService.getUserTweets(string handle)
        {
            String resource_url = "https://api.twitter.com/1.1/statuses/user_timeline.json?screen_name=" + handle + "&count=4&trim_user=true";
            StringBuilder returnTrends = new StringBuilder();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(resource_url);
            request.Headers.Add("Authorization", "Bearer " + (access_token));
            request.Method = "GET";
            request.UserAgent = "Rudraksh";
            request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            request.Headers.Add("Accept-Encoding", "gzip,deflate");
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            
            HttpWebResponse httpResponse = null;
            try
            {
                httpResponse = (System.Net.HttpWebResponse)request.GetResponse();
            }
            catch (Exception r)
            {
                throw r;
            }

            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                Stream respStream = httpResponse.GetResponseStream();
                StreamReader reader = new StreamReader(respStream, Encoding.UTF8);
                String output = reader.ReadToEnd();

                dynamic json = JsonConvert.DeserializeObject(output);
                JArray len = json;
                int length = len.Count;

                Console.WriteLine("USER TWEETS @" + handle);
                for (int i = 0; i < length; i++)
                {
                    string tweet = json[i].text;
                    string created_at = json[i].created_at;
                    returnTrends.Append("@" + handle + " :: " + tweet + " :: " + created_at + "*****");
                }
            }
            else
            {
                throw new Exception(String.Format("Server error (HTTP {0}: {1}).", httpResponse.StatusCode, httpResponse.StatusDescription));
            }
            return returnTrends.ToString();
        }
    }
}
