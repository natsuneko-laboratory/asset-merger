/*-------------------------------------------------------------------------------------------
 * Copyright (c) Fuyuno Mikazuki / Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

using System;

using Mochizuki.VariationPackager.Models.Interface;

using UnityEngine;

// ReSharper disable UnassignedField.Local

namespace Mochizuki.VariationPackager.Models.Unity
{
    [Serializable]
    public class PackageVariation : IPackageVariation
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

        #region Archive

        // Unity could not serialize interfaces, so we provide internal accessor.
        [SerializeField]
        private PackageConfiguration _archive;

        public IPackageConfiguration Archive => _archive;

        #endregion

        #region UnityPackage

        // Unity could not serialize interfaces, so we provide internal accessor.
        [SerializeField]
        private PackageConfiguration _unityPackage;

        public IPackageConfiguration UnityPackage => _unityPackage;

        #endregion
    }
}