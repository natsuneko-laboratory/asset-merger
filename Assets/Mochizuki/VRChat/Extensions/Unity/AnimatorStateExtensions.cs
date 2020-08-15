/*-------------------------------------------------------------------------------------------
 * Copyright (c) Fuyuno Mikazuki / Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

#if UNITY_EDITOR

using System;
using System.Linq;

using UnityEditor.Animations;

using UnityEngine;

namespace Mochizuki.VRChat.Extensions.Unity
{
    public static class AnimatorStateExtensions
    {
        public static AnimatorState CloneDeep(this AnimatorState source)
        {
            var dest = new AnimatorState();
            dest.Apply(source);

            return dest;
        }

        public static void Apply(this AnimatorState dest, AnimatorState source)
        {
            dest.cycleOffsetParameter = source.cycleOffsetParameter;
            dest.cycleOffset = source.cycleOffset;
            dest.cycleOffsetParameterActive = source.cycleOffsetParameterActive;
            dest.iKOnFeet = source.iKOnFeet;
            dest.mirror = source.mirror;
            dest.mirrorParameter = source.mirrorParameter;
            dest.mirrorParameterActive = source.mirrorParameterActive;
            dest.motion = source.motion; // keep reference
            dest.speed = source.speed;
            dest.speedParameter = source.speedParameter;
            dest.speedParameterActive = source.speedParameterActive;
            dest.tag = source.tag;
            dest.timeParameter = source.timeParameter;
            dest.timeParameterActive = source.timeParameterActive;
            dest.writeDefaultValues = source.writeDefaultValues;
            dest.name = source.name;
            dest.hideFlags = source.hideFlags;

            foreach (var sourceTransition in source.transitions.Where(w => w.isExit))
            {
                var transition = dest.AddExitTransition(false);
                sourceTransition.CloneTo(transition);

                // should always false
                if (InstanceCaches<AnimatorStateTransition>.Find(sourceTransition.GetInstanceID()) == null)
                    InstanceCaches<AnimatorStateTransition>.Register(sourceTransition.GetInstanceID(), transition);
            }

            foreach (var sourceTransition in source.transitions.Where(w => !w.isExit))
            {
                AnimatorStateTransition transition = null;
                if (sourceTransition.destinationStateMachine != null)
                    transition = dest.AddTransition(InstanceCaches<AnimatorStateMachine>.FindOrCreate(sourceTransition.destinationStateMachine, w => w.CloneDeep()));

                if (sourceTransition.destinationState != null)
                    transition = dest.AddTransition(InstanceCaches<AnimatorState>.FindOrCreate(sourceTransition.destinationState, CloneDeep));

                if (transition == null)
                    throw new ArgumentNullException(nameof(transition));

                sourceTransition.CloneTo(transition);

                // should always false
                if (InstanceCaches<AnimatorStateTransition>.Find(sourceTransition.GetInstanceID()) == null)
                    InstanceCaches<AnimatorStateTransition>.Register(sourceTransition.GetInstanceID(), transition);
            }

            foreach (var sourceBehaviour in source.behaviours)
            {
                var behaviour = dest.AddStateMachineBehaviour(sourceBehaviour.GetType());
                sourceBehaviour.CloneDeepTo(behaviour);

                // store
                InstanceCaches<StateMachineBehaviour>.Register(behaviour.GetInstanceID(), behaviour);
            }
        }
    }
}

#endif