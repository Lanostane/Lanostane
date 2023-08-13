using UnityEngine;

namespace LST.Player.Graphics
{
    public class FlickArrowVisual : MonoBehaviour
    {
        private static readonly int MAIN_TEX = Shader.PropertyToID("_MainTex");
        private static readonly int MAIN_TEX_ST = Shader.PropertyToID("_MainTex_ST");

        public Texture Texture;
        public Renderer Renderer;
        public bool InDirection = false;

        private MaterialPropertyBlock _Props;
        private float _Offset;

        void Awake()
        {
            _Offset = UnityEngine.Random.Range(0.0f, 1.0f);
            _Props = new();
            _Props.SetTexture(MAIN_TEX, Texture);
        }

        void Update()
        {
            var delta = Time.deltaTime * 2.0f;
            if (InDirection)
                delta = -delta;

            _Offset += delta;

            _Props.SetVector(MAIN_TEX_ST, new Vector4(1.0f, 1.0f, 0.0f, _Offset));
            Renderer.SetPropertyBlock(_Props);
        }
    }
}
