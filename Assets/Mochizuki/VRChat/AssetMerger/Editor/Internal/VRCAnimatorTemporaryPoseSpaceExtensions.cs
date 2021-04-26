/*-------------------------------------------------------------------------------------------
 * Copyright (c) Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

using VRC.SDK3.Avatars.Components;

namespace Mochizuki.VRChat.AssetMerger.Internal
{
    // ReSharper disable once InconsistentNaming
    internal static class VRCAnimatorTemporaryPoseSpaceExtensions
    {
        public static void ApplyTo(this VRCAnimatorTemporaryPoseSpace source, VRCAnimatorTemporaryPoseSpace dest)
        {
            dest.ApplySettings = source.ApplySettings;
            dest.delayTime = source.delayTime;
            dest.enterPoseSpace = source.enterPoseSpace;
            dest.fixedDelay = source.fixedDelay;
            dest.debugString = source.debugString;
            dest.name = source.name;
            dest.hideFlags = source.hideFlags;
        }
    }
}