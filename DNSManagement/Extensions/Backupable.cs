using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace DNSManagement.Extensions
{
    public abstract class Backupable
    {
        public virtual string ToConfigurationFile()
        {
            StringBuilder sb = new StringBuilder();

            Type type = this.GetType();
            var props = type.GetProperties();
            foreach (var prop in props)
            {
                sb.AppendLineFormat("{0}={1}", prop.Name, prop.GetValue(this, null));
            }

            return sb.ToString();
        }
    }
}
