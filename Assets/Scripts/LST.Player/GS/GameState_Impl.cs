using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LST.Player
{
    internal sealed class GameState_Impl : MonoBehaviour
    {
        void Awake()
        {
            GameState.Instance = this;
        }

        void OnDestroy()
        {
            GameState.Instance = null;    
        }

        void Update()
        {
            
        }
    }
}
