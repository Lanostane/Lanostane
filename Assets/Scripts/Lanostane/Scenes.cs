using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lanostane
{
    public enum SceneName
    {
        Bootup,
        MainUI,
        GamePlay
    }

    public struct SceneInfo
    {
        public SceneName Type;
        public string SceneToLoad;
        public LoadSceneMode LoadMode;

        public bool HasValidMode => LoadMode == LoadSceneMode.Single || LoadMode == LoadSceneMode.Additive;
    }

    public static class Scenes
    {
        private readonly static Dictionary<SceneName, SceneInfo> _Lookup = new();

        static Scenes()
        {
            _Lookup.Add(SceneName.Bootup, new()
            {
                Type = SceneName.Bootup,
                SceneToLoad = "_Bootup",
                LoadMode = LoadSceneMode.Single
            });

            _Lookup.Add(SceneName.MainUI, new()
            {
                Type = SceneName.MainUI,
                SceneToLoad = "MainUI",
                LoadMode = LoadSceneMode.Single
            });

            _Lookup.Add(SceneName.GamePlay, new()
            {
                Type = SceneName.GamePlay,
                SceneToLoad = "GamePlay",
                LoadMode = LoadSceneMode.Additive
            });
        }

        public static bool IsLoaded(SceneName scene)
        {
            if (!_Lookup.TryGetValue(scene, out var info))
            {
                Debug.LogError($"SceneName: {scene} is not definied or registered!");
                return false;
            }

            return IsLoaded(info);
        }

        public static bool IsLoaded(SceneInfo info)
        {
            var scene = SceneManager.GetSceneByName(info.SceneToLoad);
            return scene.IsValid() && scene.isLoaded;
        }

        public static AsyncOperation Load(SceneName scene)
        {
            if (!_Lookup.TryGetValue(scene, out var info))
            {
                Debug.LogError($"SceneName: {scene} is not definied or registered!");
                return null;
            }

            if (IsLoaded(info))
            {
                Debug.LogWarning($"Scene Load was called with loaded scene {scene}");
                return null;
            }

            if (!info.HasValidMode)
            {
                Debug.LogError($"SceneName: {scene} has invalid LoadMode [Mode: {info.LoadMode}]!");
                return null;
            }

            return SceneManager.LoadSceneAsync(info.SceneToLoad, info.LoadMode);
        }

        public static AsyncOperation Unload(SceneName scene)
        {
            if (!_Lookup.TryGetValue(scene, out var info))
            {
                Debug.LogError($"SceneName: {scene} is not definied or registered!");
                return null;
            }

            if (!IsLoaded(info))
            {
                Debug.LogWarning($"Scene Unload was called with not loaded scene {scene}");
                return null;
            }

            switch (info.LoadMode)
            {
                case LoadSceneMode.Additive:
                    return SceneManager.UnloadSceneAsync(info.SceneToLoad);

                default:
                    Debug.LogError($"SceneName: {scene} is not Addtivie Scene LoadMode [Mode: {info.LoadMode}]! Cannot be unloaded explicitely!");
                    return null;
            }
        }
    }
}
