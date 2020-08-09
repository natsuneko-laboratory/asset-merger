/*-------------------------------------------------------------------------------------------
 * Copyright (c) Fuyuno Mikazuki / Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

#if UNITY_EDITOR

using UnityEditor;

using UnityEngine;

namespace Mochizuki.VRChat.Extensions.Unity
{
    public static class AssetDatabaseExtensions
    {
        public static T CopyAndLoadAsset<T>(T src, string dest) where T : Object
        {
            AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(src), dest);
            return AssetDatabase.LoadAssetAtPath<T>(dest);
        }

        public static T LoadAssetFromGuid<T>(string guid) where T : Object
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            return AssetDatabase.LoadAssetAtPath<T>(path);
        }
    }
}

#endif