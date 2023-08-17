using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils.Unity
{
    public class Rotate : MonoBehaviour
    {
        public Vector3 RotationAxis;
        public float RotationSpeed;

        void Update()
        {
            transform.Rotate(RotationAxis, RotationSpeed * Time.deltaTime);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.DrawLine(transform.position, transform.position + RotationAxis.normalized * 3.0f);    
        }
    }
}

