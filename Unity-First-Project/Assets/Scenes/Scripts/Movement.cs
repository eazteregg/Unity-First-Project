using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float horizontalInfluence = 1.0f;
    public float jumpCooldownMAX = .3f;
    public float maxWalljumpFallSpeed = -5.0f;
    private float jumpCooldown = 0.0f;


    private CharacterController characterController;
    private Collider collider;

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
        collider = GetComponent<BoxCollider>();

        forward = transform.forward;
        right = transform.right;


        //Debug.Log(forward);
        //Debug.Log(right);
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

            if (Input.GetButton("Jump") && jumpCooldown <= 0.0f)
            {
                moveDirection.y = jumpSpeed;
                jumpCooldown = jumpCooldownMAX;
            }
        }
        if (jumpCooldown > 0.0f)
            {
                jumpCooldown -= 1 * Time.deltaTime;
            }
        

        moveDirection.y -= gravity * Time.deltaTime;

        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
       // Debug.Log("Touching stuff", other);
        if (other.gameObject.tag == "wall")
        {
         //   Debug.Log("Touching wall", other);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "wall")
        {
            //Debug.Log("Touching wall");

            if (!characterController.isGrounded && Input.GetButton("Jump") && jumpCooldown <= 0.0f)
            {


                
                //apply vertical movement
                if (moveDirection.y > maxWalljumpFallSpeed)
                {

                    Debug.Log(moveDirection);
                    Vector3 horizontalMovement = new Vector3(moveDirection.x, 0, moveDirection.z);
                    Debug.Log("horizontal movement" + horizontalMovement);
                    Vector3 horizontalMovementReflected = Vector3.Reflect(horizontalMovement, hit.normal);
                    Debug.Log("Movement reflected" + horizontalMovementReflected);

                    moveDirection = new Vector3(horizontalMovementReflected.x, moveDirection.y, horizontalMovementReflected.z);
                    Debug.Log("Inverted moveDirection" + moveDirection);

                    if (moveDirection.y <= 0.0f)
                    {
                        moveDirection.y = jumpSpeed;
                    }
                    else
                    {
                        moveDirection.y += jumpSpeed;
                    }
                    
                    jumpCooldown = jumpCooldownMAX;
                    moveDirection.y -= gravity * Time.deltaTime;

                    characterController.Move(moveDirection * Time.deltaTime);
                    Debug.Log("final" + moveDirection);
                }
                

                
            }

        }
    }
    void onTriggerStay(Collider other)
    {

        Debug.Log("Touching stuff");
        if (other.gameObject.tag == "wall")
            
        {
            Debug.Log("Touching Wall");

           
        }
    }
}
