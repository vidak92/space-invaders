using SpaceInvaders.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SpaceInvaders.UI
{
    public class OnScreenButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        // Fields
        [Header("Components")]
        [SerializeField]
        private Image _image;

        [Header("Config")]
        [SerializeField]
        private InputAction _inputAction;

        [SerializeField]
        private Sprite _sprite;

        [SerializeField]
        private Color _defaultColor;

        [SerializeField]
        private Color _pressedColor;

        private InputService _inputService;

        public void Init(InputService inputService)
        {
            _inputService = inputService;
        }

        // Unity Events
        private void Update()
        {
            var isPressed = _inputService.GetInputAction(_inputAction);
            _image.color = isPressed ? _pressedColor : _defaultColor;
        }

        private void OnValidate()
        {
            if (_image != null)
            {
                _image.sprite = _sprite;
                _image.color = _defaultColor;
            }
        }

        // Event Handlers
        public void OnPointerEnter(PointerEventData eventData)
        {
            _inputService.SetInputAction(_inputAction, true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _inputService.SetInputAction(_inputAction, false);
        }
    }
}