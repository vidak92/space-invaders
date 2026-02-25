using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SpaceInvaders
{
    public class TouchButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public Action OnPressed;
        public Action OnReleased;

        public void OnPointerDown(PointerEventData eventData)
        {
            OnPressed?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnReleased?.Invoke();
        }
    }
}