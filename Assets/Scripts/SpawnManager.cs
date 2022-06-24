using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _powerupPrefab;
    [SerializeField]
    private GameObject _enemy_Container;

    private bool _stopSpawning = false;
    

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
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
        float _tripleShotSpawnTime = Random.Range(2.5f, 7.5f);

        while (_stopSpawning == false)
        {
            var positionToSpawn = new Vector3(Random.Range(-8f, 8f), 7f, 0);
            Instantiate(_powerupPrefab, positionToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(_tripleShotSpawnTime);
        }
    }

    public void OnPlayerDead()
    {
        _stopSpawning = true;
    }
}