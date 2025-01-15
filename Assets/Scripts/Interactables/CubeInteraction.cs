using UnityEngine;
using Unity.Netcode;

public class CubeInteraction : NetworkBehaviour
{
    public GameObject cubePrefab;
    public float spawnRadius = 10f;
    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;

        if (other.CompareTag("Player"))
        {
            DestroyCubeServerRpc();

            SpawnNewCube();
        }
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void DestroyCubeServerRpc()
    {
        Destroy(gameObject);
    }

    private void SpawnNewCube()
    {
        // Generate a random position within a set radius
        Vector3 randomPosition = transform.position + new Vector3(
            Random.Range(-spawnRadius, spawnRadius),
            0f,
            Random.Range(-spawnRadius, spawnRadius)
        );

        // Ensure the new position is on the ground or in a valid spot
        randomPosition.y = transform.position.y;

        // Instantiate the cube on the server
        GameObject newCube = Instantiate(cubePrefab, randomPosition, Quaternion.identity);

        // Spawn the new cube across the network
        newCube.GetComponent<NetworkObject>().Spawn();
    }
}