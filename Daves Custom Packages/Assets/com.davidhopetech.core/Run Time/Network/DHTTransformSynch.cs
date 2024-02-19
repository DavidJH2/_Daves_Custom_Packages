using Unity.Netcode;
using UnityEngine;

public class DHTTransformSynch : NetworkBehaviour
{
    private Transform _transform;

    private NetworkVariable<Vector3> position = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<Vector3> rotation = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    
    
    void Start()
    {
        _transform = GetComponent<Transform>();
    }


    void FixedUpdate()
    {
        if (IsOwner)
        {
            position.Value = _transform.position;
            rotation.Value = _transform.rotation.eulerAngles;
        }
        else
        {
            _transform.position = position.Value;
            _transform.rotation = Quaternion.Euler(rotation.Value);
        }
    }
}
