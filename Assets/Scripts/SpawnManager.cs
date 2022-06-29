using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemy_Container;
    [SerializeField]
    private GameObject[] _powerups;

    private bool _stopSpawning = false;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {
            var positionToSpawn = new Vector3(Random.Range(-8f, 8f), 7f, 0);
            var newEnemy = Instantiate(_enemyPrefab, positionToSpawn, Quaternion.identity);
            newEnemy.transform.SetParent(_enemy_Container.transform);
            yield return new WaitForSeconds(2.5f);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        float _randomSpawnTime = Random.Range(2.5f, 7.5f);

        while (_stopSpawning == false)
        {
            int randomPowerup = Random.Range(0, _powerups.Length);
            var positionToSpawn = new Vector3(Random.Range(-8f, 8f), 7f, 0);
            Instantiate(_powerups[randomPowerup], positionToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(_randomSpawnTime);
        }
    }

    public void OnPlayerDead()
    {
        _stopSpawning = true;
    }
}