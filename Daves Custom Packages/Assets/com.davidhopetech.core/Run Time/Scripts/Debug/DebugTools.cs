using UnityEngine;

public class DebugTools : MonoBehaviour
{
    [SerializeField] private GameObject _activator;
   
    private string DebugToolsActivationStateString = "Visible";

    
    void Start()
    {
        UpdateDebugToolsActvationState();
    }

    
    public bool Visible
    {
        get =>  PlayerPrefs.GetInt(DebugToolsActivationStateString, 1) != 0;
        set
        {
            PlayerPrefs.SetInt(DebugToolsActivationStateString, value ? 1 : 0); 
            UpdateDebugToolsActvationState();
        }
    }

    
    void UpdateDebugToolsActvationState()
    {
        _activator.SetActive(Visible);
    }
}
