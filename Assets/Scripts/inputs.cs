using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
public class PlayerController : MonoBehaviour
{
    public TextMeshProUGUI dashText;
    public float dashSpeed = 20f;
    public float dashTime = 0.15f;
    public int dashesLeft = 3;
    private float dashTimer;
    // Rigidbody of the player.
    private Rigidbody rb;

    // Variable to keep track of collected "PickUp" objects.
    private int count;

    // Movement along X and Y axes.
    private float movementX;
    private float movementY;
    private float jumpForce = 4f;
    private Boolean isGrounded = true;

    // Speed at which the player moves.
    private float speed = 8;
    public GameObject pauseText;

    // UI text component to display count of "PickUp" objects collected.
    public TextMeshProUGUI countText;

    // UI object to display winning text.
    public GameObject winTextObject;

    public GameObject door1;
    public float killY = -10f;
    bool paused;

    // Start is called before the first frame update.
    void Start()
    {
        dashText.text = "Dashes: " + dashesLeft;
        // Get and store the Rigidbody component attached to the player.
        rb = GetComponent<Rigidbody>();

        // Initialize count to zero.
        count = 0;

        // Update the count display.
        SetCountText();

        // Initially set the win text to be inactive.
        winTextObject.SetActive(false);
        pauseText.SetActive(false);

    }

    // This function is called when a move input is detected.
    void OnMove(InputValue movementValue)
    {
        // Convert the input value into a Vector2 for movement.
        Vector2 movementVector = movementValue.Get<Vector2>();

        // Store the X and Y components of the movement.
        movementX = movementVector.x;
        movementY = movementVector.y;
    }
    void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            paused = !paused;
            Time.timeScale = paused ? 0f : 1f;
            pauseText.SetActive(paused);
        }

        if (paused) return; // no jumping/dashing while paused

        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }


        if (Keyboard.current.leftShiftKey.wasPressedThisFrame && dashesLeft > 0 && dashTimer <= 0)
        {
            dashesLeft--;
            dashTimer = dashTime;
            dashText.text = "Dashes: " + dashesLeft;
        }
        if (transform.position.y < killY)
        {
            Destroy(gameObject);
            winTextObject.gameObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
        }

    }

    // FixedUpdate is called once per fixed frame-rate frame.
    void FixedUpdate()
    {
        Vector3 v = rb.linearVelocity;
        rb.linearVelocity = new Vector3(movementX * speed, v.y, movementY * speed);

        // Face the direction of movement
        Vector3 dir = new Vector3(movementX, 0f, movementY);
        if (dir.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.LookRotation(dir);
        if (dashTimer > 0)
        {
            dashTimer -= Time.fixedDeltaTime;
            rb.linearVelocity = transform.forward * dashSpeed;
            return;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        // Check if the object the player collided with has the "PickUp" tag.
        if (other.gameObject.CompareTag("Pickup"))
        {
            // Deactivate the collided object (making it disappear).
            other.gameObject.SetActive(false);

            // Increment the count of "PickUp" objects collected.
            count = count + 1;

            OpenDoors();

            // Update the count display.
            SetCountText();
        }
    }
    void OpenDoors()
    {
        if (count >= 2)
        {
            door1.SetActive(false);

        }

    }



    // Function to update the displayed count of "PickUp" objects collected.
    void SetCountText()
    {
        // Update the count text with the current count.
        countText.text = "Count: " + count.ToString();

        // Check if the count has reached or exceeded the win condition.
        if (count >= 7)
        {
            // Display the win text.
            winTextObject.SetActive(true);

            // Destroy the enemy GameObject.
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            winTextObject.gameObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
        }
        else
        {
            isGrounded = true;
        }
        if (collision.gameObject.CompareTag("Spinner")) { 
            transform.SetParent(collision.transform);
    }
}

void OnCollisionExit(Collision collision)
{
    if (collision.gameObject.CompareTag("Spinner"))
        transform.SetParent(null);
}


}