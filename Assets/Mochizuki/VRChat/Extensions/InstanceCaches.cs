/*-------------------------------------------------------------------------------------------
 * Copyright (c) Fuyuno Mikazuki / Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Object = UnityEngine.Object;

namespace Mochizuki.VRChat.Extensions
{
    // Why static class?
    // It doesn't matter where the references comes from, as long as it's in the same session.
    // It also prevents parameters bucket relays.
    public static class InstanceCaches<T> where T : Object
    {
        private static readonly Dictionary<int, T> InternalCaches;

        public static IReadOnlyList<T> Caches => InternalCaches.Select(w => w.Value).ToList().AsReadOnly();

        static InstanceCaches()
        {
            InternalCaches = new Dictionary<int, T>();
        }

        public static void Register(int instanceId, T obj)
        {
            InternalCaches.Add(instanceId, obj);
        }

        public static T Find(int instanceId)
        {
            return InternalCaches.ContainsKey(instanceId) ? InternalCaches[instanceId] : null;
        }

        public static T FindOrCreate(T obj, Func<T, T> func)
        {
            if (obj == null)
            {
                Debug.LogWarning($"[Mochizuki.VRChat.AssetMerger] {nameof(obj)} is null, return default(T).");
                return default;
            }

            var instanceId = obj.GetInstanceID();
            if (InternalCaches.ContainsKey(instanceId))
            {
                Debug.Log($"return caches of instance {instanceId}");
                return InternalCaches[instanceId];
            }

            InternalCaches.Add(instanceId, func.Invoke(obj));
            Debug.Log($"return new instance of {instanceId}");
            return InternalCaches[instanceId];
        }

        public static void Clear()
        {
            InternalCaches.Clear();
        }
    }
}

#endif