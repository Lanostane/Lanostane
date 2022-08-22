using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Overlays;
using UI.Screens;
using UnityEngine;

namespace UI
{
    public interface IOverlayHolder
    {
        ILoadingOverlay Loading { get; }
        IOverlay GameHeader { get; }
    }

    public class OverlayHolder : MonoBehaviour, IOverlayHolder
    {
        [SerializeField] private LoadingOverlay _Loading;
        [SerializeField] private GameHeaderOverlay _GameHeader;

        public ILoadingOverlay Loading => _Loading;
        public IOverlay GameHeader => _GameHeader;
    }
}
