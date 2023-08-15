using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LST.Player
{
    [RequireComponent(typeof(Camera))]
    public sealed class MainGameCamera : MonoBehaviour
    {
        public static Camera Cam { get; private set; }
        public static Transform Transform { get; private set; }

        void Awake()
        {
            Cam = GetComponent<Camera>();
            Transform = Cam.transform;
        }

        void OnDestroy()
        {
            Cam = null;
            Transform = null;
        }
    }
}
