using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LST.Player
{
    [RequireComponent(typeof(Camera))]
    internal sealed class MainGameCamera : MonoBehaviour
    {
        void Awake()
        {
            GamePlay.MainCam = GetComponent<Camera>();
            GamePlay.MainCamTransform = transform;
        }

        void OnDestroy()
        {
            GamePlay.MainCam = null;
            GamePlay.MainCamTransform = null;
        }
    }
}
