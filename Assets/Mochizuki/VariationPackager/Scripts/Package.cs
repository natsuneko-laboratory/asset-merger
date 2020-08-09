/*-------------------------------------------------------------------------------------------
 * Copyright (c) Fuyuno Mikazuki / Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;

using Mochizuki.VariationPackager.Models.Abstractions;
using Mochizuki.VariationPackager.Models.Interface;

using UnityEngine;

#pragma warning disable 649

namespace Mochizuki.VariationPackager.Models.Unity
{
    [AddComponentMenu("Scripts/Mochizuki/VariationPackager/Package")]
    [Serializable]
    public class Package : MonoBehaviour, IPackage
    {
        #region Name

        [SerializeField]
        private string _name;

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        #endregion

        #region Version

        [SerializeField]
        private string _version;

        public string Version
        {
            get => _version;
            set => _version = value;
        }

        #endregion

        #region Describe

        // ReSharper disable once UnassignedField.Local
        [SerializeField]
        private PackageDescribe _describe;

        public IPackageDescribe Describe => _describe;

        #endregion

        #region PreProcessors

        // ReSharper disable once UnassignedField.Local
        [SerializeField]
        private List<Processor> _preProcessors;

        public List<IProcessor> PreProcessors => _preProcessors.Cast<IProcessor>().ToList();

        #endregion

        #region PostProcessors

        // ReSharper disable once UnassignedField.Local
        [SerializeField]
        private List<Processor> _postProcessors;

        public List<IProcessor> PostProcessors => _postProcessors.Cast<IProcessor>().ToList();

        #endregion
    }
}