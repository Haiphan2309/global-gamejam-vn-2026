using UnityEngine;
using System.Collections;
using System.Linq;

public class FlySpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject flyPrefab;
    [SerializeField] private float startDelay = 1f;
    [SerializeField] private float respawnDelay = 2f;

    [Header("Spawn Locations")]
    [SerializeField] private Transform[] spawnPoints;

    private GameObject currentFly;
    private bool isWaitingToSpawn = false;


    void Start()
    {
        LoadSpawnPoints();
        StartCoroutine(InitialSpawnRoutine());
    }

    private void LoadSpawnPoints()
    {
        GameObject container = GameObject.FindGameObjectWithTag("FlySpawnPos");
        if (container == null) return;
        spawnPoints = container.GetComponentsInChildren<Transform>()
                               .Where(t => t != container.transform)
                               .ToArray();
    }

    void Update()
    {
        if (currentFly == null && !isWaitingToSpawn)
        {
            StartCoroutine(RespawnRoutine());
        }
    }

    IEnumerator InitialSpawnRoutine()
    {
        isWaitingToSpawn = true;

        yield return new WaitForSeconds(startDelay);

        SpawnFly();

        isWaitingToSpawn = false;
    }

    IEnumerator RespawnRoutine()
    {
        isWaitingToSpawn = true;

        yield return new WaitForSeconds(respawnDelay);

        SpawnFly();

        isWaitingToSpawn = false;
    }

    void SpawnFly()
    {
        if (spawnPoints.Length == 0) return;

        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];

        currentFly = Instantiate(flyPrefab, spawnPoint.position, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        if (spawnPoints == null) return;

        Gizmos.color = Color.red;
        foreach (Transform point in spawnPoints)
        {
            if (point != null)
                Gizmos.DrawSphere(point.position, 0.2f);
        }
    }
}