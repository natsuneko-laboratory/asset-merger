/*-------------------------------------------------------------------------------------------
 * Copyright (c) Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

using UnityEditor.Animations;

namespace Mochizuki.VRChat.AssetMerger.Internal
{
    internal static class AnimatorTransitionExtensions
    {
        public static void ApplyTo(this AnimatorTransition source, AnimatorTransition dest)
        {
            dest.mute = source.mute;
            dest.solo = source.solo;
            dest.name = source.name;
            dest.hideFlags = source.hideFlags;

            foreach (var condition in source.conditions)
                dest.AddCondition(condition.mode, condition.threshold, condition.parameter);
        }
    }
}