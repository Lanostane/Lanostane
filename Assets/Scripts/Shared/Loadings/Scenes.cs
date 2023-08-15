using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Loadings
{
    public enum SceneName
    {
        Main,
        GamePlay
    }

    public struct SceneInfo
    {
        public SceneName Type;
        public string SceneToLoad;
        public LoadSceneMode LoadMode;

        public readonly bool HasValidMode => LoadMode == LoadSceneMode.Single || LoadMode == LoadSceneMode.Additive;
    }

    public static class Scenes
    {
        private static readonly Dictionary<SceneName, SceneInfo> s_Lookup = new();

        static Scenes()
        {
            s_Lookup.Add(SceneName.Main, new()
            {
                Type = SceneName.Main,
                SceneToLoad = "_Main",
                LoadMode = LoadSceneMode.Single
            });

            s_Lookup.Add(SceneName.GamePlay, new()
            {
                Type = SceneName.GamePlay,
                SceneToLoad = "GamePlay",
                LoadMode = LoadSceneMode.Additive
            });
        }

        public static bool IsLoaded(SceneName scene)
        {
            if (!s_Lookup.TryGetValue(scene, out var info))
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
            if (!s_Lookup.TryGetValue(scene, out var info))
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
            if (!s_Lookup.TryGetValue(scene, out var info))
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
