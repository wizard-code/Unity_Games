using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;

    void Start()
    {
        if (player == null) return;
        offset = transform.position - player.transform.position;
    }
    void LateUpdate()
    {
        if (player == null) return;
        transform.position = player.transform.position + offset;
    }
}