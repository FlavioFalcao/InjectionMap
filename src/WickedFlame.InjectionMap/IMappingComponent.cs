﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace WickedFlame.InjectionMap
{
    public interface IMappingComponent
    {
        Guid ID { get; }

        Type KeyType { get; }

        Type ValueType { get; }

        Expression<Func<object>> ValueCallback { get; set; }

        IMappingOption MappingOption { get; }

        IList<IBindingArgument> Arguments { get; }
    }
}
