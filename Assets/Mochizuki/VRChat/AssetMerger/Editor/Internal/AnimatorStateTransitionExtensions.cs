/*-------------------------------------------------------------------------------------------
 * Copyright (c) Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

using UnityEditor.Animations;

namespace Mochizuki.VRChat.AssetMerger.Internal
{
    internal static class AnimatorStateTransitionExtensions
    {
        public static void ApplyTo(this AnimatorStateTransition source, AnimatorStateTransition dest)
        {
            dest.orderedInterruption = source.orderedInterruption;
            dest.canTransitionToSelf = source.canTransitionToSelf;
            dest.duration = source.duration;
            dest.exitTime = source.exitTime;
            dest.hasExitTime = source.hasExitTime;
            dest.hasFixedDuration = source.hasFixedDuration;
            dest.interruptionSource = source.interruptionSource;
            dest.mute = source.mute;
            dest.solo = source.solo;
            dest.name = source.name;
            dest.hideFlags = source.hideFlags;

            foreach (var condition in source.conditions)
                dest.AddCondition(condition.mode, condition.threshold, condition.parameter);
        }
    }
}