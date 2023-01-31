using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] Transform cameraTarget;
    [SerializeField] float sensitivity = 8;
    float pitch;
    float yaw;
    public float dstFromTarget;

    [SerializeField] Transform cameraTransform;
    [SerializeField] Rigidbody rb;
    [SerializeField] float walkSpeed = 5;
    [SerializeField] float runSpeed = 12;
    float velocityY;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Rotation
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch = Mathf.Clamp(pitch, -90, 90);
        cameraTransform.localEulerAngles = new Vector3(pitch, 0, 0);
        transform.eulerAngles = new Vector3(0, yaw, 0);

        if (Input.mouseScrollDelta.y > 0)
        {
            dstFromTarget -= 1;
            if(dstFromTarget <= 0)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            dstFromTarget += 1;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        dstFromTarget = Mathf.Clamp(dstFromTarget, 0, 20);
        if (Physics.Raycast(cameraTarget.position, -cameraTransform.forward, out RaycastHit hit, dstFromTarget))
        {
            cameraTransform.position = hit.point;
        }
        else
        {
            cameraTransform.position = cameraTarget.position - cameraTransform.forward * dstFromTarget;
        }

        // Movement
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);
        if (velocityY < 0 && isGrounded)
        {
            velocityY = 0;
        }
        else
        {
            velocityY -= 10 * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocityY += 5;
        }

        float horizontal;
        float vertical;
        if(Input.GetKey(KeyCode.LeftShift))
        {
            horizontal = Input.GetAxisRaw("Horizontal") * runSpeed;
            vertical = Input.GetAxisRaw("Vertical") * runSpeed;
        }
        else
        {
            horizontal = Input.GetAxisRaw("Horizontal") * walkSpeed;
            vertical = Input.GetAxisRaw("Vertical") * walkSpeed;
        }

        rb.velocity = transform.right * horizontal + transform.forward * vertical + Vector3.up * velocityY;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Ball") && collision.transform.TryGetComponent<Rigidbody>(out var otherRB))
        {
            float magnitude = Input.GetKey(KeyCode.LeftShift) ? 20 : rb.velocity.magnitude;
            Vector3 force = (collision.transform.position - transform.position).normalized;
            force.y = 0.25f;
            otherRB.AddForceAtPosition(force * magnitude, transform.position, ForceMode.Impulse);
        }
    }
}
