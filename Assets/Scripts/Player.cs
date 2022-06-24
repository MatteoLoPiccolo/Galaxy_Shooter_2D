using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.15f;
    [SerializeField]
    private int _lives = 3;

    private SpawnManager _spawnManager;
    private float _canFire = -1f;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
            Debug.Log("SpawnManager is NULL!");
        
        transform.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    private void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        float yOffset = 1.05f;
        Instantiate(_laserPrefab, new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z), Quaternion.identity);
    }

    private void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        var movement = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(movement * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.15f, 0f), 0);

        if (transform.position.x <= -11.25)
        {
            transform.position = new Vector3(11.25f, transform.position.y, 0);
        }
        else if (transform.position.x >= 11.25)
        {
            transform.position = new Vector3(-11.25f, transform.position.y, 0);
        }
    }

    public void Damage()
    {
        _lives--;

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDead();
            Destroy(gameObject);
        }
    }
}