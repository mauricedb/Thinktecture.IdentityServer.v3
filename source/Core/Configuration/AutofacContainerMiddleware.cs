/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license
 */
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using TinyIoC;

namespace Thinktecture.IdentityServer.Core.Configuration
{
    public class AutofacContainerMiddleware
    {
        readonly private Func<IDictionary<string, object>, Task> _next;
        readonly private TinyIoCContainer _container;

        public AutofacContainerMiddleware(Func<IDictionary<string, object>, Task> next, TinyIoCContainer container)
        {
            _next = next;
            _container = container;
        }

        public async Task Invoke(IDictionary<string, object> env)
        {
            var context = new OwinContext(env);

            // this creates a per-request, disposable scope
            using (var childContainer = _container.GetChildContainer())
            {
                childContainer.Register<IOwinContext>(context);
                // this makes scope available for downstream frameworks
                context.Set("idsrv:TinyIoCContainer", childContainer);
                await _next(env);
            }
        }
    }
}