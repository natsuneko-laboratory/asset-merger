/*-------------------------------------------------------------------------------------------
 * Copyright (c) Fuyuno Mikazuki / Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

#if UNITY_EDITOR
#if VRC_SDK_VRCSDK3

using System;
using System.Linq;

using UnityEngine;

using VRC.SDK3.Avatars.ScriptableObjects;

namespace Mochizuki.VRChat.Extensions.VRC
{
    // ReSharper disable once InconsistentNaming
    public static class VRCExpressionParametersExtensions
    {
        private static readonly string[] DefaultParameterNames = { "VRCEmote", "VRCFaceBlendH", "VRCFaceBlendV" };

        public static void InitExpressionParameters(this VRCExpressionParameters obj)
        {
            // Simulate `VRCExpressionParametersEditor#InitExpressionParameters(bool)`
            obj.parameters = new VRCExpressionParameters.Parameter[VRCExpressionParameters.MAX_PARAMETERS];

            for (var i = 0; i < VRCExpressionParameters.MAX_PARAMETERS; i++)
                obj.parameters[i] = new VRCExpressionParameters.Parameter
                {
                    name = "",
                    valueType = VRCExpressionParameters.ValueType.Int
                };

            obj.parameters[0].name = DefaultParameterNames[0];
            obj.parameters[0].valueType = VRCExpressionParameters.ValueType.Int;

            obj.parameters[1].name = DefaultParameterNames[1];
            obj.parameters[1].valueType = VRCExpressionParameters.ValueType.Float;

            obj.parameters[2].name = DefaultParameterNames[2];
            obj.parameters[2].valueType = VRCExpressionParameters.ValueType.Float;
        }

        public static void AddParametersToFirstEmptySpace(this VRCExpressionParameters obj, string name, VRCExpressionParameters.ValueType type)
        {
            var first = obj.GetFirstEmptyParameter();
            if (first == null)
                throw new ArgumentOutOfRangeException($"This exceeds the maximum number that can be set in {nameof(VRCExpressionParameters)}");

            first.name = name;
            first.valueType = type;
        }

        public static void AddParametersToLastEmptySpace(this VRCExpressionParameters obj, string name, VRCExpressionParameters.ValueType type)
        {
            var last = obj.GetLastEmptyParameter();
            if (last == null)
                throw new ArgumentOutOfRangeException($"This exceeds the maximum number that can be set in {nameof(VRCExpressionParameters)}");

            last.name = name;
            last.valueType = type;
        }

        public static VRCExpressionParameters.Parameter GetFirstEmptyParameter(this VRCExpressionParameters obj)
        {
            return obj.parameters.FirstOrDefault(w => string.IsNullOrWhiteSpace(w.name));
        }

        public static VRCExpressionParameters.Parameter GetLastEmptyParameter(this VRCExpressionParameters obj)
        {
            return obj.parameters.LastOrDefault(w => string.IsNullOrWhiteSpace(w.name));
        }

        public static bool HasParameter(this VRCExpressionParameters obj, string name)
        {
            return obj.FindParameter(name) != null;
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
                {
                    Debug.LogWarning($"Parameter {parameter.name} is already registered");
                    continue;
                }

                source.AddParametersToFirstEmptySpace(parameter.name, parameter.valueType);
            }
        }
    }
}

#endif
#endif