using UnityEngine;

public class pet : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(1.5f, 1.5f, -1.5f); // right, up, behind
    public float followSmooth = 0.2f;
    public float bobHeight = 0.2f;
    public float bobSpeed = 2f;

    Vector3 velocity;

    void LateUpdate()
    {
        // offset relative to the player's facing direction
        if (player == null) return;
        Vector3 target = player.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, followSmooth);
    }
}
