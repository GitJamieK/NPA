using UnityEngine;
using Unity.Netcode;
using System.Collections;
using System.Threading.Tasks;
using System;

public class CubeSpawner : NetworkBehaviour
{
    public GameObject CubePrefab;
    float time = .5f;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            StartCoroutine(WaitUntilSpawn());
        }
    }

    /*async Awaitable SpawnCubeAfterTime(float time)
    {
        await Task.Delay((int)time);

        // Spawn the NPC
        if (CubePrefab)
        {
            GameObject[] npcInstances = await InstantiateAsync(CubePrefab);

            if (npcInstances.Length > 0)
            {
                GameObject npcInstance = npcInstances[0];
                NetworkObject networkObject = npcInstance.GetComponent<NetworkObject>();
                if (networkObject)
                {
                    networkObject.Spawn();
                }
            }
        }
    }*/

    public IEnumerator WaitUntilSpawn()
    {
        yield return new WaitForSecondsRealtime(time);

        if (CubePrefab)
        {
            GameObject npcInstance = Instantiate(CubePrefab);

            NetworkObject networkObject = npcInstance.GetComponent<NetworkObject>();
            if (networkObject)
            {
                networkObject.Spawn();
            }
        }
    }
}