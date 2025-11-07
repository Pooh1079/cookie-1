using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ZombieWave
{
    public GameObject zombiePrefab;
    public int count = 1;
    public float delay = 1f;
}

public class ZombieSpawner : MonoBehaviour
{
    public List<ZombieWave> waves = new List<ZombieWave>();
    public float startDelay = 2f;

    void Start()
    {
        StartCoroutine(SpawnAll());
    }

    IEnumerator SpawnAll()
    {
        yield return new WaitForSeconds(startDelay);

        foreach (ZombieWave wave in waves)
        {
            for (int i = 0; i < wave.count; i++)
            {
                SpawnZombie(wave.zombiePrefab);
                yield return new WaitForSeconds(wave.delay);
            }
        }
    }

    void SpawnZombie(GameObject prefab)
    {
        Instantiate(prefab, transform.position, Quaternion.identity);

        // Сообщаем GameManager о новом зомби
        if (GameManager.instance != null)
            GameManager.instance.ZombieSpawned();
    }
}