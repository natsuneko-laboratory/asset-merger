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
        /// <summary>
        ///     Show [SerializeFieldAttribute] property's inspector GUI on Editor Window.
        /// </summary>
        public static void PropertyField(EditorWindow editor, string property)
        {
            var so = new SerializedObject(editor);
            so.Update();

            EditorGUILayout.PropertyField(so.FindProperty(property), true);

            so.ApplyModifiedProperties();
        }

        /// <summary>
        ///     Generics Wrapper of EditorGUILayout#ObjectField
        /// </summary>
        public static T ObjectPicker<T>(string label, T obj) where T : Object
        {
            return EditorGUILayout.ObjectField(new GUIContent(label), obj, typeof(T), true) as T;
        }

        /// <summary>
        ///     Readonly version of ObjectPicker.
        /// </summary>
        public static T ReadonlyObjectPicker<T>(string label, T obj) where T : Object
        {
            using (new DisabledGroup())
                return ObjectPicker(label, obj);
        }
    }
}

#endif