using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Xml.Linq;
using System.Xml;
using System.Xml.XPath;

namespace DNSManagement
{
    /// <summary>
    /// WMI Exception
    /// <para>Used to parse the WMI Generic Error exception</para>
    /// </summary>
    public class WMIException : Exception
    {
       
        /// <summary>
        /// Parses the ManagementException.ErrorInformation.
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private static string GetExceptionText(ManagementException ex)
        {
            //get exception xml
            var exceptionxml = ex.ErrorInformation.GetText(System.Management.TextFormat.CimDtd20); 

            

            var errormessage = "";

            //parse exception
            var doc = XDocument.Parse(exceptionxml);

            var descriptionelement = doc.XPathSelectElements(".//PROPERTY[@NAME='Description']").FirstOrDefault();
            if (descriptionelement != null)
            {
                var valueelement = descriptionelement.Element("VALUE");
                if (valueelement != null)
                    errormessage = valueelement.Value;
            }

            //if we have a description, use it
            if (errormessage != "")
                return errormessage;

            //otherwise, return the whole message
            var exceptiontext = ex.ErrorInformation.GetText(System.Management.TextFormat.Mof);
            return exceptiontext;
        }


        public WMIException(ManagementException ex) : base(GetExceptionText(ex),ex)
        {
        }
    }
}
