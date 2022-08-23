using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UI.Overlays
{
    public interface IOverlay
    {
        void SetActive(bool wantToActive);
    }

    public abstract class BaseOverlay : MonoBehaviour, IOverlay
    {
        protected abstract void OnOverlayEnabled();
        protected abstract void OnOverlayDisabled();
        protected abstract bool AutoActiveObject { get; }
        protected abstract bool AutoDeactiveObject { get; }

        private bool _Active = false;

        public virtual void Setup()
        {
            _Active = gameObject.activeSelf;
        }
        
        public void SetActive(bool wantToActive)
        {
            if (_Active && !wantToActive)
            {
                _Active = false;
                OnOverlayDisabled();
                if (AutoDeactiveObject) gameObject.SetActive(false);
            }
            else if (!_Active && wantToActive)
            {
                _Active = true;
                OnOverlayEnabled();
                if (AutoActiveObject) gameObject.SetActive(true);
            }
        }
    }
}
