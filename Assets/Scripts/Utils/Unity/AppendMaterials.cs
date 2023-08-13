using Cysharp.Threading.Tasks.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Utils.Unity
{
    public sealed class AppendMaterials : MonoBehaviour
    {
        public Material[] MaterialsToAppend;

        private void Start()
        {
            var renderers = GetComponentsInChildren<Renderer>(includeInactive: true);
            foreach (var renderer in renderers)
            {
                if (renderer.gameObject.CompareTag("DoNotAppendMaterials"))
                    continue;

                var mats = renderer.sharedMaterials;
                var newMats = mats.Concat(MaterialsToAppend).ToArray();
                renderer.sharedMaterials = newMats;
            }
        }
    }
}
