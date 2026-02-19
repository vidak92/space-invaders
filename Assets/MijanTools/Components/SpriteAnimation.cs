using MijanTools.Common;
using UnityEngine;

namespace MijanTools.Components
{
    // TODO: Add looping options.
    public class SpriteAnimation : MonoBehaviour
    {
        public SpriteRenderer SpriteRenderer;

        [Space]
        [Tooltip("Time between frames in seconds.")]
        public float Delay;
        public Sprite[] Sprites;

        private bool _isRunning;
        private float _timer;
        private int _spriteIndex;

        private void Update()
        {
            if (_isRunning && !Sprites.IsNullOrEmpty())
            {
                _timer += Time.deltaTime;
                if (_timer >= Delay)
                {
                    _timer -= Delay;
                    _spriteIndex = (_spriteIndex + 1) % Sprites.Length;
                    SetSpriteIndex(_spriteIndex);
                }
            }
        }

        public void StartAnimation()
        {
            _isRunning = true;
            _timer = 0f;
            SetSpriteIndex(0);
        }

        public void StopAnimation()
        {
            _isRunning = false;
            _timer = 0f;
            SetSpriteIndex(0);
        }

        private void SetSpriteIndex(int index)
        {
            _spriteIndex = index;
            if (!Sprites.IsNullOrEmpty() && Sprites.ContainsIndex(index))
            {
                SpriteRenderer.sprite = Sprites[index];
            }
        }
    }
}