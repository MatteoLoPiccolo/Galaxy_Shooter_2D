using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _scoreText;
    [SerializeField]
    private TMP_Text _gameOverText;
    [SerializeField]
    private TMP_Text _restartText;
    [SerializeField]
    private TMP_Text _quitText;
    [SerializeField]
    private TMP_Text _ammoCountText;
    [SerializeField]
    private Image _LivesImg;
    [SerializeField]
    private Sprite[] _liveSprites;

    private Player _player;
    private GameManager _gameManager;

    public static Action<int> OnUpdateScore;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
            Debug.LogError("Player is NULL!");

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null)
            Debug.LogError("Game_Manager is NULL!");

        _scoreText.text = "Score: " + 0;

        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _quitText.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Player.OnUpdateScore += UpdateScore;
        Player.OnUpdateLives += UpdateLives;
        Player.OnUpdateAmmo += UpdateAmmoCount;
    }

    private void OnDisable()
    {
        Player.OnUpdateScore -= UpdateScore;
        Player.OnUpdateLives -= UpdateLives;
        Player.OnUpdateAmmo -= UpdateAmmoCount;
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score : " + playerScore.ToString();
    }

    public void UpdateAmmoCount(int ammoCount)
    {
        _ammoCountText.text = "Ammo Count : " + ammoCount.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        _LivesImg.sprite = _liveSprites[currentLives];

        if (currentLives == 0)
            GameOverSequence();
    }

    public void GameOverSequence()
    {
        StartCoroutine(GameOverFlickerRoutine());
        _restartText.gameObject.SetActive(true); 
        _quitText.gameObject.SetActive(true);
        _gameManager.GameOver();
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }
}