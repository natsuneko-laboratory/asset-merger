/*-------------------------------------------------------------------------------------------
 * Copyright (c) Fuyuno Mikazuki / Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

#if UNITY_EDITOR

using System;

using UnityEditor;

namespace Mochizuki.VRChat.Extensions.Unity
{
    public class IncreaseIndent : IDisposable
    {
        public IncreaseIndent()
        {
            EditorGUI.indentLevel++;
        }

        public void Dispose()
        {
            EditorGUI.indentLevel--;
        }
    }
}

#endif