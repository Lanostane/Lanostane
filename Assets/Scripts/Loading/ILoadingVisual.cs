using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Loading
{
    public interface ILoadingVisual
    {
        LoadingStyles Type { get; }
        GameObject gameObject { get; }
        Coroutine ShowScreen(bool animation = true);
        Coroutine HideScreen(bool animation = true);
        void SetMainText(string text);
        void SetTaskText(string text);
        void SetMainProgress(float p);
        void SetTaskProgress(float p);
    }
}
