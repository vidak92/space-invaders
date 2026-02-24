using SGSTools.Extensions;
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
        public float TextPositionYOffsetOnPress;

        public void Awake()
        {
            Image.sprite = SpriteDefault;
            Text.rectTransform.SetAnchoredPositionX(0f);
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
            Text.rectTransform.SetAnchoredPositionY(-TextPositionYOffsetOnPress);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Image.sprite = SpriteDefault;
            Text.rectTransform.SetAnchoredPositionY(0f);
        }
    }
}