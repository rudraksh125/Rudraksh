using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : Page
{
    char[] sp = new char[] { '*', '*', '*', '*', '*' };
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void buttonInvoke_Click(object sender, EventArgs e)
    {
        String base_URL = "http://localhost:58059/TwitterService.svc/";
        String param = "/statuses?handle=" + TextBoxHandle.Text.Trim();
        WebRequest requestStoreLocation = WebRequest.Create(base_URL + param);
        WebResponse ws = requestStoreLocation.GetResponse();
        HttpWebResponse httpresponse = (System.Net.HttpWebResponse)ws;
        String output = "";
        if (httpresponse.StatusCode == HttpStatusCode.OK)
        {
            Stream respStream = httpresponse.GetResponseStream();
            StreamReader reader = new StreamReader(respStream, Encoding.UTF8);
            output = reader.ReadToEnd();
            output = output.Substring(output.IndexOf('>')+1);
            string processed = null;
            string[] words = output.Split(sp);
            if (words.Length > 1)
            {
                for (int i = 0; i < words.Length; i++)
                {
                    processed += '@' + words[i] + "\n";
                }
            }
            TextBoxHandleTweets.Rows = 20;
            TextBoxHandleTweets.TextMode = TextBoxMode.MultiLine;
            processed = processed.Replace("</string>", "");
            TextBoxHandleTweets.Text = processed;
        }
        else
        {
            throw new Exception(String.Format("Server error (HTTP {0}: {1}).", httpresponse.StatusCode, httpresponse.StatusDescription));
        }
    }
    protected void ButtonSearch_Click(object sender, EventArgs e)
    {
        String base_URL = "http://localhost:58059/TwitterService.svc/";
        String param = "/search/tweets?q=" + TextBoxHas.Text.Trim();
        WebRequest requestStoreLocation = WebRequest.Create(base_URL + param);
        WebResponse ws = requestStoreLocation.GetResponse();
        HttpWebResponse httpresponse = (System.Net.HttpWebResponse)ws;
        String output = "";
        if (httpresponse.StatusCode == HttpStatusCode.OK)
        {
            Stream respStream = httpresponse.GetResponseStream();
            StreamReader reader = new StreamReader(respStream, Encoding.UTF8);
            output = reader.ReadToEnd();
            String processed = null;
            String temp = output.Substring(output.IndexOf('>')+1);
            string[] words = temp.Split(sp);
            if (words.Length > 1)
            {
                for (int i=0;i<words.Length;i++)
                {
                    processed += '@' + words[i] + "\n";
                }
            }
            TextBoxHashtag.Rows = 20;
            TextBoxHashtag.TextMode = TextBoxMode.MultiLine;
            processed = processed.Replace("</string>", "");
            TextBoxHashtag.Text = processed;
        }
        else
        {
            throw new Exception(String.Format("Server error (HTTP {0}: {1}).", httpresponse.StatusCode, httpresponse.StatusDescription));
        }
    }
    protected void ButtonTrends_Click(object sender, EventArgs e)
    {

        String base_URL = "http://localhost:58059/TwitterService.svc/";
        String param = "/trends/place?id=phoenix";
        WebRequest requestStoreLocation = WebRequest.Create(base_URL + param);
        WebResponse ws = requestStoreLocation.GetResponse();
        HttpWebResponse httpresponse = (System.Net.HttpWebResponse)ws;
        String output = "";
        if (httpresponse.StatusCode == HttpStatusCode.OK)
        {
            Stream respStream = httpresponse.GetResponseStream();
            StreamReader reader = new StreamReader(respStream, Encoding.UTF8);
            output = reader.ReadToEnd();
            String processed = null;
            String temp = output.Substring(output.IndexOf('>')+1);
            string[] words = temp.Split(sp);
            if (words.Length > 1)
            {
                for (int i = 0; i < words.Length; i++)
                {
                    processed += '@' + words[i] + "\n";
                }
            }
            TextBoxTrends.Rows = 20;
            TextBoxTrends.TextMode = TextBoxMode.MultiLine;
            processed = processed.Replace("</string>", "");
            TextBoxTrends.Text = processed;
        }
        else
        {
            throw new Exception(String.Format("Server error (HTTP {0}: {1}).", httpresponse.StatusCode, httpresponse.StatusDescription));
        }
    }
}