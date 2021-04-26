/*-------------------------------------------------------------------------------------------
 * Copyright (c) Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

using System.Linq;

using UnityEditor;

using UnityEngine;

using VRC.SDK3.Avatars.ScriptableObjects;

namespace Mochizuki.VRChat.AssetMerger.Internal
{
    // ReSharper disable once InconsistentNaming
    internal static class VRCExpressionParametersExtensions
    {
        private static readonly string[] DefaultParameterNames = { "VRCEmote", "VRCFaceBlendH", "VRCFaceBlendV" };

        public static void InitExpressionParameters(this VRCExpressionParameters parameters, bool populateWithDefault)
        {
            if (!populateWithDefault)
            {
                parameters.parameters = new VRCExpressionParameters.Parameter[0];
                return;
            }

            parameters.parameters = new VRCExpressionParameters.Parameter[3];

            parameters.parameters[0] = new VRCExpressionParameters.Parameter { name = "VRCEmote", valueType = VRCExpressionParameters.ValueType.Int };
            parameters.parameters[1] = new VRCExpressionParameters.Parameter { name = "VRCFaceBlendH", valueType = VRCExpressionParameters.ValueType.Float };
            parameters.parameters[2] = new VRCExpressionParameters.Parameter { name = "VRCFaceBlendV", valueType = VRCExpressionParameters.ValueType.Float };
        }

        public static bool HasParameter(this VRCExpressionParameters source, string name)
        {
            return source.FindParameter(name) != null;
        }

        public static void AddParameter(this VRCExpressionParameters source, VRCExpressionParameters.Parameter parameter)
        {
            var so = new SerializedObject(source);
            so.Update();

            var sourceParameters = so.FindProperty(nameof(source.parameters));

            var idx = sourceParameters.arraySize;
            sourceParameters.InsertArrayElementAtIndex(idx);

            var obj = sourceParameters.GetArrayElementAtIndex(idx);
            obj.FindPropertyRelative("name").stringValue = parameter.name;
            obj.FindPropertyRelative("valueType").intValue = (int) parameter.valueType;
            obj.FindPropertyRelative("defaultValue").floatValue = parameter.defaultValue;
            obj.FindPropertyRelative("saved").boolValue = parameter.saved;

            so.ApplyModifiedProperties();
        }

        public static void MergeParameters(this VRCExpressionParameters source, params VRCExpressionParameters[] parameters)
        {
            foreach (var parameter in parameters.SelectMany(w => w.parameters))
            {
                if (string.IsNullOrWhiteSpace(parameter.name))
                    continue;

                if (DefaultParameterNames.Contains(parameter.name))
                    continue;

                if (source.HasParameter(parameter.name))
                    continue;

                source.AddParameter(parameter);

                if (source.CalcTotalCost() > VRCExpressionParameters.MAX_PARAMETER_COST)
                    Debug.LogWarning($"parameter `{parameter.name}` is merged but total cost is greater than allowed size => ${source.CalcTotalCost()}");
            }
        }
    }
}