/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license
 */

using System;
using System.Net.Http;

using TinyIoC;

namespace Thinktecture.IdentityServer.Core.Extensions
{
    public static class HttpRequestMessageExtensions
    {
        public static string GetBaseUrl(this HttpRequestMessage request, string host = null)
        {
            if (host.IsMissing())
            {
                host = "https://" + request.Headers.Host;
            }

            var baseUrl = new Uri(new Uri(host), request.GetRequestContext().VirtualPathRoot).AbsoluteUri;
            if (!baseUrl.EndsWith("/")) baseUrl += "/";

            return baseUrl;
        }

        public static TinyIoCContainer GetAutofacScope(this HttpRequestMessage request)
        {
            var owinContext = request.GetOwinContext();
            var scope = owinContext.Get<TinyIoCContainer>("idsrv:TinyIoCContainer");

            return scope;
        }
    }
}
