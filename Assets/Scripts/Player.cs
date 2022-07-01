using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private Transform[] _tripleShotPosition;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject[] _engines;
    [SerializeField]
    private float _fireRate = 0.15f;
    [SerializeField]
    private int _lives = 3;

    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private AudioSource _audioSource;
    private SpriteRenderer _shieldRenderer;

    private float _canFire = -1f;
    private bool _isTripleShotActive;
    private bool _isShieldActive;
    private float _speedMultiplier = 2.0f;
    private int _score;
    private int _randomEngineSelector;
    private float _speedAcceleration = 7.5f;
    private int _shieldStrength = 3;
    private int _laserAmmo = 15;
    private int _randomAmmoMunition;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
            Debug.LogError("Spawn_Manager is NULL!");

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
            Debug.LogError("UI_Manager is NULL!");

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
            Debug.LogError("AudioSource is NULL!");

        _shieldRenderer = _shieldVisualizer.GetComponent<SpriteRenderer>();
        if (_shieldRenderer == null)
            Debug.LogError("Shield SpriteRenderer is NULL!");

        transform.position = Vector3.zero;

        _shieldVisualizer.SetActive(false);
        _engines[0].SetActive(false);
        _engines[1].SetActive(false);

        _randomEngineSelector = Random.Range(0, _engines.Length);
        _randomAmmoMunition = Random.Range(1, 8);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKey(KeyCode.LeftShift))
            _speed = _speedAcceleration;

        if (Input.GetKeyUp(KeyCode.LeftShift))
            _speed = 5.0f;

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _laserAmmo >= 1)
        {
            if (_isTripleShotActive)
            {
                TripleShot();
            }
            else
                FireLaser();

            _audioSource.Play();
        }
    }

    private void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        _laserAmmo--;
        _uiManager.UpdateAmmoCount(_laserAmmo);
        float yOffset = 1.05f;
        var laser = Instantiate(_laserPrefab, new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z), Quaternion.identity);
        laser.gameObject.AddComponent(typeof(PlayerLaser));
        laser.tag = TagManager._playerLaser;
    }

    private void TripleShot()
    {
        _canFire = Time.time + _fireRate;

        foreach (var tripleShotTransform in _tripleShotPosition)
        {
            var tripleShot = Instantiate(_laserPrefab, tripleShotTransform.position, Quaternion.identity);
            tripleShot.gameObject.AddComponent(typeof(PlayerLaser));
            tripleShot.tag = TagManager._playerLaser;
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    public void SpeedActive()
    {
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedPowerupDownRoutine());
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
    }

    public void AddAmmo()
    {
        _laserAmmo += _randomAmmoMunition;

        if (_laserAmmo >= 15)
            _laserAmmo = 15;

        _uiManager.UpdateAmmoCount(_laserAmmo);
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

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            switch (_shieldStrength)
            {
                case 3:
                    _shieldStrength--;
                    _shieldRenderer.color = Color.green;
                    break;
                case 2:
                    _shieldStrength--;
                    _shieldRenderer.color = Color.red;
                    break;
                case 1:
                    _shieldStrength--;
                    _isShieldActive = false;
                    _shieldVisualizer.SetActive(false);
                    break;
            }

            return;
        }

        _lives--;

        switch (_lives)
        {
            case 2:
                _engines[_randomEngineSelector].SetActive(true);
                break;
            case 1:
                if (_engines[0].activeInHierarchy)
                    _engines[1].SetActive(true);
                else
                    _engines[0].SetActive(true);
                break;
        }

        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDead();
            Destroy(gameObject);
        }
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    IEnumerator SpeedPowerupDownRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        _speed /= _speedMultiplier;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagManager._enemyLaser))
        {
            Damage();
            Destroy(collision.gameObject);
        }
    }
}