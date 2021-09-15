using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VerticurlPoc.Foundation.NetworkType.Pipeline.RunServiceWorker
{
    public class NetworkTypeRetrievalService
    {
        public static string RetrieveNetworkType()
        {
            var context = HttpContext.Current;
            string[] header = context.Request.Headers.AllKeys;
            if (header.Contains("network-type"))
            {
                return context.Request.Headers.Get("network-type");
            }
            return string.Empty;
        }

            
    }
}