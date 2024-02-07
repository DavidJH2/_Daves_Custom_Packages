using System;
using System.Collections;
using System.Collections.Generic;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;


namespace com.davidhopetech.vr.Run_Time.Scripts
{
    public class Keyboard : MonoBehaviour
    {
        public DHT_TMP_InputField tmpInputField;

        private KeyboardKey[] keys;
        private bool          upercase = true;

        [SerializeField] private UnityEvent           KeyboardEnterEvent;
        private                  DebugPanel           _debugPanel;


        int SelectionStart
        {
            get
            {
                var anchorPosition = 0;
                var focusPosition  = 0;
                // if (tmpInputField != null)
                {
                    anchorPosition = tmpInputField.selectionStringAnchorPosition;
                    focusPosition  = tmpInputField.selectionStringFocusPosition;
                }

                return Math.Min(anchorPosition, focusPosition);
            }
        }


        int SelectionEnd
        {
            get
            {
                var anchorPosition = 0;
                var focusPosition  = 0;
                if (tmpInputField != null)
                {
                    anchorPosition = tmpInputField.selectionStringAnchorPosition;
                    focusPosition  = tmpInputField.selectionStringFocusPosition;
                }

                return Math.Max(anchorPosition, focusPosition);
            }
        }

        bool AtTextEntryStart
        {
            get
            {
                return SelectionStart == 0;
            }
        }

        void Start()
        {
            keys = GetComponentsInChildren<KeyboardKey>();
            SetUppercaseIfFirstCharacter();
            InitInputFieldCallbacks();
        }


        private void Update()
        {
        }
        
        
        void InitInputFieldCallbacks()
        {
            var tmpifs = GameObjectExtensions.FindAllComponentsOfType<DHT_TMP_InputField>();
            foreach (var tmpif in tmpifs)
            {
                var a = tmpif;
                tmpif.onSelect.AddListener((a) =>
                {
                    SetCurrentInputField(tmpif);
                    SetUppercaseIfFirstCharacter();
                });
            }
        }

        void SetCurrentInputField(DHT_TMP_InputField tmpif)
        {
            tmpInputField = tmpif;
        }

        void SetUppercaseIfFirstCharacter()
        {
            if (tmpInputField == null) return;
            
            if ((SelectionStart == 0 && SelectionEnd == 0) || (tmpInputField.caretPosition>0 && tmpInputField.text[tmpInputField.caretPosition - 1] == ' '))
            {
                if (!upercase)
                    ToggleCase();
            }
            else
            {
                if (upercase)
                    ToggleCase();;
            }
        }


        public void KeyPressed(KeyboardKey pressedKey)
        {
            var addText        = pressedKey.keyValue;
            var anchorPosition = tmpInputField.selectionStringAnchorPosition;
            var focusPosition  = tmpInputField.selectionStringFocusPosition;

            var start = Math.Min(anchorPosition, focusPosition);
            var end   = Math.Max(anchorPosition, focusPosition);

            var text     = tmpInputField.text;
            var caretPos = tmpInputField.caretPosition;

            string newText;
            int    newPos;

            switch (pressedKey.keyType)
            {
                case KeyboardKey.KeyType.Enter:
                    KeyboardEnterEvent.Invoke();
                    break;

                case KeyboardKey.KeyType.BackSpace:

                    if (tmpInputField.selectionAnchorPosition == tmpInputField.selectionFocusPosition)
                    {
                        var pos = tmpInputField.caretPosition;

                        if (pos > 1)
                        {
                            newText = text.Substring(0, pos - 1) + text.Substring(pos, text.Length - pos);
                            newPos  = pos - 1;
                        }
                        else
                        {
                            newText = text.Substring(pos, text.Length - pos);
                            newPos  = 0;
                        }

                        tmpInputField.text = newText;

                        tmpInputField.caretPosition                 = newPos;
                        tmpInputField.selectionStringAnchorPosition = newPos;
                        tmpInputField.selectionStringFocusPosition  = newPos;
                    }
                    else
                    {
                        newText            = text.Substring(0, start) + text.Substring(end, text.Length - end);
                        tmpInputField.text = newText;

                        newPos                                      = start;
                        tmpInputField.caretPosition                 = newPos;
                        tmpInputField.selectionStringAnchorPosition = newPos;
                        tmpInputField.selectionStringFocusPosition  = newPos;
                    }

                    SetUppercaseIfFirstCharacter();
                    break;

                case KeyboardKey.KeyType.ToggleCase:
                    ToggleCase();
                    break;

                default:
                    // Replace any Selected Text with 'addText'
                    newText            = text.Substring(0, start) + addText + text.Substring(end, text.Length - end);
                    tmpInputField.text = newText;

                    // Place cursor at the end of 'addText'
                    newPos = start + addText.Length;

                    tmpInputField.caretPosition                 = newPos;
                    tmpInputField.selectionStringAnchorPosition = newPos;
                    tmpInputField.selectionStringFocusPosition  = newPos;


                    tmpInputField.ActivateInputField();
                    tmpInputField.DeactivateInputField();
                    //Invoke("AcrivateInputField", 2f);
                    StartCoroutine(MyCoroutine());
                    
                    SetUppercaseIfFirstCharacter();
                    break;
            }

        }

        IEnumerator MyCoroutine()
        {
            yield return new WaitForSeconds(2f);
            tmpInputField.DeactivateInputField();

            yield break;
            
            yield return new WaitForSeconds(.5f);
            tmpInputField.ActivateInputField();
            yield return new WaitForSeconds(1f);
            tmpInputField.ActivateInputField();
            yield return new WaitForSeconds(1f);
            tmpInputField.ActivateInputField();
            // yield return new WaitForSeconds(2f);
            // EventSystem.current.SetSelectedGameObject(null);
            //yield return new WaitForSeconds(.1f);
            // tmpInputField.Select();
            /*
            yield return new WaitForSeconds(.1f);
            EventSystem.current.SetSelectedGameObject(null);
            yield return new WaitForSeconds(.1f);
            tmpInputField.Select();
            */
            yield break;
        }

        
        /*
        private void AcrivateInputField()
        {
            EventSystem.current.SetSelectedGameObject(null);
            // tmpInputField.ActivateInputField();
            // tmpInputField.DeactivateInputField();\
            // other();
            Invoke("other", 1f);
        }

        void other()
        {
            tmpInputField.Select();
        }
        */

        public void ToggleCase()
        {
            if (upercase)
            {
                ToLowercase();
            }
            else
            {
                ToUppercase();
            }

            upercase = !upercase;

            void ToUppercase()
            {
                foreach (var key in keys)
                {
                    key.ToUppercase();
                }
            }


            void ToLowercase()
            {
                foreach (var key in keys)
                {
                    key.ToLowercase();
                }
            }
        }
    }
}
