/*
DNS Web Admin - MS DNS Web Administration
Copyright (C) 2011 Dror Gluska
	
This program is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License along
with this program; if not, write to the Free Software Foundation, Inc.,
51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.



Change log:
2011-05-17 - Initial version

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MSDNSWebAdmin.Controllers
{
    /// <summary>
    /// Base Controller for DNS Web Management
    /// </summary>
    public class ControllerBase : Controller
    {
        /// <summary>
        /// store loggers for component/methods.
        /// </summary>
        private static System.Collections.Concurrent.ConcurrentDictionary<string, log4net.ILog> m_loggers = new System.Collections.Concurrent.ConcurrentDictionary<string, log4net.ILog>();

        /// <summary>
        /// Gets a logger instance
        /// </summary>
        /// <param name="component">Component/Controller name</param>
        /// <param name="method">The method which inserted the log record</param>
        /// <returns></returns>
        public log4net.ILog Logger(string component,string method)
        {
            var key = component + "(" + method + ")";
            log4net.ILog log;
            if (m_loggers.TryGetValue(key, out log))
                return log;
            log = log4net.LogManager.GetLogger(key);

            m_loggers[key] = log;

            return log;
        }

        /// <summary>
        /// Executes when an unhandled exception happens in the controller.
        /// </summary>
        /// <param name="filterContext">Information about the current request and action.</param>
        protected override void OnException(ExceptionContext filterContext)
        {
            Logger("ControllerBase", "OnException").FatalFormat("Unhandled exception {0}", filterContext.Exception.ToString());
            base.OnException(filterContext);
        }

    }
}
