using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace VR.UI
{
    public class KeyboardVR : MonoBehaviour
    {
        public Action<string> TextChanged;
        public Action EnterPressed;

        [SerializeField] private List<GameObject> keysWithLetters;
        private string text;
        private bool isCapsLock;

        public void AddSymbol(GameObject symbolKey)
        {
            text += symbolKey.name;
            TextChanged?.Invoke(text);

            if (!isCapsLock)
            {
                ToLowerLetters();
            }
        }

        public void Backspace()
        {
            if (text.Length > 0)
            {
                text = text.Substring(0, text.Length - 1);
            }
            
            TextChanged?.Invoke(text);
        }

        public void CapsLock()
        {
            isCapsLock = !isCapsLock;

            if (isCapsLock)
            {
                ToUpperLetters();
            }
            else
            {
                ToLowerLetters();
            }
        }

        public void Shift()
        {
            isCapsLock = false;
            ToUpperLetters();
        }

        public void ClearAll()
        {
            text = "";
        }

        public void Enter()
        {
            EnterPressed?.Invoke();
        }

        public void SetText(string newText)
        {
            text = newText;
        }

        private void ToUpperLetters()
        {
            foreach (var key in keysWithLetters)
            {
                key.name = key.name.ToUpper();
                key.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = key.name;
            }
        }
        
        private void ToLowerLetters()
        {
            foreach (var key in keysWithLetters)
            {
                key.name = key.name.ToLower();
                key.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = key.name;
            }
        }
    }
}
