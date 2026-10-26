using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
public class PlayerController : MonoBehaviour
{
    public TextMeshProUGUI dashText;
    public TextMeshProUGUI lives;
    public GameObject portal;
    private AudioSource audioSource;
    public AudioClip pickupSound;
    public float dashSpeed = 20f;
    public float dashTime = 0.15f;
    public int dashesLeft = 3;

    public int livescount = 3;
    private float dashTimer;
    // Rigidbody of the player.
    private Rigidbody rb;

    // Variable to keep track of collected "PickUp" objects.
    private int count;
    public int Count => count;
    // Movement along X and Y axes.
    private float movementX;
    private float movementY;
    private float jumpSpeed = 4f;
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
    static bool hasSavedState;
    static int savedDashes, savedLives, savedCount;

    public void SaveState()
    {
        hasSavedState = true;
        savedDashes = dashesLeft;
        savedLives = livescount;
        savedCount = count;
    }
    // Start is called before the first frame update.
    void Start()
    {
        if (hasSavedState)
        {
            dashesLeft = savedDashes;
            livescount = savedLives;
            count = savedCount;
        }
        else
        {
            count = 0;
        }

        dashText.text = "Dashes: " + dashesLeft;
        lives.text = "Lives: " + livescount;
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        if (portal != null) portal.SetActive(false);
        dashText.text = "Dashes: " + dashesLeft;
        lives.text = "Lives: " + livescount;

        // Initialize count to zero.
       

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
            Vector3 v = rb.linearVelocity;
            rb.linearVelocity = new Vector3(v.x, jumpSpeed, v.z);
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
            LoseLife();
        }
        if (livescount <= 0)
        {
            Destroy(gameObject);
            winTextObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
        }

    }

    // FixedUpdate is called once per fixed frame-rate frame.
    void FixedUpdate()
    {
        Vector3 input = new Vector3(movementX, 0f, movementY);

        if (input.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.LookRotation(input);

        if (dashTimer > 0)
        {
            dashTimer -= Time.fixedDeltaTime;
            Vector3 d = transform.forward * dashSpeed;
            rb.linearVelocity = new Vector3(d.x, rb.linearVelocity.y, d.z);
            return;
        }

        Vector3 move = Vector3.ClampMagnitude(input, 1f) * speed;
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);
    }


    void OnTriggerEnter(Collider other)
    {
        // Check if the object the player collided with has the "PickUp" tag.
        if (other.gameObject.CompareTag("Pickup"))
        {
            if (audioSource != null && pickupSound != null)
            {
                audioSource.PlayOneShot(pickupSound);
            }
            other.gameObject.SetActive(false);
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
        if (door1 != null && count >= 2)
        {
            door1.SetActive(false);
        }

    }



    // Function to update the displayed count of "PickUp" objects collected.
    void SetCountText()
    {
        // Update the count text with the current count.
        countText.text = "Count: " + count.ToString();

        if (door1 != null && count >= 2)
        {
            portal.SetActive(true);
        }

        // Check if the count has reached or exceeded the win condition.
        if (count >= 12)
        {
            // Display the win text.
            winTextObject.SetActive(true);

            // Destroy the enemy GameObject.
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
        }
    }
    void LoseLife()
    {
        livescount--;
        lives.text = "Lives: " + livescount;

        
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.SetParent(null);              
        transform.position = new Vector3(0, 1, 0);  
        dashTimer = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            LoseLife();
        }
        else
        {
            isGrounded = true;
        }
        if (collision.gameObject.CompareTag("Spinner"))
            transform.SetParent(collision.transform);
    }

void OnCollisionExit(Collision collision)
{
    if (collision.gameObject.CompareTag("Spinner"))
        transform.SetParent(null);
}


}