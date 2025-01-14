using UnityEngine;
using Unity.Netcode;

public class CubeInteraction : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (IsServer)
            {
                DestroyCube();
            }
            else
            {
                DestroyCubeServerRpc();
            }
        }
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void DestroyCubeServerRpc()
    {
        DestroyCube();
    }

    private void DestroyCube()
    {
        GetComponent<NetworkObject>().Despawn(true);
    }
}