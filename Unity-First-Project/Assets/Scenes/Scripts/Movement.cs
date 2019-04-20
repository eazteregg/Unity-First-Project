using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    private CharacterController characterController;

    private Vector3 forward;
    private Vector3 right;
    private Vector3 moveDirection;

    private Transform cameraTransform;

    public float horizontalTurnSpeed = 2.0f;
    public float verticalTurnSpeed = 2.0f;


    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = transform.GetChild(0);

        forward = transform.forward;
        right = transform.right;


        Debug.Log(forward);
        Debug.Log(right);
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Mouse X") * horizontalTurnSpeed;
        float v = Input.GetAxis("Mouse Y") * verticalTurnSpeed;

        forward = transform.forward;
        right = transform.right;

        transform.Rotate(0, h, 0);
        cameraTransform.Rotate(-v, 0, 0);
        
        if (characterController.isGrounded)
        {
            float directHoriz = Input.GetAxis("Horizontal");
            float directVert = Input.GetAxis("Vertical");

            moveDirection = (forward * directVert) + (right * directHoriz);
            moveDirection.Normalize();
            moveDirection *= speed;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;

        characterController.Move(moveDirection * Time.deltaTime);
    }
}
