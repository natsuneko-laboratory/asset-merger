﻿/*-------------------------------------------------------------------------------------------
 * Copyright (c) Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

using Mochizuki.VRChat.AssetMerger.Internal;

using UnityEditor;
using UnityEditor.Animations;

using UnityEngine;

using VRC.SDK3.Avatars.ScriptableObjects;

#pragma warning disable 649

namespace Mochizuki.VRChat.AssetMerger
{
    public class AssetMergeEditor : EditorWindow
    {
        private const string Product = "VRChat Asset Merger";
        private const string Version = "0.4.0";
        private static readonly VersionManager Manager;
        private readonly GUIContent[] _tabItems;

        private AssetType _asset;

        [SerializeField]
        private AnimatorController[] _sourceControllers;

        [SerializeField]
        private VRCExpressionsMenu[] _sourceExpressions;

        [SerializeField]
        private VRCExpressionParameters[] _sourceParameters;

        static AssetMergeEditor()
        {
            Manager = new VersionManager("mika-f/VRChat-AssetMerger", Version, new Regex("v(?<version>.*)"));
        }

        public AssetMergeEditor()
        {
            _tabItems = Enum.GetNames(typeof(AssetType))
                            .Select(w => Regex.Replace(w, "(\\B[A-Z])", " $1"))
                            .Select(w => new GUIContent(w))
                            .ToArray();
        }

        [InitializeOnLoadMethod]
        public static void InitializeOnLoad()
        {
            Manager.CheckNewVersion();
        }

        [MenuItem("Mochizuki/VRChat/Asset Merger/Documents")]
        public static void ShowDocuments()
        {
            Process.Start("https://docs.mochizuki.moe/vrchat/asset-merger/");
        }

        [MenuItem("Mochizuki/VRChat/Asset Merger/Editor")]
        public static void ShowWindow()
        {
            var window = GetWindow<AssetMergeEditor>();
            window.titleContent = new GUIContent("Asset Merge Editor");

            window.Show();
        }

        // ReSharper disable once UnusedMember.Local
        private void OnGUI()
        {
            EditorStyles.label.wordWrap = true;
            EditorGUILayout.Space();
            EditorGUILayout.LabelField($"{Product} - {Version}");
            EditorGUILayout.Space();

            using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                EditorGUILayout.LabelField("自分で設定したものや、各ツールで自動生成された VRChat の Animator Controller, Expression Parameters および Expressions Menu を統合することが出来る Editor 拡張です。");

            using (new EditorGUILayout.HorizontalScope())
                _asset = (AssetType) GUILayout.Toolbar((int) _asset, _tabItems);

            switch (_asset)
            {
                case AssetType.AnimatorController:
                    OnGUIForAnimatorController();
                    break;

                case AssetType.ExpressionParameters:
                    OnGUIForExpressionParameters();
                    break;

                case AssetType.ExpressionsMenu:
                    OnGUIForExpressionsMenu();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (Manager.HasNewVersion)
                using (new EditorGUILayout.VerticalScope(GUI.skin.box))
                {
                    EditorGUILayout.LabelField($"{Product} の新しいバージョンがリリースされています。");
                    if (GUILayout.Button("BOOTH からダウンロード"))
                        Process.Start("https://natsuneko.booth.pm/items/2281798");
                }
        }

        private void OnGUIForAnimatorController()
        {
            using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                EditorGUILayout.LabelField("複数の Animator Controller をまとめます。");

            PropertyField(this, nameof(_sourceControllers));

            using (new EditorGUI.DisabledGroupScope(_sourceControllers?.All(w => w == null) == true))
            {
                if (GUILayout.Button("マージする"))
                    MergeAnimatorControllers(_sourceControllers);
            }
        }

        private void OnGUIForExpressionParameters()
        {
            using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
            {
                EditorGUILayout.LabelField(@"
複数の Expression Parameters をまとめます。
なお、パラメータの数が総コスト数を越えた場合は処理を中断します。
※VRChat のデフォルトパラメータは統合後も初期のもの3つのみ保持します。
※処理を中断した場合、エラー内容は Console に出力されます。
".Trim());
            }

            PropertyField(this, nameof(_sourceParameters));

            using (new EditorGUI.DisabledGroupScope(_sourceParameters?.All(w => w == null) == true))
            {
                if (GUILayout.Button("マージする"))
                    MergeExpressionParameters(_sourceParameters);
            }
        }

        private void OnGUIForExpressionsMenu()
        {
            using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
            {
                EditorGUILayout.LabelField(@"
複数の Expressions Menu をまとめます。
なお、ルートメニューの数が合計8個を越える場合は処理を中断します。
※処理を中断した場合、エラー内容は Console に出力されます。
".Trim());
            }

            PropertyField(this, nameof(_sourceExpressions));

            using (new EditorGUI.DisabledGroupScope(_sourceExpressions?.All(w => w == null) == true))
            {
                if (GUILayout.Button("マージする"))
                    MergeExpressionsMenus(_sourceExpressions);
            }
        }

        private static void MergeAnimatorControllers(AnimatorController[] controllers)
        {
            var dest = EditorUtility.SaveFilePanelInProject("Save merged animator controller to...", "MergedAnimatorController", "controller", "");
            var controller = new AnimatorController();
            AssetDatabase.CreateAsset(controller, dest);

            controller.MergeControllers(controllers);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void MergeExpressionParameters(VRCExpressionParameters[] parameters)
        {
            var dest = EditorUtility.SaveFilePanelInProject("Save merged expression parameters to...", "MergedVRCExpressionParameters", "asset", "");
            var parameter = CreateInstance<VRCExpressionParameters>();
            parameter.InitExpressionParameters(true);
            parameter.MergeParameters(parameters);

            AssetDatabase.CreateAsset(parameter, dest);
        }

        private static void MergeExpressionsMenus(VRCExpressionsMenu[] expressions)
        {
            var dest = EditorUtility.SaveFilePanelInProject("Save merged expressions menu to...", "MergedVRCExpressionsMenu", "asset", "");
            var expr = CreateInstance<VRCExpressionsMenu>();
            expr.MergeExpressions(expressions);

            AssetDatabase.CreateAsset(expr, dest);
        }

        private static void PropertyField(EditorWindow editor, string property)
        {
            var so = new SerializedObject(editor);
            so.Update();

            EditorGUILayout.PropertyField(so.FindProperty(property), true);

            so.ApplyModifiedProperties();
        }

        private enum AssetType
        {
            AnimatorController,

            ExpressionParameters,

            ExpressionsMenu
        }
    }
}