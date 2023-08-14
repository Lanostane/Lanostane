using UnityEditor;

namespace NaughtyAttributes.Editor
{
    internal class SavedBool
    {
        private bool _Value;
        private readonly string _Name;

        public bool Value
        {
            get
            {
                return _Value;
            }
            set
            {
                if (_Value == value)
                {
                    return;
                }

                _Value = value;
                EditorPrefs.SetBool(_Name, value);
            }
        }

        public SavedBool(string name, bool value)
        {
            _Name = name;
            _Value = EditorPrefs.GetBool(name, value);
        }
    }
}