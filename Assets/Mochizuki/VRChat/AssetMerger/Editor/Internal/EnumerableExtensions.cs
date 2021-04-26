/*-------------------------------------------------------------------------------------------
 * Copyright (c) Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace Mochizuki.VRChat.AssetMerger.Internal
{
    internal static class EnumerableExtensions
    {
        public static IEnumerable<string> Duplicate<T>(this IEnumerable<T> obj, Func<T, string> groupByFunc)
        {
            return obj.GroupBy(groupByFunc).Where(w => w.Count() > 1).Select(w => w.Key).ToList();
        }
    }
}