/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;

using TinyIoC;

public class AutofacScope : IDependencyScope
{
    TinyIoCContainer scope;

    public AutofacScope(TinyIoCContainer scope)
    {
        this.scope = scope;
    }
    
    public object GetService(System.Type serviceType)
    {
        object resolvedType;
        scope.TryResolve(serviceType, out resolvedType);
        return resolvedType;
    }

    public System.Collections.Generic.IEnumerable<object> GetServices(System.Type serviceType)
    {
        if (!scope.CanResolve(serviceType))
        {
            return Enumerable.Empty<object>();
        }

        Type type = typeof(IEnumerable<>).MakeGenericType(new Type[] { serviceType });
        return (IEnumerable<object>)scope.Resolve(type);
    }

    public void Dispose()
    {
    }
}