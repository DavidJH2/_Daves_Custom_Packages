using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Keyboard : MonoBehaviour
{
    public TMP_InputField tmpInputField;

    private KeyboardKey[] keys;
    private bool          upercase = true;

    [SerializeField] private UnityEvent KeyboardEnterEvent;
    
    
    
    int SelectionStart
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
        keys        = GetComponentsInChildren<KeyboardKey>();
        CapitalizeFirstCharacter();
        
        var tmpifs = EventSystem.FindObjectsByType<TMP_InputField>(FindObjectsSortMode.None);
        foreach (var tmpif in tmpifs)
        {
            var a = tmpif;
            tmpif.onSelect.AddListener((a) =>
            {
                SetCurrentInputField(tmpif);
            });
        }
    }

    void SetCurrentInputField(TMP_InputField tmpif)
    {
        tmpInputField = tmpif;
    }

    void CapitalizeFirstCharacter()
    {
        if ((SelectionStart == 0 && SelectionEnd == 0) || tmpInputField.text[tmpInputField.caretPosition - 1] == ' ')
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
                        newPos  = pos-1;
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

                    newPos  = start;
                    tmpInputField.caretPosition                 = newPos;
                    tmpInputField.selectionStringAnchorPosition = newPos;
                    tmpInputField.selectionStringFocusPosition  = newPos;
                }

                CapitalizeFirstCharacter();
                break;
            
            case KeyboardKey.KeyType.ToggleCase:
                ToggleCase();
                break;

            default:
                newText = text.Substring(0, start) + addText + text.Substring(end, text.Length - end);
                newPos  = start + addText.Length;

                tmpInputField.text                          = newText;
                tmpInputField.caretPosition                 = newPos;
                tmpInputField.selectionStringAnchorPosition = newPos;
                tmpInputField.selectionStringFocusPosition  = newPos;

                CapitalizeFirstCharacter();
                break;
        }
    }


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
