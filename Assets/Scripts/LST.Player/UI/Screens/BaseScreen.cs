using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LST.Player.UI
{
    public enum ScreenType
    {
        Intro,
        MainMenu,
        Game,
        Overlay
    }

    public interface IScreen
    {
        ScreenType Type { get; }
        void SetActive(bool wantToActive);
    }

    [RequireComponent(typeof(CanvasGroup))]
    public abstract class BaseScreen : MonoBehaviour, IScreen
    {
        protected abstract void OnScreenEnabled();
        protected abstract void OnScreenDisabled();
        protected abstract bool AutoActiveObject { get; }
        protected abstract bool AutoDeactiveObject { get; }
        protected CanvasGroup GraphicGroup { get; private set; }

        public abstract ScreenType Type { get; }

        private bool _Active = false;

        public virtual void Setup()
        {
            _Active = gameObject.activeSelf;
            GraphicGroup = GetComponent<CanvasGroup>();
        }

        public void SetActive(bool wantToActive)
        {
            if (_Active && !wantToActive)
            {
                _Active = false;
                OnScreenDisabled();
                if (AutoDeactiveObject) gameObject.SetActive(false);
            }
            else if (!_Active && wantToActive)
            {
                _Active = true;
                OnScreenEnabled();
                if (AutoActiveObject) gameObject.SetActive(true);
            }
        }
    }
}
