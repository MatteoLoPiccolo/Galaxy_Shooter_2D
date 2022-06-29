using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 8.5f;
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private AudioClip _explosionClip;

    private SpawnManager _spawnManager;
    private Camera _cam;

    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
            Debug.LogError("Spawn_Manager is NULL!");

        _cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        if (_cam == null)
            Debug.LogError("Main Camera is NULL!");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * -_rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Laser"))
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            AudioSource.PlayClipAtPoint(_explosionClip, _cam.transform.position);
            _spawnManager.StartSpawning();
            Destroy(gameObject, 0.25f);
        }
    }
}