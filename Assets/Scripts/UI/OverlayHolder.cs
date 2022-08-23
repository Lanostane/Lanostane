using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Overlays;
using UnityEngine;

namespace UI
{
    public interface IOverlayHolder
    {
        ILoadingOverlay Loading { get; }
        IOverlay GameHeader { get; }
        IOverlay GameResult { get; }
    }

    public class OverlayHolder : MonoBehaviour, IOverlayHolder
    {
        [SerializeField] private LoadingOverlay _Loading;
        [SerializeField] private GameHeaderOverlay _GameHeader;
        [SerializeField] private GameResultOverlay _GameResult;

        public ILoadingOverlay Loading => _Loading;
        public IOverlay GameHeader => _GameHeader;
        public IOverlay GameResult => _GameResult;

        void Awake()
        {
            _Loading.Setup();
            _GameHeader.Setup();
            _GameResult.Setup();
        }
    }
}
