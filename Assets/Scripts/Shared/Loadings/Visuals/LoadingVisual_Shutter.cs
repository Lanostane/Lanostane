using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Loadings.Visuals
{
    public sealed class LoadingVisual_Shutter : MonoBehaviour, ILoadingVisual
    {
        public Transform Shutter;
        public TextMeshProUGUI LoadingText;
        public TextMeshProUGUI ProgressText;

        public LoadingStyles Type => LoadingStyles.BlackShutter;

        public void Setup()
        {

        }

        public Coroutine HideScreen(bool animation = true)
        {
            if (animation)
            {
                return StartCoroutine(Hide());
            }
            else
            {
                LoadingText.gameObject.SetActive(false);
                ProgressText.gameObject.SetActive(false);
                Shutter.gameObject.SetActive(false);
                return null;
            }

            IEnumerator Hide()
            {
                LoadingText.gameObject.SetActive(false);
                ProgressText.gameObject.SetActive(false);
                Shutter.gameObject.SetActive(true);
                Shutter.localScale = new(1.0f, 1.0f, 1.0f);
                yield return Shutter
                        .DOScaleY(0.0f, 0.45f)
                        .SetEase(Ease.OutCirc)
                        .WaitForCompletion();
                Shutter.gameObject.SetActive(false);
            }
        }

        public Coroutine ShowScreen(bool animation = true)
        {
            if (animation)
            {
                return StartCoroutine(Show());
            }
            else
            {
                LoadingText.gameObject.SetActive(true);
                ProgressText.gameObject.SetActive(true);
                Shutter.gameObject.SetActive(true);
                return null;
            }

            IEnumerator Show()
            {
                Shutter.gameObject.SetActive(true);
                Shutter.localScale = new(1.0f, 0.0f, 1.0f);
                yield return Shutter
                        .DOScaleY(1.0f, 0.45f)
                        .SetEase(Ease.OutCirc)
                        .WaitForCompletion();
                LoadingText.gameObject.SetActive(true);
                ProgressText.gameObject.SetActive(true);
            }
        }

        public void SetMainProgress(float p)
        {
            
        }

        public void SetTaskProgress(float p)
        {
            if (p >= 1.0f)
            {
                ProgressText.text = "DONE";
            }
            else
            {
                ProgressText.text = $"{Mathf.RoundToInt(p * 100.0f)}%";
            }
        }

        public void SetMainText(string text)
        {
            
        }

        public void SetTaskText(string text)
        {
            LoadingText.text = text;
        }
    }
}
