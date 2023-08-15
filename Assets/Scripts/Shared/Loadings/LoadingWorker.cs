using Cysharp.Threading.Tasks;
using DG.Tweening;
using Loadings.Visuals;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Utils.Unity;

namespace Loadings
{
    public interface ILoadingWorker
    {
        void AddJob(LoadJob job);
        void AddSceneLoadJob(SceneName scene);
        void AddSceneUnloadJob(SceneName scene, bool unloadUnusedAssets = false);
        void StartLoading(LoadingStyle style, Action onDone = null);
    }

    public class LoadJob
    {
        public string JobDescription;
        public Func<UniTask?> Job;
        public Action DoneCallback;
    }

    public sealed class LoadingWorker : MonoBehaviour, ILoadingWorker
    {
        public static ILoadingWorker Instance { get; private set; } = null;
        public Action OnDoneCallback { get; set; }

        private readonly Queue<LoadJob> _Jobs = new();

        [ChildTypeEnumValue((int)LoadingStyles.BlackShutter)]
        [SuppressMessage("Style", "IDE0044:Add readonly modifier")]
        private ILoadingVisual _ShutterVisual;



        private bool _LoadingInProgress = false;

        void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }

            Instance = this;
            transform.SetParent(null);

            var results = ChildTypeComponents<LoadingStyles>
                .FindAndMatchTo<ILoadingVisual>(this, x => x.Type);

            foreach (var result in results)
            {
                result.Setup();
            }
        }

        public void AddJob(LoadJob job)
        {
            if (job == null)
                return;

            if (job.Job == null)
                return;

            _Jobs.Enqueue(job);
        }

        public void AddSceneLoadJob(SceneName scene)
        {
            AddJob(new LoadJob()
            {
                JobDescription = "Loading Scenes...",
                Job = () =>
                {
                    return Scenes.Load(scene).ToUniTask();
                }
            });
        }

        public void AddSceneUnloadJob(SceneName scene, bool unloadUnusedAssets = false)
        {
            AddJob(new LoadJob()
            {
                JobDescription = "Loading Scenes...",
                Job = () =>
                {
                    return Scenes.Unload(scene).ToUniTask();
                }
            });
            if (unloadUnusedAssets)
            {
                AddJob(new LoadJob()
                {
                    JobDescription = "Unloading Unused Assets...",
                    Job = () =>
                    {
                        return Resources.UnloadUnusedAssets().ToUniTask();
                    }
                });
            }
        }

        public void StartLoading(LoadingStyle style, Action onDone = null)
        {
            if (_Jobs.Count <= 0)
            {
                onDone?.Invoke();
                return;
            }

            if (!_LoadingInProgress)
            {
                StartCoroutine(LoadingJob(style, onDone));
            }
        }

        IEnumerator LoadingJob(LoadingStyle style, Action onDone = null)
        {
            var visual = style.Style switch
            {
                LoadingStyles.BlackShutter => _ShutterVisual,
                _ => throw new NotImplementedException()
            };

            _LoadingInProgress = true;
            visual.gameObject.SetActive(true);
            visual.HideScreen(animation: false);
            yield return visual.ShowScreen(animation: true);

            while (_Jobs.TryDequeue(out var job))
            {
                var operation = job.Job.Invoke();
                if (!operation.HasValue)
                    continue;

                visual.SetTaskText(job.JobDescription);
                var awaiter = operation.Value.GetAwaiter();
                while (!awaiter.IsCompleted)
                {
                    //TODO: Reimplement Loading System Tbh
                    visual.SetTaskProgress(0.0f);
                    yield return null;
                }
                visual.SetTaskProgress(1.0f);
                yield return new WaitForSeconds(0.1f);
            }

            onDone?.Invoke();
            if (style.HideScreenOnFinished)
            {
                yield return visual.HideScreen(animation: true);
                visual.gameObject.SetActive(false);
            }
            _LoadingInProgress = false;
        }
    }
}
