using UnityEngine;
using Unity.Netcode;

public class CubeInteraction : NetworkBehaviour
{
    public GameObject cubePrefab;
    public CubeSpawner cubeSpawner;
    
    /*private void Start()
    {
        if (IsOwner)
        {
            GameObject CubeSpawner = GameObject.FindWithTag("Respawn");
            if (CubeSpawner != null)
            {
                cubeSpawner = CubeSpawner.GetComponent<CubeSpawner>();
            }
        }
    }*/
    
    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;

        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.AddScoreServerRpc();
            }

            HandleCubeInteractionServerRpc();
        }
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void HandleCubeInteractionServerRpc()
    {
        Destroy(gameObject);
        CubeSpawner spawner = Object.FindFirstObjectByType<CubeSpawner>();
        if (spawner != null)
        {
            spawner.StartCoroutine(spawner.WaitUntilSpawn());
        }
        else
        {
            Debug.LogError("CubeSpawner instance not found in the scene.");
        }
        //SpawnNewCubeServerRpc();
    }
    
    /*[ServerRpc(RequireOwnership = false)]
    private void SpawnNewCubeServerRpc()
    {
        Vector3 randomPosition = transform.position + new Vector3(
            Random.Range(-spawnRadius, spawnRadius),
            0f,
            Random.Range(-spawnRadius, spawnRadius)
        );

        randomPosition.y = transform.position.y;

        GameObject newCube = Instantiate(cubePrefab, randomPosition, Quaternion.identity);
        Debug.Log("Spawning new cube at " + randomPosition);
        newCube.GetComponent<NetworkObject>().Spawn();
    }
    
    /*private void SpawnNewCube()
    {
        Vector3 randomPosition = transform.position + new Vector3(
            Random.Range(-spawnRadius, spawnRadius),
            0f,
            Random.Range(-spawnRadius, spawnRadius)
        );

        randomPosition.y = transform.position.y;
        
        GameObject newCube = Instantiate(cubePrefab, randomPosition, Quaternion.identity);
        newCube.GetComponent<NetworkObject>().Spawn();
    }*/
}