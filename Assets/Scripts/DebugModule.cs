using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class DebugModule : MonoBehaviour
{
    float timeScale;

    public Events events;

    public void DebugMessage(string message)
    {
        Debug.Log(message);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void PauseGame()
    {
        PauseWorld();
        events.onPause.Invoke();
    }

    public void UnPauseGame()
    {
        events.onUnpause.Invoke();
        UnpauseWorld();
    }

    public void PauseWorld()
    {
        timeScale = Time.timeScale;
        Time.timeScale = 0;
    }

    public void UnpauseWorld()
    {
        Time.timeScale = timeScale;
    }

    public void GameOver()
    {
        events.onGameOver.Invoke();
    }

    [System.Serializable]
    public struct Events
    {
        public UnityEvent onPause;
        public UnityEvent onUnpause;
        public UnityEvent onGameOver;
    }
}
