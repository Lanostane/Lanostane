using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Loadings
{
    public sealed class LoadSceneJob : ILoadJob
    {
        public SceneName LoadingScene;

        public LoadSceneJob(SceneName loadingScene)
        {
            LoadingScene = loadingScene;
        }

        public async UniTask Job(IProgress<JobProgress> progressHandle)
        {
            var newProgress = JobProgress.CreateNew("Loading Scene");

            var progress = Progress.Create<float>((p) =>
            {
                //0.9 means completed in Unity's Awesome API Design
                newProgress.Progress = Mathf.InverseLerp(0.0f, 0.9f, p);
                progressHandle.Report(newProgress);
            });

            await Scenes.Load(LoadingScene).ToUniTask(progress);
            progressHandle.Report(JobProgress.CreateDone("Scene Loaded"));
        }
    }

    public sealed class UnloadSceneJob : ILoadJob
    {
        public bool UnloadUnusedAssets = true;
        public bool CallGC = true;
        public SceneName UnloadingScene;

        public UnloadSceneJob(SceneName unloadingScene, bool unloadUnusedAssets = true, bool callGC = true)
        {
            UnloadingScene = unloadingScene;
            UnloadUnusedAssets = unloadUnusedAssets;
            CallGC = callGC;
        }

        public async UniTask Job(IProgress<JobProgress> progressHandle)
        {
            var newProgress = JobProgress.CreateNew("Unloading Scene");

            var progress = Progress.Create<float>((p) =>
            {
                //0.9 means completed in Unity's Awesome API Design
                newProgress.Progress = Mathf.InverseLerp(0.0f, 0.9f, p);
                progressHandle.Report(newProgress);
            });

            await Scenes.Unload(UnloadingScene).ToUniTask(progress);

            if (UnloadUnusedAssets)
            {
                newProgress.BatchName = "Unloading Unused Assets";
                progressHandle.Report(newProgress);
                await Resources.UnloadUnusedAssets().ToUniTask(progress);
            }

            if (CallGC)
            {
                GC.Collect();
            }

            progressHandle.Report(JobProgress.CreateDone("Scene Unloaded"));
        }
    }
}
