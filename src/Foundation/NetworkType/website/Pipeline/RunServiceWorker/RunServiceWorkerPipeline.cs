using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Pipelines.HttpRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VerticurlPoc.Foundation.NetworkType.Pipeline.RunServiceWorker
{
    public class RunServiceWorkerPipeline : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            HttpContext currentHttpContext = HttpContext.Current;
            bool isLiteVersionEnabled = false;
            if (currentHttpContext == null || Context.Database == null)
                return;
            if (Context.Site.Name.ToLower() == "shell")
                return;
            var litePageSettingID = Sitecore.Configuration.Settings.GetSetting("LiteVesionSettingItemID");
            var siteItem = Sitecore.Context.Database.GetItem(litePageSettingID);
            if(siteItem != null)
            {
                //Read the Multilist Field
                Sitecore.Data.Fields.MultilistField multiselectField = siteItem.Fields["LiteVersionPages"];

                Sitecore.Data.Items.Item[] items = multiselectField.GetItems();
                //Iterate through each item
                if (items != null && items.Length > 0)
                {
                    foreach (Item lvItem in items)
                    {
                        if (lvItem.Fields["Route"].Value == Context.HttpContext.Request.Url.AbsolutePath)
                        {
                            isLiteVersionEnabled = true;
                        }
                    }
                }
                if (isLiteVersionEnabled)
                {
                    var networkType = NetworkTypeRetrievalService.RetrieveNetworkType();
                    var slowNetork = Sitecore.Configuration.Settings.GetSetting("SlowNetwork");
                    //var fastNetork = Sitecore.Configuration.Settings.GetSetting("FastNetwork");
                    if (string.IsNullOrEmpty(networkType))
                    {
                        currentHttpContext.Response.Redirect(currentHttpContext.Request.Url.Scheme + "://" + currentHttpContext.Request.Url.Host + "/redirect.html");
                    }
                    else if (slowNetork.Contains(networkType))
                    {
                        SetDevice("SlowNetwork");
                        return;
                    }
                    //else if (fastNetork.Contains(networkType))
                    //{
                    //    SetDevice("Defult");
                    //    return;
                    //}
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
            
            


        }
        private void SetDevice(string deviceName)
        {
            DeviceItem device = Context.Database.Resources.Devices["Default"];
            device = Context.Database.Resources.Devices[deviceName];
            Context.Device = device;
        }
    }
}