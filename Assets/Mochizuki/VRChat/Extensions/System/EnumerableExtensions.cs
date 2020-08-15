/*-------------------------------------------------------------------------------------------
 * Copyright (c) Fuyuno Mikazuki / Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;

namespace Mochizuki.VRChat.Extensions.System
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<string> Duplicate<T>(this IEnumerable<T> obj, Func<T, string> groupByFunc)
        {
            return obj.GroupBy(groupByFunc).Where(w => w.Count() > 1).Select(w => w.Key).ToList();
        }
    }
}

#endif