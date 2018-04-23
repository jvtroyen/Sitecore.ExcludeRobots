using Sitecore.Analytics.Pipelines.ExcludeRobots;
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
            //Empty userAgents are treated as bad
            if (!string.IsNullOrEmpty(userAgent) && !ExcludeList.ContainsUserAgent(userAgent))
            {
                return;
            }

            args.IsInExcludeList = true;
        }

        public static WildcardExcludeList ExcludeList
        {
            get
            {
                //TODO: modify this, so it is cached for 10 hours or so
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