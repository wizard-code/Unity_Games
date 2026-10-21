using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseText; // drag PauseEvent here
    bool paused;

    void Start()
    {
        pauseText.SetActive(false); // hidden at start
    }

    void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            paused = !paused;
            Time.timeScale = paused ? 0f : 1f;
            pauseText.SetActive(paused);
        }
    }
}
