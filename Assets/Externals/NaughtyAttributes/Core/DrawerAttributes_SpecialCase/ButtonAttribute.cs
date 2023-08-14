using System;

namespace NaughtyAttributes
{
    public enum EnableWhen
    {
        /// <summary>
        /// Button should be active always
        /// </summary>
        Anytime,
        /// <summary>
        /// Button should be active only in editor
        /// </summary>
        Editor,
        /// <summary>
        /// Button should be active only in playmode
        /// </summary>
        Playmode
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ButtonAttribute : SpecialCaseDrawerAttribute
    {
        public string Text { get; private set; }
        public EnableWhen SelectedEnableMode { get; private set; }

        public ButtonAttribute(string text = null, EnableWhen enabledMode = EnableWhen.Anytime)
        {
            Text = text;
            SelectedEnableMode = enabledMode;
        }

        public ButtonAttribute(EnableWhen enabledMode)
        {
            Text = null;
            SelectedEnableMode = enabledMode;
        }
    }
}
