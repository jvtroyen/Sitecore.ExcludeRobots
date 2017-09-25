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

        public override void Process(ExcludeRobotsArgs args)
        {
            Assert.ArgumentNotNull((object)args, "args");

            var context = HttpContext;
            Assert.IsNotNull(context, "HttpContext");
            Assert.IsNotNull(context.Request, "Request");

            var userAgent = context.Request.UserAgent;
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
                if (System.Web.HttpContext.Current == null)
                    return null;

                return new HttpContextWrapper(System.Web.HttpContext.Current);
            }
        }
    }
}