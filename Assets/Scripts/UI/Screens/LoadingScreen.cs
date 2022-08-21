using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens
{
    public interface ILoadingScreen
    {
        void SetText(string text);
        void Show(Action callback = null);
        void Hide();
        void HideAfter(AsyncOperation awaiter, Action callback = null);
    }

    public class LoadingScreen : MonoBehaviour, ILoadingScreen
    {
        public RectTransform MainRect;
        public TextMeshProUGUI Text;
        public Image Background;

        private bool _IsVisible = false;

        public void Show(Action callback = null)
        {
            if (_IsVisible)
                return;

            gameObject.SetActive(true);
            transform.localScale = new Vector3(1.0f, 0.0f, 1.0f);
            _IsVisible = true;
            StartCoroutine(Do());

            IEnumerator Do()
            {
                yield return transform.DOScaleY(1.0f, 1.2f).SetEase(Ease.OutCirc);
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

            StartCoroutine(Do());
            _IsVisible = false;

            IEnumerator Do()
            {
                yield return transform.DOScaleY(0.0f, 1.2f).SetEase(Ease.OutCirc).WaitForCompletion();
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
    }
}