using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string sceneName;
    public TextMeshProUGUI messageText;
    public int required = 7;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerController pc = other.GetComponent<PlayerController>();
        if (pc == null) return;

        if (pc.Count >= required)
        {
            pc.SaveState();
            SceneManager.LoadScene(sceneName);
        }

    }



}