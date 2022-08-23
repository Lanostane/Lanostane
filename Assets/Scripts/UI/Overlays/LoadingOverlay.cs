using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Overlays
{
    public interface ILoadingOverlay
    {
        void SetText(string text);
        void DoLoading(Func<AsyncOperation> awaiterReturn, Action onDone = null);
        void Show(Action callback = null);
        void Hide();
        void HideAfter(AsyncOperation awaiter, Action callback = null);
    }

    public class LoadingOverlay : BaseOverlay, ILoadingOverlay
    {
        public const float TransitionTime = 0.45f;

        public RectTransform MainRect;
        public TextMeshProUGUI Text;
        public Image Background;

        private bool _IsVisible = false;
        protected override bool AutoActiveObject => true;
        protected override bool AutoDeactiveObject => false;

        public void Show(Action callback = null)
        {
            if (_IsVisible)
                return;

            SetActive(true);
            StartCoroutine(Do());

            IEnumerator Do()
            {
                yield return transform
                    .DOScaleY(1.0f, TransitionTime)
                    .SetEase(Ease.OutCirc)
                    .WaitForCompletion();
                callback?.Invoke();
            }
        }

        public void SetText(string text)
        {
            Text.text = text;
        }

        public void Hide()
        {
            if (!_IsVisible)
                return;

            SetActive(false);
            StartCoroutine(Do());

            IEnumerator Do()
            {
                yield return transform
                    .DOScaleY(0.0f, TransitionTime)
                    .SetEase(Ease.OutCirc)
                    .WaitForCompletion();
                gameObject.SetActive(false);
            }
        }

        public void HideAfter(AsyncOperation awaiter, Action callback = null)
        {
            if (!_IsVisible)
                return;

            StartCoroutine(Do());

            IEnumerator Do()
            {
                yield return awaiter;
                Hide();
                callback?.Invoke();
            }
        }

        protected override void OnOverlayEnabled()
        {
            transform.localScale = new Vector3(1.0f, 0.0f, 1.0f);
            _IsVisible = true;
        }

        protected override void OnOverlayDisabled()
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            _IsVisible = false;
        }

        public void DoLoading(Func<AsyncOperation> awaiterReturn, Action onDone = null)
        {
            if (awaiterReturn == null)
            {
                Debug.LogError("DoLoading cannot be used without awaiter return");
                return;
            }

            Show(() =>
            {
                var waitfor = awaiterReturn.Invoke();
                HideAfter(waitfor, () =>
                {
                    onDone?.Invoke();
                });
            });
        }
    }
}