using TMPro;
using UnityEngine;

namespace VR.UI
{
    public class InputFieldVR : MonoBehaviour
    {
        [SerializeField] private KeyboardVR keyboard;
        [SerializeField] private GameObject title;
        [SerializeField] private TextMeshProUGUI textMeshPro;
        [SerializeField] private int maxSymbols = 30;
        [SerializeField] private bool hide;

        private string text;
        
        public void OnClick()
        {
            if (keyboard.gameObject.activeSelf)
            {
                keyboard.EnterPressed();
            }
            else
            {
                keyboard.gameObject.SetActive(true);
                keyboard.SetText(textMeshPro.text);
                keyboard.TextChanged += OnChangeText;
                keyboard.EnterPressed += OnEnterPress;
            }
        }

        public string GetText()
        {
            return text;
        }

        private void OnEnterPress()
        {
            keyboard.TextChanged -= OnChangeText;
            keyboard.EnterPressed -= OnEnterPress;
            keyboard.gameObject.SetActive(false);
        }

        private void OnChangeText(string newText)
        {
            if (newText.Length > maxSymbols)
            {
                newText = newText.Substring(0, maxSymbols);
                keyboard.SetText(newText);
            }

            text = newText;
            
            if (hide)
            {
                textMeshPro.text = "";
                
                for (var i = 0; i < text.Length; i++)
                {
                    textMeshPro.text += "*";
                }
            }
            else
            {
                textMeshPro.text = text;
            }

            title.SetActive(textMeshPro.text.Length <= 0);
        }
    }
}
