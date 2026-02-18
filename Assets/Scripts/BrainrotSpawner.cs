using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainrotSpawner : MonoBehaviour
{
    public static BrainrotSpawner Instance;

    [Header("Spawn Settings")]
    public List<GameObject> brainrotPrefabs;   // All different brainrot prefabs
    public int maxBrainrots = 20;

    [Header("Sand Area")]
    public Transform sandArea;   // Empty object covering sand

    private int currentCount;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartCoroutine(SpawnInitial());
    }

    IEnumerator SpawnInitial()
    {
        for (int i = 0; i < maxBrainrots; i++)
        {
            SpawnBrainrot();
            yield return new WaitForSeconds(Random.Range(0.1f, 0.4f));
        }
    }


    public void SpawnBrainrot()
    {
        if (brainrotPrefabs.Count == 0) return;

        Vector3 randomPos = GetRandomPosition();

        GameObject prefab = brainrotPrefabs[Random.Range(0, brainrotPrefabs.Count)];
        GameObject obj = Instantiate(prefab, randomPos, Quaternion.identity);

        BrainrotInteraction bi = obj.GetComponent<BrainrotInteraction>();
        if (bi != null)
        {
            bi.RandomizeSinkValues();
        }


        currentCount++;
    }

    Vector3 GetRandomPosition()
    {
        if (sandArea == null)
            return Vector3.zero;

        // Unity plane size = 10 units
        float width = 10f * sandArea.localScale.x;
        float length = 10f * sandArea.localScale.z;

        float x = Random.Range(-width / 2f, width / 2f);
        float z = Random.Range(-length / 2f, length / 2f);

        Vector3 center = sandArea.position;

        return new Vector3(center.x + x, center.y + 0.1f, center.z + z);
    }


    public void OnBrainrotDestroyed()
    {
        currentCount--;

        if (currentCount < maxBrainrots)
        {
            SpawnBrainrot();
        }
    }
}
