using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utils.Unity;

namespace Loadings.Visuals
{
    public interface ILoadingVisual : IUnityObject
    {
        LoadingStyles Type { get; }
        void Setup();
        Coroutine ShowScreen(bool animation = true);
        Coroutine HideScreen(bool animation = true);
        void SetMainText(string text);
        void SetTaskText(string text);
        void SetMainProgress(float p);
        void SetTaskProgress(float p);
    }
}
