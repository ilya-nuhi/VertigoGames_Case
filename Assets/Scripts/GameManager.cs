using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class GameManager : MonoSingleton<GameManager>
{
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}