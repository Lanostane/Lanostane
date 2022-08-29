using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Utils.Unity;

namespace Lst.Loading
{
    public interface ILoadingWorker
    {
        void Enqueue(LoadJob job);
        void DoLoading(LoadingStyle style, Action onDone = null);
    }

    public class LoadJob
    {
        public string JobDescription;
        public Func<AsyncOperation> Job;
        public Action DoneCallback;
    }

    public sealed class LoadingWorker : MonoBehaviour, ILoadingWorker
    {
        public static ILoadingWorker Instance { get; private set; } = null;
        private readonly Queue<LoadJob> _Jobs = new();

        [ChildTypeEnumValue((int)LoadingStyles.BlackShutter)]
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
            DontDestroyOnLoad(this);

            ChildTypeComponents<LoadingStyles>
                .FindAndMatchTo<ILoadingVisual>(transform, this, x => x.Type);
        }

        public void Enqueue(LoadJob job)
        {
            if (job == null)
                return;

            if (job.Job == null)
                return;

            _Jobs.Enqueue(job);
        }

        public void DoLoading(LoadingStyle style, Action onDone = null)
        {
            if (_Jobs.Count > 0 && !_LoadingInProgress)
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
                visual.SetTaskText(job.JobDescription);
                while (!operation.isDone)
                {
                    visual.SetTaskProgress(operation.progress);
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
