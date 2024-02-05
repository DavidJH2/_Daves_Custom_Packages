using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.davidhopetech.vr.Run_Time.Scripts
{
    public class KeyboardKey : MonoBehaviour, IPointerDownHandler
    {

        private Keyboard        keyboard;
        private TMP_InputField  tmpInputField;
        private TextMeshProUGUI tmp;
        private Button          button;


        public enum KeyType
        {
            Character,
            BackSpace,
            ToggleCase,
            Enter
        }


        public bool IsSpace
        {
            get
            {
                return tmp.text == "Space";
            }
        }

        public bool IsEnter
        {
            get
            {
                return tmp.text == "Enter";
            }
        }

        public bool IsLowerSpace
        {
            get
            {
                return tmp.text == "space";
            }
        }

        public string keyValue
        {
            get
            {
                switch (tmp.text)
                {
                    case "Space":
                        return " ";
                    default:
                        return tmp.text;
                }
            }
        }

        public KeyType keyType
        {
            get
            {
                switch (tmp.text)
                {
                    case "Enter":
                        return KeyType.Enter;
                    case "<":
                        return KeyType.BackSpace;
                    case "^":
                        return KeyType.ToggleCase;
                    default:
                        return KeyType.Character;
                }
            }
        }

        void Start()
        {
            keyboard      = GetComponentInParent<Keyboard>();
            tmpInputField = keyboard.tmpInputField;
            tmp           = GetComponentInChildren<TextMeshProUGUI>();
            button        = GetComponent<Button>();
            //button.onClick.AddListener(DoClick);
        }


        public void ToUppercase()
        {
            if (keyType == KeyType.Character && keyValue != " ")
            {
                var text    = tmp.text;
                var newText = text.ToUpper();
                tmp.text = newText;
            }
        }

        public void ToLowercase()
        {
            if (keyType == KeyType.Character && keyValue != " ")
            {
                var text    = tmp.text;
                var newText = text.ToLower();
                tmp.text = newText;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            keyboard.KeyPressed(this);
        }
    }
}
