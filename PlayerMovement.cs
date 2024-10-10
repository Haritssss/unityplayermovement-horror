using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyPlayerController : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float mouseSensitivity = 2f;
    public Transform playerCamera;
    public AudioSource walkAudioSource; // if you want to add walking sfx

    private Rigidbody rb;
    private float xRotation = 0f;
    private bool isMoving = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; 
        Cursor.lockState = CursorLockMode.Locked; // this will lock your cursor

        if (walkAudioSource != null)
        {
            walkAudioSource.loop = true; // loop the walking sound
        }
    }

    void FixedUpdate()
    {
        // Handle Mouse Look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = (transform.right * moveX + transform.forward * moveZ).normalized;
        Vector3 targetVelocity = move * walkSpeed;


        Vector3 velocity = new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.z);
        rb.velocity = velocity;

        // check if player walk
        if (velocity.magnitude > 0.1f && !isMoving)
        {
            // if walk play the sfx
            if (walkAudioSource != null)
            {
                walkAudioSource.Play();
            }
            isMoving = true;
        }
        else if (velocity.magnitude <= 0.1f && isMoving)
        {
            // if stop stop the sfx
            if (walkAudioSource != null)
            {
                walkAudioSource.Stop();
            }
            isMoving = false;
        }
    }
}