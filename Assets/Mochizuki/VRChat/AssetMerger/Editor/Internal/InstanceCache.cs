/*-------------------------------------------------------------------------------------------
 * Copyright (c) Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;

using Object = UnityEngine.Object;

namespace Mochizuki.VRChat.AssetMerger.Internal
{
    internal static class InstanceCache<T> where T : Object
    {
        private static readonly Dictionary<int, T> InternalCaches;

        public static IReadOnlyList<T> Caches => InternalCaches.Select(w => w.Value).ToList().AsReadOnly();

        static InstanceCache()
        {
            InternalCaches = new Dictionary<int, T>();
        }

        public static void SafeRegister(int instanceId, T obj)
        {
            if (InternalCaches.ContainsKey(instanceId))
                return;

            InternalCaches.Add(instanceId, obj);
        }

        public static T Find(int instanceId)
        {
            return InternalCaches.ContainsKey(instanceId) ? InternalCaches[instanceId] : null;
        }

        public static T FindOrCreate(T obj, Func<T, T> func)
        {
            if (obj == null)
                return default;

            var instanceId = obj.GetInstanceID();
            if (InternalCaches.ContainsKey(instanceId))
                return InternalCaches[instanceId];

            InternalCaches.Add(instanceId, func.Invoke(obj));
            return InternalCaches[instanceId];
        }

        public static void Clear()
        {
            InternalCaches.Clear();
        }
    }
}