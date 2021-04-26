/*-------------------------------------------------------------------------------------------
 * Copyright (c) Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;

using UnityEditor;
using UnityEditor.Animations;

using UnityEngine;

namespace Mochizuki.VRChat.AssetMerger.Internal
{
    internal static class AnimatorControllerExtensions
    {
        public static AnimatorControllerLayer GetLayer(this AnimatorController controller, string name)
        {
            return controller.layers.FirstOrDefault(w => w.name == name);
        }

        public static void SetLayer(this AnimatorController controller, string name, AnimatorControllerLayer layer)
        {
            var layers = controller.layers;
            var (value, index) = layers.Select((w, i) => (Value: w, Index: i)).FirstOrDefault(w => w.Value.name == name);
            if (value == null)
                return;

            layers[index] = layer;

            controller.layers = layers;
        }

        public static bool HasLayer(this AnimatorController obj, string name)
        {
            return obj.layers.Any(w => w.name == name);
        }

        public static bool HasParameter(this AnimatorController obj, string name)
        {
            return obj.parameters.Any(w => w.name == name);
        }

        public static void MergeControllers(this AnimatorController source, params AnimatorController[] controllers)
        {
            if (string.IsNullOrWhiteSpace(AssetDatabase.GetAssetPath(source)))
                throw new InvalidOperationException($"{source} must be persistent at the filesystem");

            var destLayers = new List<AnimatorControllerLayer>();

            foreach (var controller in controllers)
            {
                foreach (var parameter in controller.parameters)
                {
                    if (source.HasParameter(parameter.name))
                        continue;
                    source.AddParameter(parameter.name, parameter.type);
                }

                if (controller.layers.Length == 0)
                    continue;

                var layers = controller.layers;
                layers.First().defaultWeight = 1.0f;

                destLayers.AddRange(layers.Select(layer => layer.CloneDeep()));
            }

            if (destLayers.Select(w => w.name).Distinct().Count() != destLayers.Count)
                foreach (var duplicate in destLayers.Duplicate(w => w.name))
                {
                    var count = 1;
                    var index = destLayers.Select((w, i) => (Value: w, Index: w)).Where(w => w.Value.name == duplicate).ToList();
                    foreach (var (layer, i) in index)
                        layer.name = $"{layer.name} - {count++}";
                }

            source.layers = destLayers.ToArray();

            // add instances to controller
            foreach (var stateMachine in destLayers.Select(w => w.stateMachine).Where(w => w != null))
                AssetDatabase.AddObjectToAsset(stateMachine, source);

            foreach (var state in InstanceCache<AnimatorState>.Caches)
                AssetDatabase.AddObjectToAsset(state, source);

            foreach (var stateMachine in InstanceCache<AnimatorStateMachine>.Caches)
                AssetDatabase.AddObjectToAsset(stateMachine, source);

            foreach (var transition in InstanceCache<AnimatorStateTransition>.Caches)
                AssetDatabase.AddObjectToAsset(transition, source);

            foreach (var behaviour in InstanceCache<StateMachineBehaviour>.Caches)
                AssetDatabase.AddObjectToAsset(behaviour, source);

            // Cleanup All Caches
            InstanceCache<AnimatorState>.Clear();
            InstanceCache<AnimatorStateMachine>.Clear();
            InstanceCache<AnimatorStateTransition>.Clear();
            InstanceCache<StateMachineBehaviour>.Clear();
        }
    }
}