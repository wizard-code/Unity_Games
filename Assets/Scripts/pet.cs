using UnityEngine;

public class PetFollow : MonoBehaviour
{
    public Transform player;
    public float followDistance = 2f;
    public float speed = 5f;
    public float floatHeight = 1.5f;
    public float bobAmount = 0.2f;
    public float bobSpeed = 2f;
    public float turnSpeed = 8f;

    void Update()
    {
        if (player == null) return;

        Vector3 target = player.position;
        target.y += floatHeight + Mathf.Sin(Time.time * bobSpeed) * bobAmount;

        // Stop when within follow distance
        if (Vector3.Distance(transform.position, target) > followDistance)
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // Face the player (flat rotation)
        Vector3 look = player.position - transform.position;
        look.y = 0;
        if (look.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(look), turnSpeed * Time.deltaTime);
    }
}