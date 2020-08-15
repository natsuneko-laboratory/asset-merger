/*-------------------------------------------------------------------------------------------
 * Copyright (c) Fuyuno Mikazuki / Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

#if UNITY_EDITOR

using UnityEditor.Animations;

namespace Mochizuki.VRChat.Extensions.Unity
{
    public static class AnimatorStateTransitionExtensions
    {
        public static void CloneTo(this AnimatorStateTransition source, AnimatorStateTransition dest)
        {
            dest.orderedInterruption = source.orderedInterruption;
            dest.canTransitionToSelf = source.canTransitionToSelf;
            dest.duration = source.duration;
            dest.exitTime = source.exitTime;
            dest.hasFixedDuration = source.hasFixedDuration;
            dest.interruptionSource = source.interruptionSource;
            dest.mute = source.mute;
            dest.solo = source.solo;
            dest.name = source.name;
            dest.hideFlags = source.hideFlags;

            foreach (var sourceCondition in source.conditions)
                dest.AddCondition(sourceCondition.mode, sourceCondition.threshold, sourceCondition.parameter);

            // source.destinationState is ignored because it is resolved by parent
            // source.destinationStateMachine is ignored because it is resolved by parent
        }
    }
}

#endif