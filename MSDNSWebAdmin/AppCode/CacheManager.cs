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
using System.Collections.Concurrent;

namespace MSDNSWebAdmin.AppCode
{
    public class CacheManager
    {
        private class CacheItem
        {
            public DateTime Timestamp { get; set; }
            public DateTime Expires {get;set;}
            public object Item { get; set; }
        }

        private static ConcurrentDictionary<string, ConcurrentDictionary<string, CacheItem>> m_stores = new ConcurrentDictionary<string, ConcurrentDictionary<string, CacheItem>>();

        private static ConcurrentDictionary<string,CacheItem> GetStore(string store)
        {
            if (!m_stores.ContainsKey(store))
                m_stores[store] = new ConcurrentDictionary<string,CacheItem>();
            return m_stores[store];
        }

        public static TItem Get<TItem>(string storename, string key)
        {
            var store = GetStore(storename);

            if (!store.ContainsKey(key))
                return default(TItem);

            var item = store[key];
            if (item.Expires < DateTime.UtcNow)
                return default(TItem);

            return (TItem)item.Item;
        }

        public static void Set<TItem>(string storename, string key, DateTime expiresUtc, TItem item)
        {
            var store = GetStore(storename);
            store[key] = new CacheItem{
                Timestamp = DateTime.UtcNow,
                Expires = expiresUtc,
                Item = item
            };

        }
    }
}