﻿using Sitecore.Analytics.Pipelines.ExcludeRobots;
using Sitecore.Diagnostics;
using System.Web;
using TheReference.DotNet.Sitecore.ExcludeRobots.Configuration;

namespace TheReference.DotNet.Sitecore.ExcludeRobots.Pipelines
{
    public class CheckUserAgentWithWildcard : ExcludeRobotsProcessor
    {
        private static object excludeListSync = new object();
        private static WildcardExcludeList excludeList;
        private HttpContextBase context;

        public override void Process(ExcludeRobotsArgs args)
        {
            Assert.ArgumentNotNull((object)args, "args");
            Assert.IsNotNull(HttpContext, "HttpContext");
            Assert.IsNotNull(HttpContext.Request, "Request");

            var userAgent = HttpContext.Request.UserAgent;
            if (userAgent == null || !ExcludeList.ContainsUserAgent(userAgent))
            {
                //Log.Info("UserAgent NOT excluded - " + userAgent, this);
                return;
            }

            //Unwise to keep logging, as this is executed per session
            //Log.Warn("UserAgent excluded - " + userAgent, this);
            args.IsInExcludeList = true;
        }

        public static WildcardExcludeList ExcludeList
        {
            get
            {
                if (excludeList == null)
                {
                    lock (excludeListSync)
                    {
                        if (excludeList == null)
                            excludeList = WildcardExcludeList.GetExcludeList();
                    }
                }
                return excludeList;
            }
            internal set
            {
                excludeList = value;
            }
        }

        private HttpContextBase HttpContext
        {
            get
            {
                if (context != null)
                    return context;
                if (System.Web.HttpContext.Current == null)
                    return null;
                return context = new HttpContextWrapper(System.Web.HttpContext.Current);
            }
        }
    }
}