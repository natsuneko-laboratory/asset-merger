/*-------------------------------------------------------------------------------------------
 * Copyright (c) Fuyuno Mikazuki / Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;

using Mochizuki.VariationPackager.Models.Interface;

using UnityEngine;

namespace Mochizuki.VariationPackager.Models.Unity
{
    [Serializable]
    public class PackageConfiguration : IPackageConfiguration
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

        #region BaseDir

        [SerializeField]
        private string _baseDir;

        public string BaseDir
        {
            get => _baseDir;
            set => _baseDir = value;
        }

        #endregion

        #region Includes

        [SerializeField]
        private List<string> _includes;

        public List<string> Includes
        {
            get => _includes;
            set => _includes = value;
        }

        #endregion

        #region Excludes

        [SerializeField]
        private List<string> _excludes;

        public List<string> Excludes
        {
            get => _excludes;
            set => _excludes = value;
        }

        #endregion
    }
}