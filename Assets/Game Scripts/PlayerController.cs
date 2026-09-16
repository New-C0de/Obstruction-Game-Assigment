using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private GameObject winText;

    private Rigidbody rb;
    private bool isGrounded = true;
    private bool isGameWon = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Make sure the win message is hidden when the game begins.
        winText.SetActive(false);
    }

    void Update()
    {
        // Stop all gameplay once the player has won.
        if (isGameWon)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        // Automatic forward movement.
        rb.linearVelocity = new Vector3(
            moveSpeed,
            rb.linearVelocity.y,
            rb.linearVelocity.z
        );

        // One-button jump.
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Player failed.
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // Player successfully completed the game.
        if (collision.gameObject.CompareTag("Finish"))
        {
            isGameWon = true;
            moveSpeed = 0f;
            rb.linearVelocity = Vector3.zero;

            winText.SetActive(true);

            Debug.Log("You Win!");
        }
    }
}