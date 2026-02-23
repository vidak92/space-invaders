using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SpaceInvaders
{
    public class UIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public Button Button;
        public Image Image;
        public TMP_Text Text;

        [Space]
        public Sprite SpriteDefault;
        public Sprite SpritePressed;

        public void Awake()
        {
            Image.sprite = SpriteDefault;
            Button.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            Image.sprite = SpriteDefault;
        }

        private void OnButtonClick()
        {
            // @TODO play sound, maybe invoke action/callback?
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Image.sprite = SpritePressed;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Image.sprite = SpriteDefault;
        }
    }
}