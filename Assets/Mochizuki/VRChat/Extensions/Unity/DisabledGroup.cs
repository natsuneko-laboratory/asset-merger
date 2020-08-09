/*-------------------------------------------------------------------------------------------
 * Copyright (c) Fuyuno Mikazuki / Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

#if UNITY_EDITOR

using System;

using UnityEditor;

namespace Mochizuki.VRChat.Extensions.Unity
{
    public class DisabledGroup : IDisposable
    {
        public DisabledGroup() : this(true) { }

        public DisabledGroup(bool b)
        {
            EditorGUI.BeginDisabledGroup(b);
        }

        public void Dispose()
        {
            EditorGUI.EndDisabledGroup();
        }
    }
}

#endif