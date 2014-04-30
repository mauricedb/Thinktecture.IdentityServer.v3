/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license
 */

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Hosting;

using TinyIoC;

namespace Thinktecture.IdentityServer.Core.Configuration
{
    public class KatanaDependencyResolver : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var owin = request.GetOwinContext();
            var scope = owin.Get<TinyIoCContainer>("idsrv:TinyIoCContainer");
            if (scope != null)
            {
                request.Properties[HttpPropertyKeys.DependencyScope] = scope.GetChildContainer();
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}