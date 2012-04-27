using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace MSDNSWebAdmin.AppCode
{
    public static class AppSettings
    {
        /// <summary>
        /// HTTPS only access to application
        /// </summary>
        public static bool HTTPSOnly = false;

        //Always logged on settings, mostly for debugging.
        public static string AlwaysLoggedOnUsername = ConfigurationManager.AppSettings["AlwaysLoggedOnUsername"];
        public static string AlwaysLoggedOnPassword = ConfigurationManager.AppSettings["AlwaysLoggedOnPassword"];
        public static string AlwaysLoggedOnDomain = ConfigurationManager.AppSettings["AlwaysLoggedOnDomain"];




        /// <summary>
        /// AppSetting's LimitToServers
        /// <para>If this is set, no other servers are allowed</para>
        /// </summary>
        public static readonly string[] LimitToServers;


        static AppSettings()
        {
            if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["HTTPSOnly"]) && (ConfigurationManager.AppSettings["HTTPSOnly"].ToLower() == "true"))
                HTTPSOnly = true;


            var limits = ConfigurationManager.AppSettings["LimitToServers"];
            if (limits == null)
                LimitToServers = new string[] { };
            LimitToServers = limits.Split(',').Where(i => !string.IsNullOrWhiteSpace(i)).Select(i => i.ToLower()).ToArray();
        }
    }
}