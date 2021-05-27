using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomieMovement : MonoBehaviour
{
    private Homie homie;
    private CharacterController characterController;
    private Collider ledge;
    private float dashSpeed = 5;
    private float dashTime = 5;
    private bool isDashing = false;
    private bool HeadRaycast = false;
    private float gravityValue = -9.81f;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 playerVelocity;
   
    public bool canmove = true;
    public float gravity = 20.0f;
    public float speed;
    public Transform Head;
    public float jumpHeight = 1.0f;
    public Transform cameraRig;
    public GameObject Homie;

    void Start()
    {
    Physics.IgnoreLayerCollision(9, 11);
        homie = gameObject.GetComponent<Homie>();
        characterController = gameObject.GetComponent<CharacterController>();
    }
   


    void Update()
    {
        if (canmove) { Move(); }

        if (isDashing)
        {
        }
        //findledge();

    }

    private void Move()
    {   
        if (characterController.isGrounded)
        {
            if (playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }
            // We are grounded, so recalculate
            // move direction directly from axes
            cameraRig = HomieCamera.Instance.transform;
            Vector3 right = cameraRig.right;
            Vector3 forward = cameraRig.forward;
           
            Vector2 inputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            Vector3 force = ((inputDirection.x * right) + (inputDirection.y * forward));
            force.y = 0;
            moveDirection = force;
            moveDirection *= speed;
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, step);

            if (Input.GetKeyDown("left shift"))
            {
                if (!isDashing)
                {
                StartCoroutine(Dash());
                }
            }
            if(Input.GetKey("left ctrl"))
            {
                characterController.height = 1.15f;
                speed = 3f;

            }
            else
            {
                characterController.height = 2.36f;
                speed = 6f;

            }

            /* sprinting and freelook*/
            if (!Input.GetKey("left alt"))
            {
               transform.rotation = Quaternion.RotateTowards(transform.rotation, cameraRig.transform.rotation, Time.deltaTime * 100000000);
            }
            if (Input.GetKey("left shift"))
            {
                speed = 10f;
            }
            else
            {
                //anim.SetBool("Sprinting", false);
                speed = 6f;
            }
            if (Input.GetButtonDown("Jump"))
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);

            }
        }
        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded && ledge)
        {
            if(Input.GetButtonDown("Jump"))
            {
                StartCoroutine(climbOnLedge(ledge));

            }

        }


            moveDirection.y -= gravity * Time.deltaTime;
        // Move the controller
        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
        characterController.Move(moveDirection * Time.deltaTime);
    }
    public IEnumerator Dash()
    {
        float timeLeft = .35f;

        Vector3 dashDirection = moveDirection;
            while (timeLeft >= 0.0f)
            {
            isDashing = true;
            timeLeft -= Time.deltaTime;
            
            characterController.Move(dashDirection * Time.deltaTime * 3f);
            //yield return new WaitForEndOfFrame ();          
            yield return null; 
            }
        isDashing = false;
    }




    void findledge()
    {
        RaycastHit BodyRaycast;
        int layerMask = 1 << 8;
        float grabdistance = 1.5f;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out BodyRaycast, grabdistance, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * BodyRaycast.distance, Color.yellow);
            if (!this.HeadRaycast)
            {
                ledge = BodyRaycast.collider;
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * grabdistance, Color.white);
            ledge = null;
        }
        RaycastHit HeadRaycast;
        if (Physics.Raycast(Head.position, transform.TransformDirection(Vector3.forward), out HeadRaycast, grabdistance, layerMask))
        {
            Debug.DrawRay(Head.position, transform.TransformDirection(Vector3.forward) * HeadRaycast.distance, Color.yellow);
            this.HeadRaycast = true;
        }
        else
        {
            Debug.DrawRay(Head.position, transform.TransformDirection(Vector3.forward) * grabdistance, Color.white);
            this.HeadRaycast = false;
        }
    }
    public IEnumerator climbOnLedge(Collider ledge)
    {

        Homie.transform.position = new Vector3(Homie.transform.position.x, ledge.transform.position.y+2f, Homie.transform.position.z);
        characterController.Move(moveDirection * Time.deltaTime * 4f);
        yield return new WaitForSeconds(.2f);
        playerVelocity = new Vector3(0,0,0);


        //remember to autosync transforms
    }

}
