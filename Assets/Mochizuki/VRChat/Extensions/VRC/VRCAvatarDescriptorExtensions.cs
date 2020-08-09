/*-------------------------------------------------------------------------------------------
 * Copyright (c) Fuyuno Mikazuki / Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

#if UNITY_EDITOR
#if VRC_SDK_VRCSDK3

using System;
using System.Linq;

using UnityEditor.Animations;

using UnityEngine;

using VRC.SDK3.Avatars.Components;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Mochizuki.VRChat.Extensions.VRC
{
    // ReSharper disable once InconsistentNaming
    public static class VRCAvatarDescriptorExtensions
    {
        public static bool HasAnimator(this VRCAvatarDescriptor avatar)
        {
            return avatar.gameObject.GetComponent<Animator>() != null;
        }

        public static bool HasArmature(this VRCAvatarDescriptor avatar)
        {
            return avatar.gameObject.GetComponentsInChildren<Transform>().Any(w => w.name == "Armature");
        }

        public static void SetAnimationLayer(this VRCAvatarDescriptor avatar, VRCAvatarDescriptor.AnimLayerType layer, AnimatorController controller)
        {
            if (!avatar.customizeAnimationLayers)
                avatar.customizeAnimationLayers = true;

            var l = avatar.GetAnimationLayer(layer);
            if (l.isDefault)
                l.isDefault = false;

            l.animatorController = controller;
            avatar.SetAnimationLayer(layer, l);
        }

        public static void SetExpressions(this VRCAvatarDescriptor avatar, VRCExpressionsMenu expr, VRCExpressionParameters parameters)
        {
            if (!avatar.customExpressions)
                avatar.customExpressions = true;

            avatar.expressionsMenu = expr;
            avatar.expressionParameters = parameters;
        }

        #region Animation Layer

        public static VRCAvatarDescriptor.CustomAnimLayer GetAnimationLayer(this VRCAvatarDescriptor avatar, VRCAvatarDescriptor.AnimLayerType layer)
        {
            switch (layer)
            {
                case VRCAvatarDescriptor.AnimLayerType.Base:
                case VRCAvatarDescriptor.AnimLayerType.Additive:
                case VRCAvatarDescriptor.AnimLayerType.Gesture:
                case VRCAvatarDescriptor.AnimLayerType.Action:
                case VRCAvatarDescriptor.AnimLayerType.FX:
                    return avatar.baseAnimationLayers.First(w => w.type == layer);

                case VRCAvatarDescriptor.AnimLayerType.SpecialIK:
                case VRCAvatarDescriptor.AnimLayerType.Sitting:
                case VRCAvatarDescriptor.AnimLayerType.TPose:
                case VRCAvatarDescriptor.AnimLayerType.IKPose:
                    return avatar.specialAnimationLayers.First(w => w.type == layer);

                default:
                    throw new ArgumentOutOfRangeException(nameof(layer), layer, null);
            }
        }

        public static void SetAnimationLayer(this VRCAvatarDescriptor avatar, VRCAvatarDescriptor.AnimLayerType layer, VRCAvatarDescriptor.CustomAnimLayer animation)
        {
            switch (layer)
            {
                case VRCAvatarDescriptor.AnimLayerType.Base:
                case VRCAvatarDescriptor.AnimLayerType.Additive:
                case VRCAvatarDescriptor.AnimLayerType.Gesture:
                case VRCAvatarDescriptor.AnimLayerType.Action:
                case VRCAvatarDescriptor.AnimLayerType.FX:
                    var (_, bIndex) = avatar.baseAnimationLayers.Select((w, i) => (Value: w, Index: i)).First(w => w.Value.type == layer);
                    avatar.baseAnimationLayers[bIndex] = animation;
                    break;

                case VRCAvatarDescriptor.AnimLayerType.SpecialIK:
                case VRCAvatarDescriptor.AnimLayerType.Sitting:
                case VRCAvatarDescriptor.AnimLayerType.TPose:
                case VRCAvatarDescriptor.AnimLayerType.IKPose:
                    var (_, sIndex) = avatar.specialAnimationLayers.Select((w, i) => (Value: w, Index: i)).First(w => w.Value.type == layer);
                    avatar.specialAnimationLayers[sIndex] = animation;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(layer), layer, null);
            }
        }

        public static bool HasAnimationLayer(this VRCAvatarDescriptor avatar, VRCAvatarDescriptor.AnimLayerType layer)
        {
            if (!avatar.customizeAnimationLayers)
                return false;

            var anim = avatar.GetAnimationLayer(layer);
            return !anim.isDefault && anim.isEnabled && anim.animatorController != null;
        }

        #endregion
    }
}

#endif
#endif