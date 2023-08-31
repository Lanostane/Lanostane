using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LST.GamePlay
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    internal sealed class MainGameCamera : MonoBehaviour
    {
        //public AssetReferenceT<Material> BlurMaterial;

        //private Material _BlurMat;

        private void Awake()
        {
            GamePlays.MainCam = GetComponent<Camera>();
            GamePlays.MainCamTransform = transform;
            //_BlurMat = BlurMaterial.LoadAssetAsync<Material>().WaitForCompletion();
        }

        private void OnDestroy()
        {
            GamePlays.MainCam = null;
            GamePlays.MainCamTransform = null;
        }

        //private void OnRenderImage(RenderTexture source, RenderTexture destination)
        //{
        //    UnityEngine.Graphics.Blit(source, destination, _BlurMat);
        //}
    }
}
