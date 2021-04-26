/*-------------------------------------------------------------------------------------------
 * Copyright (c) Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

using System;
using System.Linq;

using UnityEditor.Animations;

using UnityEngine;

namespace Mochizuki.VRChat.AssetMerger.Internal
{
    internal static class AnimatorStateExtensions
    {
        public static AnimatorState CloneDeep(this AnimatorState source)
        {
            var dest = new AnimatorState
            {
                cycleOffset = source.cycleOffset,
                cycleOffsetParameter = source.cycleOffsetParameter,
                cycleOffsetParameterActive = source.cycleOffsetParameterActive,
                iKOnFeet = source.iKOnFeet,
                mirror = source.mirror,
                mirrorParameter = source.mirrorParameter,
                mirrorParameterActive = source.mirrorParameterActive,
                motion = source.motion,
                speed = source.speed,
                speedParameter = source.speedParameter,
                speedParameterActive = source.speedParameterActive,
                tag = source.tag,
                timeParameter = source.timeParameter,
                timeParameterActive = source.timeParameterActive,
                writeDefaultValues = source.writeDefaultValues,
                name = source.name,
                hideFlags = source.hideFlags
            };

            foreach (var sourceTransition in source.transitions.Where(w => w.isExit))
            {
                var transition = dest.AddExitTransition(false);
                sourceTransition.ApplyTo(transition);

                InstanceCache<AnimatorStateTransition>.SafeRegister(sourceTransition.GetInstanceID(), transition);
            }

            foreach (var sourceTransition in source.transitions.Where(w => !w.isExit))
            {
                AnimatorStateTransition transition = null;
                if (sourceTransition.destinationStateMachine != null)
                    transition = dest.AddTransition(InstanceCache<AnimatorStateMachine>.FindOrCreate(sourceTransition.destinationStateMachine, w => w.CloneDeep()));

                if (sourceTransition.destinationState != null)
                    transition = dest.AddTransition(InstanceCache<AnimatorState>.FindOrCreate(sourceTransition.destinationState, w => w.CloneDeep()));

                if (transition == null)
                    throw new ArgumentNullException(nameof(transition));

                sourceTransition.ApplyTo(transition);

                InstanceCache<AnimatorStateTransition>.SafeRegister(sourceTransition.GetInstanceID(), transition);
            }

            foreach (var sourceBehaviour in source.behaviours)
            {
                var behaviour = dest.AddStateMachineBehaviour(sourceBehaviour.GetType());
                sourceBehaviour.ApplyTo(behaviour);

                InstanceCache<StateMachineBehaviour>.SafeRegister(behaviour.GetInstanceID(), behaviour);
            }

            return dest;
        }
    }
}