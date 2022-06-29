using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
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
    
    private float _canFire = -1f;
    private bool _isTripleShotActive;
    private bool _isShieldActive;
    private float _speedMultiplier = 2.0f;
    private int _score;
    private int _randomEngineSelector;

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

        transform.position = Vector3.zero;

        _shieldVisualizer.SetActive(false);
        _engines[0].SetActive(false);
        _engines[1].SetActive(false);

        _randomEngineSelector = Random.Range(0, _engines.Length);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
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
        float yOffset = 1.05f;
        Instantiate(_laserPrefab, new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z), Quaternion.identity);
    }

    private void TripleShot()
    {
        _canFire = Time.time + _fireRate;
        Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
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
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
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
}