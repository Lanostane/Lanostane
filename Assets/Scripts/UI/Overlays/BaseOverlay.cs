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
        void OnOverlayEnabled();
        void OnOverlayDisabled();
    }

    public abstract class BaseOverlay : MonoBehaviour, IOverlay
    {
        public abstract void OnOverlayEnabled();
        public abstract void OnOverlayDisabled();
    }
}
