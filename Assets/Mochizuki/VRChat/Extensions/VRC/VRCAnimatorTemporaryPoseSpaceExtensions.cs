/*-------------------------------------------------------------------------------------------
 * Copyright (c) Fuyuno Mikazuki / Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

#if UNITY_EDITOR
#if VRC_SDK_VRCSDK3

using VRC.SDK3.Avatars.Components;

namespace Mochizuki.VRChat.Extensions.VRC
{
    // ReSharper disable once InconsistentNaming
    public static class VRCAnimatorTemporaryPoseSpaceExtensions
    {
        public static void CloneTo(this VRCAnimatorTemporaryPoseSpace source, VRCAnimatorTemporaryPoseSpace dest)
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

#endif
#endif