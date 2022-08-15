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
        void Show();
        void Hide();
        void HideAfter(AsyncOperation awaiter, Action callback = null);
    }

    public class LoadingScreen : MonoBehaviour, ILoadingScreen
    {
        public RectTransform MainRect;
        public TextMeshProUGUI Text;
        public Image Background;

        private bool _IsVisible = false;

        public void Show()
        {
            if (_IsVisible)
                return;

            gameObject.SetActive(true);
            transform.localScale = new Vector3(1.0f, 0.0f, 1.0f);
            transform.DOScaleY(1.0f, 1.2f).SetEase(Ease.OutCirc);
            _IsVisible = true;
        }

        public void SetText(string text)
        {
            Text.text = text;
        }

        public void Hide()
        {
            if (!_IsVisible)
                return;

            StartCoroutine(DoHide());
            _IsVisible = false;
        }

        public void HideAfter(AsyncOperation awaiter, Action callback = null)
        {
            if (!_IsVisible)
                return;

            StartCoroutine(WaitForDoneAndHide(awaiter, callback));
        }

        IEnumerator DoHide()
        {
            yield return transform.DOScaleY(0.0f, 1.2f).SetEase(Ease.OutCirc).WaitForCompletion();
            gameObject.SetActive(false);
        }

        IEnumerator WaitForDoneAndHide(AsyncOperation operation, Action callback = null)
        {
            yield return operation;
            Hide();
            callback?.Invoke();
        }
    }
}