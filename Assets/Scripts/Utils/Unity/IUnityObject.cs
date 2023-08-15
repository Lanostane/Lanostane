﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Utils.Unity
{
    public interface IUnityObject
    {
#pragma warning disable IDE1006 // Naming Styles
        public string name { get; set; }
        public GameObject gameObject { get; }
        public Transform transform { get; }
#pragma warning restore IDE1006 // Naming Styles

        public int GetInstanceID();
        public UnityEngine.Object GetUnityObjectReference();
    }
}
