using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Twitter_Service
{

    [ServiceContract]
    public interface ITwitterService
    {
        /* 
         * Search by #hashtags 
         * http://localhost:58059/TwitterService.svc/search/tweets?q=tempe
         */
        [OperationContract]
        [WebGet(UriTemplate = "/search/tweets?q={str}", BodyStyle = WebMessageBodyStyle.Bare)]
        string searchByHashtag(string str);

        /* 
         * Popular trends in Phoenix 
         * http://localhost:58059/TwitterService.svc/trends/place?id=phoenix
         */
        [OperationContract]
        [WebGet(UriTemplate = "/trends/place?id={woeid}")]
        string getLocationTrends(string woeid);

        /*
         * Search tweets by twitter handle 
         * http://localhost:58059/TwitterService.svc/statuses?handle=rudraksh125
         */
        [OperationContract]
        [WebGet(UriTemplate = "/statuses?handle={handle}")]
        string getUserTweets(string handle);
    }
}


