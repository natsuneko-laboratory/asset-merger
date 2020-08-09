/*-------------------------------------------------------------------------------------------
 * Copyright (c) Fuyuno Mikazuki / Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;

using Mochizuki.VariationPackager.Models.Interface;

using UnityEngine;

namespace Mochizuki.VariationPackager.Models.Unity
{
    [Serializable]
    public class PackageDescribe : IPackageDescribe
    {
        #region Output

        [SerializeField]
        private string _output;

        public string Output
        {
            get => _output;
            set => _output = value;
        }

        #endregion

        #region Variations

        // ReSharper disable once CollectionNeverUpdated.Local
        // ReSharper disable once UnassignedField.Local
        [SerializeField]
        private List<PackageVariation> _variations;

        public List<IPackageVariation> Variations => _variations.Cast<IPackageVariation>().ToList();

        #endregion
    }
}