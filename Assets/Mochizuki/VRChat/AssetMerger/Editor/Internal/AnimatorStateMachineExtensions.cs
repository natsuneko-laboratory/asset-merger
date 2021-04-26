/*-------------------------------------------------------------------------------------------
 * Copyright (c) Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

using System;

using UnityEditor.Animations;

using UnityEngine;

namespace Mochizuki.VRChat.AssetMerger.Internal
{
    internal static class AnimatorStateMachineExtensions
    {
        public static AnimatorStateMachine CloneDeep(this AnimatorStateMachine source)
        {
            var dest = new AnimatorStateMachine
            {
                defaultState = InstanceCache<AnimatorState>.FindOrCreate(source.defaultState, w => w.CloneDeep()),
                anyStatePosition = source.anyStatePosition,
                entryPosition = source.entryPosition,
                exitPosition = source.exitPosition,
                parentStateMachinePosition = source.parentStateMachinePosition,
                hideFlags = source.hideFlags,
                name = source.name
            };

            foreach (var sourceState in source.states)
                dest.AddState(InstanceCache<AnimatorState>.FindOrCreate(sourceState.state, w => w.CloneDeep()), sourceState.position);

            foreach (var sourceTransition in source.anyStateTransitions)
            {
                AnimatorStateTransition transition = null;
                if (sourceTransition.destinationStateMachine != null)
                    transition = dest.AddAnyStateTransition(InstanceCache<AnimatorStateMachine>.FindOrCreate(sourceTransition.destinationStateMachine, w => w.CloneDeep()));

                if (sourceTransition.destinationState != null)
                    transition = dest.AddAnyStateTransition(InstanceCache<AnimatorState>.FindOrCreate(sourceTransition.destinationState, w => w.CloneDeep()));

                if (transition == null)
                    throw new ArgumentNullException(nameof(transition));

                sourceTransition.ApplyTo(transition);

                InstanceCache<AnimatorStateTransition>.SafeRegister(sourceTransition.GetInstanceID(), transition);
            }

            foreach (var sourceTransition in source.entryTransitions)
            {
                AnimatorTransition transition = null;
                if (sourceTransition.destinationStateMachine != null)
                    transition = dest.AddEntryTransition(InstanceCache<AnimatorStateMachine>.FindOrCreate(sourceTransition.destinationStateMachine, w => w.CloneDeep()));

                if (sourceTransition.destinationState != null)
                    transition = dest.AddEntryTransition(InstanceCache<AnimatorState>.FindOrCreate(sourceTransition.destinationState, w => w.CloneDeep()));

                if (transition == null)
                    throw new ArgumentNullException(nameof(transition));

                sourceTransition.ApplyTo(transition);

                InstanceCache<AnimatorTransition>.SafeRegister(sourceTransition.GetInstanceID(), transition);
            }

            foreach (var sourceBehaviour in source.behaviours)
            {
                var behaviour = dest.AddStateMachineBehaviour(sourceBehaviour.GetType());
                sourceBehaviour.ApplyTo(behaviour);

                InstanceCache<StateMachineBehaviour>.SafeRegister(behaviour.GetInstanceID(), behaviour);
            }

            foreach (var sourceStateMachine in source.stateMachines)
                dest.AddStateMachine(InstanceCache<AnimatorStateMachine>.FindOrCreate(sourceStateMachine.stateMachine, w => w.CloneDeep()), sourceStateMachine.position);

            return dest;
        }
    }
}