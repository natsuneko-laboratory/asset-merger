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
    public static class VRCAnimatorTrackingControlExtensions
    {
        public static void CloneTo(this VRCAnimatorTrackingControl source, VRCAnimatorTrackingControl dest)
        {
            dest.ApplySettings = source.ApplySettings;
            dest.trackingEyes = source.trackingEyes;
            dest.trackingHead = source.trackingHead;
            dest.trackingHip = source.trackingHip;
            dest.trackingLeftFingers = source.trackingLeftFingers;
            dest.trackingLeftFoot = source.trackingLeftFoot;
            dest.trackingLeftHand = source.trackingLeftHand;
            dest.trackingMouth = source.trackingMouth;
            dest.trackingRightFingers = source.trackingRightFingers;
            dest.trackingRightFoot = source.trackingRightFoot;
            dest.trackingRightHand = source.trackingRightHand;
            dest.debugString = source.debugString;
            dest.hideFlags = source.hideFlags;
            dest.name = source.name;
        }
    }
}

#endif
#endif