using UnityEngine;
using UnityEngine.InputSystem;

public class DHTTeleportationProvider : MonoBehaviour
{
    [SerializeField] private InputActionProperty leftHandTeleport;


    void Start()
    {
    }


    void Update()
    {
        var teleport = leftHandTeleport.action.ReadValue<Vector2>();
        if (teleport.y > .1f)
        {
            Debug.Log($"Stick = {teleport}");
        }
    }
    protected void OnEnable()
    {
        leftHandTeleport.action.Enable();
    }

    /// <summary>
    /// See <see cref="MonoBehaviour"/>.
    /// </summary>
    protected void OnDisable()
    {
        leftHandTeleport.action.Disable();
    }
}
