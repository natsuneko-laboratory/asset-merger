/*-------------------------------------------------------------------------------------------
 * Copyright (c) Fuyuno Mikazuki / Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

#if UNITY_EDITOR

using System.Text.RegularExpressions;

using UnityEngine;
using UnityEngine.Networking;

using Version = System.Version;

namespace Mochizuki.VRChat.Extensions.Convenience
{
    /// <summary>
    ///     Check the new version of your package, required GitHub public repository for managing version.
    /// </summary>
    public class VersionManager
    {
        private readonly string _currentVersion;
        private readonly Regex _format;
        private readonly string _repository;
        private string _latestVersion;
        private UnityWebRequest _www;

        public string LatestVersion
        {
            get
            {
                if (!_www.isDone)
                    return null;

                if (!string.IsNullOrWhiteSpace(_latestVersion))
                    return _latestVersion;

                if (_www.isNetworkError || _www.isHttpError)
                {
                    _latestVersion = "0.0.0";
                    return _latestVersion;
                }

                var json = JsonUtility.FromJson<GitHubApiResponse>(_www.downloadHandler.text);
                _latestVersion = json.tag_name;

                if (_format != null)
                    _latestVersion = _format.Match(_latestVersion).Groups["version"].Value;

                Debug.Log($"[Mochizuki.Extensions] Latest version of {_repository} is {_latestVersion}.");
                return _latestVersion;
            }
        }

        public bool HasNewVersion => new Version(_currentVersion).CompareTo(new Version(LatestVersion ?? "0.0.0")) < 0;

        public VersionManager(string repository, string currentVersion, Regex format = null)
        {
            _repository = repository;
            _currentVersion = currentVersion;
            _format = format;
        }

        public void CheckNewVersion()
        {
            Debug.Log($"[Mochizuki.Extensions] Checking new version for {_repository}...");
            _www = UnityWebRequest.Get($"https://api.github.com/repos/{_repository}/releases/latest");
            _www.SendWebRequest();
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class GitHubApiResponse
        {
            // ReSharper disable once InconsistentNaming
#pragma warning disable 649
            public string tag_name;
#pragma warning restore 649
        }
    }
}

#endif