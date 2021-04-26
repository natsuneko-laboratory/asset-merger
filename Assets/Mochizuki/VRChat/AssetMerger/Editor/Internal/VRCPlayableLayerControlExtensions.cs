/*-------------------------------------------------------------------------------------------
 * Copyright (c) Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

using VRC.SDK3.Avatars.Components;

namespace Mochizuki.VRChat.AssetMerger.Internal
{
    // ReSharper disable once InconsistentNaming
    internal static class VRCPlayableLayerControlExtensions
    {
        public static void ApplyTo(this VRCPlayableLayerControl source, VRCPlayableLayerControl dest)
        {
            dest.layer = source.layer;
            dest.ApplySettings = source.ApplySettings;
            dest.goalWeight = source.goalWeight;
            dest.blendDuration = source.blendDuration;
            dest.debugString = source.debugString;
            dest.name = source.name;
            dest.hideFlags = source.hideFlags;
        }
    }
}