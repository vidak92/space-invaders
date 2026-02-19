using UnityEngine;

namespace MijanTools.Components
{
    public class PostProccessingEffect : MonoBehaviour
    {
        public Material EffectsMaterial;

        private void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            Graphics.Blit(src, dest, EffectsMaterial);
        }
    }
}