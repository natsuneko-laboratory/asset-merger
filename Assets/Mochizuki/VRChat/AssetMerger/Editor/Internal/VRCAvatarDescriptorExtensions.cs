/*-------------------------------------------------------------------------------------------
 * Copyright (c) Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

using System;
using System.Linq;

using UnityEditor.Animations;

using VRC.SDK3.Avatars.Components;

namespace Mochizuki.VRChat.AssetMerger.Internal
{
    // ReSharper disable once InconsistentNaming
    internal static class VRCAvatarDescriptorExtensions
    {
        public static void SetAnimationLayer(this VRCAvatarDescriptor avatar, VRCAvatarDescriptor.AnimLayerType layerType, AnimatorController animator)
        {
            if (!avatar.customizeAnimationLayers)
                avatar.customizeAnimationLayers = true;

            var layer = avatar.GetAnimationLayer(layerType);
            if (layer.isDefault)
                layer.isDefault = false;

            layer.animatorController = animator;
            avatar.SetAnimationLayer(layerType, layer);
        }

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

        public static bool HasAnimationLayer(this VRCAvatarDescriptor avatar, VRCAvatarDescriptor.AnimLayerType layer, bool isCheckEnabled = true)
        {
            if (!avatar.customizeAnimationLayers)
                return false;

            var anim = avatar.GetAnimationLayer(layer);
            return !anim.isDefault && (!isCheckEnabled || anim.isEnabled) && anim.animatorController != null;
        }
    }
}