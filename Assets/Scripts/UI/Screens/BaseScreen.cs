using System.Collections;
using UnityEngine;

namespace UI.Screens
{
    public enum ScreenType
    {
        MainMenu,
        MainMenu_Option,
        GamePlay,
        GamePlay_Pause,
    }

    public abstract class BaseScreen : MonoBehaviour
    {
        public abstract ScreenType Type { get; }

        public virtual void OnActive()
        {

        }

        public virtual void OnDeactivate()
        {

        }

        public void SetVisible()
        {

        }
    }
}