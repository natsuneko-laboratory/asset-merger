/*-------------------------------------------------------------------------------------------
 * Copyright (c) Fuyuno Mikazuki / Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

#if UNITY_EDITOR

using UnityEditor.Animations;

namespace Mochizuki.VRChat.Extensions.Unity
{
    public static class AnimatorTransitionExtensions
    {
        public static void CloneTo(this AnimatorTransition source, AnimatorTransition dest)
        {
            dest.mute = source.mute;
            dest.solo = source.solo;
            dest.name = source.name;
            dest.hideFlags = source.hideFlags;

            foreach (var condition in source.conditions)
                dest.AddCondition(condition.mode, condition.threshold, condition.parameter);

            // source.destinationState is ignored because it is resolved by parent
            // source.destinationStateMachine is ignored because it is resolved by parent
        }
    }
}

#endif