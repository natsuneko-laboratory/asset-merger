/*-------------------------------------------------------------------------------------------
 * Copyright (c) Fuyuno Mikazuki / Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

#if UNITY_EDITOR
#if VRC_SDK_VRCSDK3

using System;
using System.Linq;

using UnityEngine;

using VRC.SDK3.Avatars.ScriptableObjects;

namespace Mochizuki.VRChat.Extensions.VRC
{
    // ReSharper disable once InconsistentNaming
    public static class VRCExpressionsMenuExtensions
    {
        public static void MergeExpressions(this VRCExpressionsMenu source, params VRCExpressionsMenu[] expressions)
        {
            var count = source.controls.Count;
            foreach (var control in expressions.SelectMany(w => w.controls))
            {
                count++;

                if (count >= VRCExpressionsMenu.MAX_CONTROLS)
                    throw new ArgumentOutOfRangeException($"This exceeds the maximum number that can be set in {nameof(VRCExpressionsMenuExtensions)}");

                if (source.controls.Any(w => w.name == control.name))
                {
                    Debug.LogWarning($"The menu {control.name} is already registered.");
                    continue;
                }

                source.controls.Add(control);
            }
        }
    }
}

#endif
#endif