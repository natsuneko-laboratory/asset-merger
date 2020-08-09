/*-------------------------------------------------------------------------------------------
 * Copyright (c) Fuyuno Mikazuki / Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

#if UNITY_EDITOR

using UnityEditor;

using UnityEngine;

namespace Mochizuki.VRChat.Extensions.Unity
{
    // ReSharper disable once InconsistentNaming
    public static class EditorGUILayoutExtensions
    {
        public static void PropertyField(EditorWindow editor, string property)
        {
            var so = new SerializedObject(editor);
            so.Update();

            EditorGUILayout.PropertyField(so.FindProperty(property), true);

            so.ApplyModifiedProperties();
        }

        public static T ObjectPicker<T>(string label, T obj) where T : Object
        {
            return EditorGUILayout.ObjectField(new GUIContent(label), obj, typeof(T), true) as T;
        }

        public static T ReadonlyObjectPicker<T>(string label, T obj) where T : Object
        {
            using (new DisabledGroup())
                return ObjectPicker(label, obj);
        }
    }
}

#endif