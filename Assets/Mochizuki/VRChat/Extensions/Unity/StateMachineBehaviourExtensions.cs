/*-------------------------------------------------------------------------------------------
 * Copyright (c) Fuyuno Mikazuki / Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

#if UNITY_EDITOR

#if VRC_SDK_VRCSDK3

using System;

using VRC.SDK3.Avatars.Components;

using Mochizuki.VRChat.Extensions.VRC;

using UnityEngine;

#else
using System;

using UnityEngine;

#endif

// ReSharper disable TailRecursiveCall

namespace Mochizuki.VRChat.Extensions.Unity
{
    public static class StateMachineBehaviourExtensions
    {
        public static void CloneDeepTo(this StateMachineBehaviour source, StateMachineBehaviour dest)
        {
            if (source.GetType() != dest.GetType())
                throw new ArgumentException($"{nameof(source)} and {nameof(dest)} must be same type.");

            switch (source)
            {
#if VRC_SDK_VRCSDK3
                case VRCAnimatorLayerControl sourceAlc:
                {
                    var behaviour = dest as VRCAnimatorLayerControl;
                    sourceAlc.CloneTo(behaviour);
                    break;
                }

                case VRCAnimatorLocomotionControl sourceAlc:
                {
                    var behaviour = dest as VRCAnimatorLocomotionControl;
                    sourceAlc.CloneTo(behaviour);
                    break;
                }

                case VRCAnimatorTemporaryPoseSpace sourceTps:
                {
                    var behaviour = dest as VRCAnimatorTemporaryPoseSpace;
                    sourceTps.CloneTo(behaviour);
                    break;
                }

                case VRCAnimatorTrackingControl sourceTc:
                {
                    var behaviour = dest as VRCAnimatorTrackingControl;
                    sourceTc.CloneTo(behaviour);
                    break;
                }

                case VRCAvatarParameterDriver sourceApd:
                {
                    var behaviour = dest as VRCAvatarParameterDriver;
                    sourceApd.CloneTo(behaviour);
                    break;
                }

                case VRCPlayableLayerControl sourcePlc:
                {
                    var behaviour = dest as VRCPlayableLayerControl;
                    sourcePlc.CloneTo(behaviour);
                    break;
                }
#endif
            }
        }
    }
}

#endif