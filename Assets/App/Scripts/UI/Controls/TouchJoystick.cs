using UnityEngine;
using UnityEngine.EventSystems;

namespace SpaceInvaders
{
    public class TouchJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
    {
        public RectTransform RectTransform;
        public RectTransform PadRectTransform;
        public RectTransform StickRectTransform;
        
        [Space]
        public Vector2 DefaultPosition;

        private bool _isPressed;
        private Vector2 _startPosition;

        public Vector2 Direction { get; private set; }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isPressed = true;
            // Debug.Log($"pointer down: {eventData.position}");
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(RectTransform, eventData.position, null, out var localPosition);
            PadRectTransform.anchoredPosition = localPosition - PadRectTransform.sizeDelta / 2f;
            StickRectTransform.anchoredPosition = Vector2.zero;
            
            _startPosition = localPosition;
            Direction = Vector2.zero;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isPressed = false;
            // Debug.Log($"pointer up: {eventData.position}");
            
            PadRectTransform.anchoredPosition = DefaultPosition - PadRectTransform.sizeDelta / 2f;
            StickRectTransform.anchoredPosition = Vector2.zero;
            
            _startPosition = DefaultPosition;
            Direction = Vector2.zero;
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (!_isPressed) return;
            
            // Debug.Log($"pointer move: {eventData.position}");
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(RectTransform, eventData.position, null, out var localPosition);
            var direction = localPosition - _startPosition;
            var maxDirectionMagnitude = PadRectTransform.sizeDelta.x / 2f;
            direction = Vector2.ClampMagnitude(direction, maxDirectionMagnitude);
            StickRectTransform.anchoredPosition = direction;
            
            Direction = direction.normalized;
        }
    }
}