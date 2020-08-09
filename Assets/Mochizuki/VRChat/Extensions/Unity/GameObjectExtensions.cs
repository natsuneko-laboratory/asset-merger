/*-------------------------------------------------------------------------------------------
 * Copyright (c) Fuyuno Mikazuki / Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

#if UNITY_EDITOR

using System.Collections.Generic;

using UnityEngine;

namespace Mochizuki.VRChat.Extensions.Unity
{
    public static class GameObjectExtensions
    {
        public static string GetAbsolutePath(this GameObject obj)
        {
            var paths = new List<string>();
            var current = obj.transform;

            while (current != null && current != obj.transform)
            {
                paths.Add(current.name);
                current = current.parent;
            }

            paths.Reverse();
            return string.Join("/", paths);
        }

        public static string GetRelativePathFor(this GameObject obj, GameObject child)
        {
            var paths = new List<string>();
            var current = child.transform;

            while (current != null && current != obj.transform)
            {
                paths.Add(current.name);
                current = current.parent;
            }

            paths.Reverse();
            return string.Join("/", paths);
        }
    }
}

#endif