using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LST.GamePlay
{
    [RequireComponent(typeof(Camera))]
    internal sealed class MainGameCamera : MonoBehaviour
    {
        void Awake()
        {
            GamePlays.MainCam = GetComponent<Camera>();
            GamePlays.MainCamTransform = transform;
        }

        void OnDestroy()
        {
            GamePlays.MainCam = null;
            GamePlays.MainCamTransform = null;
        }
    }
}
