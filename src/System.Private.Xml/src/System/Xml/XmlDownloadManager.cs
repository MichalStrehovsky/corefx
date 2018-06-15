// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Xml
{
    using System;
    using System.IO;
    using System.Security;
    using System.Collections;
    using System.Net;
    using System.Net.Cache;
    using System.Runtime.CompilerServices;
    using System.Runtime.Versioning;
    using System.Net.Http;

    //
    // XmlDownloadManager
    //
    internal partial class XmlDownloadManager
    {
        internal Stream GetStream(Uri uri, ICredentials credentials, IWebProxy proxy,
            RequestCachePolicy cachePolicy)
        {
            if (uri.Scheme == "file")
            {
                return new FileStream(uri.LocalPath, FileMode.Open, FileAccess.Read, FileShare.Read, 1);
            }
            else
            {
                return GetNonFileStream(uri, credentials, proxy, cachePolicy);
            }
        }

        private Stream GetNonFileStream(Uri uri, ICredentials credentials, IWebProxy proxy,
            RequestCachePolicy cachePolicy)
        {
            WebRequest req = CreateWebRequest(uri, credentials, proxy, cachePolicy);

            using (WebResponse resp = req.GetResponse())
            using (Stream respStream = resp.GetResponseStream())
            {
                var result = new MemoryStream();
                respStream.CopyTo(result);
                result.Position = 0;
                return result;
            }
        }

        private static WebRequest CreateWebRequest(Uri uri, ICredentials credentials, IWebProxy proxy,
            RequestCachePolicy cachePolicy)
        {
            WebRequest req = CreateWebRequestInternal(uri, credentials, proxy, cachePolicy);
            if (req != null)
            {
                return req;
            }
            else
            {
                throw new CodeRemovedException(NonFileUrlSupportSwitchName);
            }
        }

        private const string NonFileUrlSupportSwitchName = "System.Xml.XmlUrlResolver.NonFileUrlSupport";

        [Removable(NonFileUrlSupportSwitchName)]
        private static WebRequest CreateWebRequestInternal(Uri uri, ICredentials credentials, IWebProxy proxy,
            RequestCachePolicy cachePolicy)
        {
            WebRequest req = WebRequest.Create(uri);
            if (credentials != null)
            {
                req.Credentials = credentials;
            }
            if (proxy != null)
            {
                req.Proxy = proxy;
            }
            if (cachePolicy != null)
            {
                req.CachePolicy = cachePolicy;
            }
            return req;
        }
    }
}
