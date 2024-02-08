using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextMeshProCanvasSoringBugWorkAround : MonoBehaviour, ISelectHandler
{
    Canvas canvas;

    
    public void OnSelect(BaseEventData a)
    {
        if (canvas)
        {
            return;
        }
        
        var dropdown = GetComponentInChildren<TMP_Dropdown>();
            
        dropdown.Show();        // Dropdown List is not Created Until the first time it is Shown
        dropdown.Hide();

        canvas = GetComponentInChildren<Canvas>(true);
        
        canvas.gameObject.SetActive(true);      // Need to make canvas GameObject active in order to modify 'canvas.overrideSorting'
        canvas.overrideSorting = false;
        canvas.gameObject.SetActive(false);
    }
}
