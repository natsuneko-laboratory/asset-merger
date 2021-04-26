/*-------------------------------------------------------------------------------------------
 * Copyright (c) Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

using VRC.SDK3.Avatars.Components;

namespace Mochizuki.VRChat.AssetMerger.Internal
{
    // ReSharper disable once InconsistentNaming
    internal static class VRCAnimatorLocomotionControlExtensions
    {
        public static void ApplyTo(this VRCAnimatorLocomotionControl source, VRCAnimatorLocomotionControl dest)
        {
            dest.ApplySettings = source.ApplySettings;
            dest.debugString = source.debugString;
            dest.disableLocomotion = source.disableLocomotion;
            dest.hideFlags = source.hideFlags;
            dest.name = source.name;
        }
    }
}