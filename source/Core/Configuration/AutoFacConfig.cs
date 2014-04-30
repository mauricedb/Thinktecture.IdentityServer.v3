/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license
 */

using System;
using Thinktecture.IdentityServer.Core.Connect;
using Thinktecture.IdentityServer.Core.Connect.Services;
using Thinktecture.IdentityServer.Core.Services;

using TinyIoC;

namespace Thinktecture.IdentityServer.Core.Configuration
{
    public static class AutofacConfig
    {
        public static TinyIoCContainer Configure(IdentityServerCoreOptions options)
        {
            if (options == null) throw new ArgumentNullException("options");
            if (options.Factory == null) throw new InvalidOperationException("null factory");
            
            IdentityServerServiceFactory fact = options.Factory;
            fact.Validate();


            var container = TinyIoCContainer.Current;

            // mandatory from factory
            container.Register((_, __) => fact.AuthorizationCodeStore());
            container.Register((_, __) => fact.CoreSettings());
            container.Register((_, __) => fact.Logger());
            container.Register((_, __) => fact.TokenHandleStore());
            container.Register((_, __) => fact.UserService());
            container.Register((_, __) => fact.ConsentService());

            // optional from factory
            if (fact.ClaimsProvider != null)
            {
                container.Register((_, __) => fact.ClaimsProvider());
            }
            else
            {
                container.Register<IClaimsProvider, DefaultClaimsProvider>();
            }

            if (fact.TokenService != null)
            {
                container.Register((_, __) => fact.TokenService());
            }
            else
            {
                container.Register<ITokenService, DefaultTokenService>();
            }

            if (fact.CustomRequestValidator != null)
            {
                container.Register((_, __) => fact.CustomRequestValidator());
            }
            else
            {
                container.Register<ICustomRequestValidator, DefaultCustomRequestValidator>();
            }

            if (fact.AssertionGrantValidator != null)
            {
                container.Register((_, __) => fact.AssertionGrantValidator());
            }
            else
            {
                container.Register<IAssertionGrantValidator, DefaultAssertionGrantValidator>();
            }

            if (fact.ExternalClaimsFilter != null)
            {
                container.Register((_, __) => fact.ExternalClaimsFilter());
            }
            else
            {
                container.Register<IExternalClaimsFilter, DefaultExternalClaimsFilter>();
            }

            // validators
            container.Register<TokenRequestValidator>();
            container.Register<AuthorizeRequestValidator>();
            container.Register<ClientValidator>();
            container.Register<TokenValidator>();

            // processors
            container.Register<TokenResponseGenerator>();
            container.Register<AuthorizeResponseGenerator>();
            container.Register<AuthorizeInteractionResponseGenerator>();
            container.Register<UserInfoResponseGenerator>();

            // for authentication
            var authenticationOptions = options.AuthenticationOptions ?? new AuthenticationOptions();
            container.Register(authenticationOptions);

            return container;
        }
    }
}