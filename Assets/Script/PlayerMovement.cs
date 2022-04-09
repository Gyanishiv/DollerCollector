using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;                                             //6

public class PlayerMovement : MonoBehaviour
{
   //veriables
   public TextMeshProUGUI countText;                     //6
   public TextMeshProUGUI winText;                     //6.1

   [SerializeField] private float moveSpeed;              //1
   [SerializeField] private float walkSpeed;              //1
   [SerializeField] private float runSpeed;               //1

   private Vector3 moveDirection;                         //1
   private Vector3 velocity;                              //2
   private int count;                                     //5


   [SerializeField] private bool isGrounded;              //2
   [SerializeField] private float groundCheckDistance;    //2
   [SerializeField] private LayerMask groundMask;         //2
   [SerializeField] private float gravity;                //2

   [SerializeField] private float jumpHeight;
   
   //reference
   private CharacterController controller;                //1
   private Animator anim;

   private void Start()                                   //1
   {
       controller = GetComponent<CharacterController>();  //1
       anim  = GetComponentInChildren<Animator>(); 
       count = 0;                                         //4
    /* if (moveDirection = !Vector3.zero)                 //6
        {
          introText.gameObject.SetActive(true);
        } */
   }

   private void Update()                                 //1
   {
       Move();                                           //1
   }

   private void Move()                                   //1
    {
    isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask); //2
          if (isGrounded && velocity.y < 0)
             {
              velocity.y = -2f;                                                           //2.1
             }
    float moveZ = Input.GetAxis("Vertical");            //1 
   
    moveDirection = new Vector3(0,0,moveZ);             //1 started moving up-down by w and x
    moveDirection = transform.TransformDirection(moveDirection);
            if (isGrounded)                         //2.2
            {
               if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))  //1.1
               {
                   Walk();
               }
               else if(moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift)) //1.1
               {
                  Run();
               }
               else if(moveDirection == Vector3.zero)                                   //1.1                              //1.1
               {
                   Idle();
               }
             moveDirection *= moveSpeed;              //1.1
     
               if(Input.GetKeyDown(KeyCode.Space)) 
               {
                   Jump();
               }
            }
   
    controller.Move(moveDirection*Time.deltaTime);   //1.1 
   
    velocity.y += gravity * Time.deltaTime;          //2.1
    controller.Move(velocity * Time.deltaTime);      //2.1
    }

   private void Idle()                              //1.1
   {
     anim.SetFloat("Speed",0, 0.03f,Time.deltaTime); 
   }

   private void Walk()                              //1.1
   {
       moveSpeed = walkSpeed;
       anim.SetFloat("Speed", 0.5f, 0.1f,Time.deltaTime); 
   }

   private void Run()                              //1.1
   {
       moveSpeed = runSpeed;
       anim.SetFloat("Speed", 1, 0.1f,Time.deltaTime);
   }

   private void Jump () 
   {
       velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
   }

   private void OnTriggerEnter(Collider other)         //4
   {   if(other.gameObject.CompareTag("Money"))
        {
        other.gameObject.SetActive(false);             
        count = count + 100;                           //5 
        countText.text = "Money - " + count.ToString(); // 6
           if(count >= 1400)
           {
               winText.gameObject.SetActive(true);
           }                                            
        }
   }
}
