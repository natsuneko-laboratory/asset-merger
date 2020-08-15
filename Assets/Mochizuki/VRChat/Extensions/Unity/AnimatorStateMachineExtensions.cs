/*-------------------------------------------------------------------------------------------
 * Copyright (c) Fuyuno Mikazuki / Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

#if UNITY_EDITOR

using System;

using UnityEditor.Animations;

using UnityEngine;

namespace Mochizuki.VRChat.Extensions.Unity
{
    public static class AnimatorStateMachineExtensions
    {
        public static AnimatorStateMachine CloneDeep(this AnimatorStateMachine source)
        {
            var dest = new AnimatorStateMachine
            {
                defaultState = InstanceCaches<AnimatorState>.FindOrCreate(source.defaultState, w => w.CloneDeep()),
                anyStatePosition = source.anyStatePosition,
                entryPosition = source.entryPosition,
                exitPosition = source.exitPosition,
                parentStateMachinePosition = source.parentStateMachinePosition,
                hideFlags = source.hideFlags,
                name = source.name
            };

            foreach (var sourceState in source.states)
                dest.AddState(InstanceCaches<AnimatorState>.FindOrCreate(sourceState.state, w => w.CloneDeep()), sourceState.position);

            foreach (var sourceTransition in source.anyStateTransitions)
            {
                AnimatorStateTransition transition = null;
                if (sourceTransition.destinationStateMachine != null)
                    transition = dest.AddAnyStateTransition(InstanceCaches<AnimatorStateMachine>.FindOrCreate(sourceTransition.destinationStateMachine, CloneDeep));

                if (sourceTransition.destinationState != null)
                    transition = dest.AddAnyStateTransition(InstanceCaches<AnimatorState>.FindOrCreate(sourceTransition.destinationState, w => w.CloneDeep()));

                if (transition == null)
                    throw new ArgumentNullException(nameof(transition));

                sourceTransition.CloneTo(transition);

                // should always false
                if (InstanceCaches<AnimatorStateTransition>.Find(sourceTransition.GetInstanceID()) == null)
                    InstanceCaches<AnimatorStateTransition>.Register(sourceTransition.GetInstanceID(), transition);
            }

            foreach (var sourceTransition in source.entryTransitions)
            {
                AnimatorTransition transition = null;
                if (sourceTransition.destinationStateMachine != null)
                    transition = dest.AddEntryTransition(InstanceCaches<AnimatorStateMachine>.FindOrCreate(sourceTransition.destinationStateMachine, CloneDeep));

                if (sourceTransition.destinationState != null)
                    transition = dest.AddEntryTransition(InstanceCaches<AnimatorState>.FindOrCreate(sourceTransition.destinationState, w => w.CloneDeep()));

                if (transition == null)
                    throw new ArgumentNullException(nameof(transition));

                transition.CloneTo(sourceTransition);

                // should always false
                if (InstanceCaches<AnimatorTransition>.Find(sourceTransition.GetInstanceID()) == null)
                    InstanceCaches<AnimatorTransition>.Register(sourceTransition.GetInstanceID(), transition);
            }

            foreach (var sourceBehaviour in source.behaviours)
            {
                var behaviour = dest.AddStateMachineBehaviour(sourceBehaviour.GetType());
                sourceBehaviour.CloneDeepTo(behaviour);

                // store
                InstanceCaches<StateMachineBehaviour>.Register(behaviour.GetInstanceID(), behaviour);
            }

            foreach (var sourceStateMachine in source.stateMachines)
                dest.AddStateMachine(InstanceCaches<AnimatorStateMachine>.FindOrCreate(sourceStateMachine.stateMachine, CloneDeep), sourceStateMachine.position);

            return dest;
        }
    }
}

#endif