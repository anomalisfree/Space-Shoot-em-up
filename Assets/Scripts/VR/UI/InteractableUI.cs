using UnityEngine;
using UnityEngine.Events;

namespace VR.UI
{
    public class InteractableUI : MonoBehaviour
    {
        private const float DeltaOnHighlight = 20f;

        public bool isPressed;
        public UnityEvent onClick;

        private bool isClicked;
        private bool canClick;
        private bool isHighlighted;
        
        public void HighlightOn()
        {
            GetComponent<RectTransform>().anchoredPosition3D = new Vector3(
                GetComponent<RectTransform>().anchoredPosition3D.x,
                GetComponent<RectTransform>().anchoredPosition3D.y, -DeltaOnHighlight);
            
            if (!isHighlighted)
            {
                canClick = false;
                isPressed = true;
            }

            isHighlighted = true;
        }
        
        public void HighlightOff()
        {
            GetComponent<RectTransform>().anchoredPosition3D = new Vector3(
                GetComponent<RectTransform>().anchoredPosition3D.x,
                GetComponent<RectTransform>().anchoredPosition3D.y, 0f);

            isHighlighted = false;

            isClicked = false;
            isPressed = false;
        }

        private void Update()
        {
            if (!isClicked && isPressed && canClick)
            {
                isClicked = true;
                onClick.Invoke();
            }

            if (isPressed) return;
            isClicked = false;
            canClick = true;
        }
    }
}
