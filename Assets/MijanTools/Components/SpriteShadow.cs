using MijanTools.Common;
using UnityEngine;

namespace MijanTools.Components
{
    public class SpriteShadow : MonoBehaviour
    {
        public SpriteRenderer SpriteRenderer;
        public SpriteRenderer TargetSpriteRenderer;
        public Color Color;
        public float Alpha;
        public Vector3 Offset = new Vector3(2f, -2f, 0f);

        private void LateUpdate()
        {
            SpriteRenderer.sprite = TargetSpriteRenderer.sprite;
            SpriteRenderer.color = Color;
            SpriteRenderer.SetAlpha(Alpha);
            transform.position = TargetSpriteRenderer.transform.position + Offset;
        }
    }
}