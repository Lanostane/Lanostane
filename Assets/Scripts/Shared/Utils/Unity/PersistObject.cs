using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils.Unity
{
    public class PersistObject : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}
