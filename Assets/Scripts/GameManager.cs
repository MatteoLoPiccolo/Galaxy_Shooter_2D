using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _isGameOver;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver)
            SceneManager.LoadScene("Game");

        if (Input.GetKeyDown(KeyCode.Escape) && _isGameOver)
        {
            Application.Quit();
            Debug.Log("You quit the Application!");
        }
    }

    public void GameOver()
    {
        _isGameOver = true;
    }
}