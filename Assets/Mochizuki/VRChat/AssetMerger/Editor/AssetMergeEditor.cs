/*-------------------------------------------------------------------------------------------
 * Copyright (c) Fuyuno Mikazuki / Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Mochizuki.VRChat.Extensions.Unity;
using Mochizuki.VRChat.Extensions.VRC;

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
        private const string Version = "0.1.0";
        private readonly GUIContent[] _tabItems;

        private AssetType _asset;

        [SerializeField]
        private AnimatorController[] _sourceControllers;

        [SerializeField]
        private VRCExpressionsMenu[] _sourceExpressions;

        [SerializeField]
        private VRCExpressionParameters[] _sourceParameters;

        public AssetMergeEditor()
        {
            _tabItems = Enum.GetNames(typeof(AssetType))
                            .Select(w => Regex.Replace(w, "(\\B[A-Z])", " $1"))
                            .Select(w => new GUIContent(w))
                            .ToArray();
        }

        [MenuItem("Mochizuki/VRChat/Asset Merge Editor")]
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
        }

        private void OnGUIForAnimatorController()
        {
            using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                EditorGUILayout.LabelField("複数の Animator Controller をまとめます。");

            EditorGUILayoutExtensions.PropertyField(this, nameof(_sourceControllers));

            using (new DisabledGroup(_sourceControllers.All(w => w == null)))
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
なお、パラメータの数が合計16個を越えた場合は処理を中断します。
※VRChat のデフォルトパラメータは統合後も初期のもの3つのみ保持します。
※処理を中断した場合、エラー内容は Console に出力されます。
".Trim());
            }

            EditorGUILayoutExtensions.PropertyField(this, nameof(_sourceParameters));

            using (new DisabledGroup(_sourceParameters.All(w => w == null)))
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

            EditorGUILayoutExtensions.PropertyField(this, nameof(_sourceExpressions));

            using (new DisabledGroup(_sourceExpressions.All(w => w == null)))
            {
                if (GUILayout.Button("マージする"))
                    MergeExpressionsMenus(_sourceExpressions);
            }
        }

        private static void MergeAnimatorControllers(AnimatorController[] controllers)
        {
            var dest = EditorUtilityExtensions.GetSaveFilePath("Save merged animator controller to...", "MergedAnimatorController", "controller");
            var mergedController = new AnimatorController();
            var layers = new List<AnimatorControllerLayer>();

            foreach (var controller in controllers)
            {
                foreach (var parameter in controller.parameters)
                    if (!mergedController.HasParameter(parameter.name))
                        mergedController.AddParameter(parameter);

                layers.First().defaultWeight = 1.0f;
                layers.AddRange(controller.layers);
            }

            if (layers.Select(w => w.name).Distinct().Count() != layers.Count)
                foreach (var dup in layers.Duplicate(w => w.name))
                {
                    var count = 1;
                    var indexes = layers.Select((w, i) => (Value: w, Index: i)).Where(w => w.Value.name == dup).ToList();
                    foreach (var (layer, index) in indexes)
                    {
                        layer.name = $"{layer.name} - {count++}";
                        layers[index] = layer;
                    }
                }

            mergedController.layers = layers.ToArray();

            AssetDatabase.CreateAsset(mergedController, dest);
        }

        private static void MergeExpressionParameters(VRCExpressionParameters[] parameters)
        {
            var dest = EditorUtilityExtensions.GetSaveFilePath("Save merged expression parameters to...", "MergedVRCExpressionParameters", "asset");
            var mergedParameters = CreateInstance<VRCExpressionParameters>();
            mergedParameters.InitExpressionParameters();

            var defaultParams = mergedParameters.parameters.Where(w => !string.IsNullOrWhiteSpace(w.name)).Select(w => w.name).ToList();
            var mergeTargets = new List<VRCExpressionParameters.Parameter>();
            foreach (var parameter in parameters.SelectMany(w => w.parameters))
            {
                if (string.IsNullOrWhiteSpace(parameter.name))
                    continue;
                if (defaultParams.Contains(parameter.name))
                    continue;

                if (mergeTargets.Any(w => w.name == parameter.name))
                {
                    Debug.LogWarning("同じ名前のパラメータがすでに登録されています。");
                    continue;
                }

                mergeTargets.Add(parameter);
            }

            if (mergeTargets.Count > VRCExpressionParameters.MAX_PARAMETERS)
            {
                Debug.LogError($"パラメータの総数が最大数である{VRCExpressionParameters.MAX_PARAMETERS}個を越えています。");
                return;
            }

            foreach (var (value, index) in mergeTargets.Select((w, i) => (Value: w, Index: i)))
                mergedParameters.parameters[index + 3] = value;

            AssetDatabase.CreateAsset(mergedParameters, dest);
        }

        private static void MergeExpressionsMenus(VRCExpressionsMenu[] expressions)
        {
            var dest = EditorUtilityExtensions.GetSaveFilePath("Save merged expressions menu to...", "MergedVRCExpressionsMenu", "asset");
            var expr = CreateInstance<VRCExpressionsMenu>();

            foreach (var control in expressions.SelectMany(w => w.controls))
            {
                if (expr.controls.Any(w => w.name == control.name))
                {
                    Debug.LogWarning("同じ名前のメニューがすでに登録されています。");
                    continue;
                }

                expr.controls.Add(control);
            }

            if (expr.controls.Count > VRCExpressionsMenu.MAX_CONTROLS)
            {
                Debug.LogError($"メニューの総数が最大数である{VRCExpressionsMenu.MAX_CONTROLS}個を越えています。");
                return;
            }

            AssetDatabase.CreateAsset(expr, dest);
        }

        private enum AssetType
        {
            AnimatorController,

            ExpressionParameters,

            ExpressionsMenu
        }
    }

    internal static class EnumerableExtensions
    {
        public static IEnumerable<string> Duplicate<T>(this IEnumerable<T> obj, Func<T, string> groupByFunc)
        {
            return obj.GroupBy(groupByFunc).Where(w => w.Count() > 1).Select(w => w.Key).ToList();
        }
    }
}