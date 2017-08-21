using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Web.IPAddresses;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Xml;

namespace TheReference.DotNet.Sitecore.ExcludeRobots.Configuration
{
    public class WildcardExcludeList
    {
        private const string _wildcard = "*";

        private HashSet<string> userAgents;
        private HashSet<string> userAgentsWithWildcard;
        private IPList ipaddresses;

        public WildcardExcludeList(IPList ipaddresses, IEnumerable<string> userAgents, IEnumerable<string> userAgentsWithWildcard)
            : this(ipaddresses, new HashSet<string>(userAgents), new HashSet<string>(userAgentsWithWildcard))
        {
        }

        public WildcardExcludeList(IPList ipaddresses, HashSet<string> userAgents, HashSet<string> userAgentsWithWildcard)
        {
            this.ipaddresses = ipaddresses;
            this.userAgents = userAgents;
            this.userAgentsWithWildcard = userAgentsWithWildcard;
        }

        public static WildcardExcludeList GetExcludeList()
        {
            return new WildcardExcludeList(GetIPAddresses(), GetUserAgents(), GetUserAgentsWithWildcard());
        }

        public bool ContainsIpAddress(string addressString)
        {
            Assert.ArgumentNotNull(addressString, "addressString");
            IPAddress address;
            if (IPAddress.TryParse(addressString, out address))
                return ipaddresses.Contains(address);
            return false;
        }

        public bool ContainsUserAgent(string userAgent)
        {
            Assert.ArgumentNotNull(userAgent, "userAgent");
            return userAgents.Contains(userAgent)
                || userAgentsWithWildcard.Any(agent => new WildcardPattern(agent).IsMatch(userAgent));
        }

        private static IPList GetIPAddresses()
        {
            XmlNode configNode = Factory.GetConfigNode("analyticsExcludeRobots/excludedIPAddresses");
            if (configNode == null)
                return new IPList();
            return IPHelper.GetIPList(configNode) ?? new IPList();
        }

        private static HashSet<string> GetUserAgents()
        {
            XmlNode configNode = Factory.GetConfigNode("analyticsExcludeRobots/excludedUserAgents");
            HashSet<string> hashSet = new HashSet<string>();
            if (configNode == null)
                return hashSet;
            string innerText = configNode.InnerText;
            char[] chArray = new char[1]
            {
                '\n'
            };
            foreach (string str1 in innerText.Split(chArray))
            {
                string str2 = str1.Trim();
                if (!(str2 == string.Empty) && !str2.StartsWith("#") && !str2.Contains(_wildcard) && !hashSet.Contains(str2))
                    hashSet.Add(str2);
            }
            return hashSet;
        }

        private static HashSet<string> GetUserAgentsWithWildcard()
        {
            XmlNode configNode = Factory.GetConfigNode("analyticsExcludeRobots/excludedUserAgents");
            HashSet<string> hashSet = new HashSet<string>();
            if (configNode == null)
                return hashSet;
            string innerText = configNode.InnerText;
            char[] chArray = new char[1]
            {
                '\n'
            };
            foreach (string str1 in innerText.Split(chArray))
            {
                string str2 = str1.Trim();
                if (!(str2 == string.Empty) && !str2.StartsWith("#") && str2.Contains(_wildcard) && !hashSet.Contains(str2))
                    hashSet.Add(str2);
            }
            return hashSet;
        }
    }
}
