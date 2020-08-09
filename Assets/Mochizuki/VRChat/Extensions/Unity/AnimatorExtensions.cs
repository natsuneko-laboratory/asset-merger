/*-------------------------------------------------------------------------------------------
 * Copyright (c) Fuyuno Mikazuki / Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

#if UNITY_EDITOR

using UnityEngine;

namespace Mochizuki.VRChat.Extensions.Unity
{
    public static class AnimatorExtensions
    {
        public static bool IsHumanoid(this Animator obj)
        {
            return obj?.avatar.isHuman == true && obj.avatar.isValid;
        }
    }
}

#endif