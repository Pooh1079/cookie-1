using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ZombieWave
{
    public GameObject zombiePrefab; // какой префаб зомби
    public int count = 1;           // сколько заспавнить
    public float delay = 1f;        // задержка между спавном
}

public class ZombieSpawner : MonoBehaviour
{
    public List<ZombieWave> waves = new List<ZombieWave>(); // список волн
    public float startDelay = 2f; // задержка перед началом спавна

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
    }
}
