/*-------------------------------------------------------------------------------------------
 * Copyright (c) Fuyuno Mikazuki / Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;

using Mochizuki.VRChat.Extensions.System;

using UnityEditor;
using UnityEditor.Animations;

using UnityEngine;

namespace Mochizuki.VRChat.Extensions.Unity
{
    public static class AnimatorControllerExtensions
    {
        public static AnimatorControllerLayer GetLayer(this AnimatorController obj, string name)
        {
            return obj.layers.FirstOrDefault(w => w.name == name);
        }

        public static void SetLayer(this AnimatorController obj, string name, AnimatorControllerLayer layer)
        {
            var layers = obj.layers;
            var (value, index) = layers.Select((w, i) => (Value: w, Index: i)).FirstOrDefault(w => w.Value.name == name);
            if (value == null)
                throw new InvalidOperationException();
            layers[index] = layer;

            obj.layers = layers;
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

            var copiedLayers = new List<AnimatorControllerLayer>();

            foreach (var controller in controllers)
            {
                // deep copy parameters
                foreach (var parameter in controller.parameters)
                    if (!source.HasParameter(parameter.name))
                        source.AddParameter(parameter.name, parameter.type);

                // deep copy layers
                if (controller.layers.Length == 0)
                    return;

                var layers = controller.layers;
                layers.First().defaultWeight = 1.0f; // set a default weight

                foreach (var layer in layers)
                    copiedLayers.Add(layer.CloneDeep());
            }

            if (copiedLayers.Select(w => w.name).Distinct().Count() != copiedLayers.Count)
                foreach (var duplicate in copiedLayers.Duplicate(w => w.name))
                {
                    var count = 1;
                    var indexes = copiedLayers.Select((w, i) => (Value: w, Index: i)).Where(w => w.Value.name == duplicate).ToList();
                    foreach (var (layer, _) in indexes)
                        layer.name = $"{layer.name} - {count++}";
                }

            source.layers = copiedLayers.ToArray();

            // add instances to controller
            foreach (var stateMachine in copiedLayers.Select(w => w.stateMachine).Where(w => w != null))
                AssetDatabase.AddObjectToAsset(stateMachine, source);

            foreach (var state in InstanceCaches<AnimatorState>.Caches)
                AssetDatabase.AddObjectToAsset(state, source);

            foreach (var stateMachine in InstanceCaches<AnimatorStateMachine>.Caches)
                AssetDatabase.AddObjectToAsset(stateMachine, source);

            foreach (var transition in InstanceCaches<AnimatorStateTransition>.Caches)
                AssetDatabase.AddObjectToAsset(transition, source);

            foreach (var behaviour in InstanceCaches<StateMachineBehaviour>.Caches)
                AssetDatabase.AddObjectToAsset(behaviour, source);

            // Cleanup All Caches
            InstanceCaches<AnimatorState>.Clear();
            InstanceCaches<AnimatorStateMachine>.Clear();
            InstanceCaches<AnimatorStateTransition>.Clear();
            InstanceCaches<StateMachineBehaviour>.Clear();
        }
    }
}

#endif