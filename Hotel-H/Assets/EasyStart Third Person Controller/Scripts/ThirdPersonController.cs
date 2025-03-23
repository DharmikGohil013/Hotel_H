//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections;
//using System.Collections.Generic;


//public class ThirdPersonController_Mobile : MonoBehaviour
//{
//    public float velocity = 5f;
//    public float sprintAdittion = 3.5f;
//    public float jumpForce = 18f;
//    public float jumpTime = 0.85f;
//    public float gravity = 9.8f;

//    private float jumpElapsedTime = 0;
//    private bool isJumping = false;
//    private bool isSprinting = false;
//    private bool isCrouching = false;

//    //public FloatingJoystick joystick; // Reference to the Joystick UI
//    public Button jumpButton;
//    public Button sprintButton;
//    public Button crouchButton;

//    private CharacterController cc;
//    private Animator animator;

//    void Start()
//    {
//        cc = GetComponent<CharacterController>();
//        animator = GetComponent<Animator>();

//        if (animator == null)
//            Debug.LogWarning("Animator component missing on player!");

//        // Assign button functions
//        jumpButton.onClick.AddListener(Jump);
//        sprintButton.onClick.AddListener(ToggleSprint);
//        crouchButton.onClick.AddListener(ToggleCrouch);
//    }

//    void Update()
//    {
//        HandleMovement();
//        HandleAnimations();
//        HeadHittingDetect();
//    }

//    private void HandleMovement()
//    {
//        float moveX = joystick.Horizontal * (velocity + (isSprinting ? sprintAdittion : 0)) * Time.deltaTime;
//        float moveZ = joystick.Vertical * (velocity + (isSprinting ? sprintAdittion : 0)) * Time.deltaTime;
//        float moveY = -gravity * Time.deltaTime;

//        // Handle Jumping
//        if (isJumping)
//        {
//            moveY = Mathf.SmoothStep(jumpForce, jumpForce * 0.30f, jumpElapsedTime / jumpTime) * Time.deltaTime;
//            jumpElapsedTime += Time.deltaTime;
//            if (jumpElapsedTime >= jumpTime)
//            {
//                isJumping = false;
//                jumpElapsedTime = 0;
//            }
//        }

//        Vector3 moveDirection = new Vector3(moveX, moveY, moveZ);
//        moveDirection = Camera.main.transform.TransformDirection(moveDirection);
//        moveDirection.y = moveY;

//        cc.Move(moveDirection);
//    }

//    private void HandleAnimations()
//    {
//        if (animator != null)
//        {
//            animator.SetBool("crouch", isCrouching);
//            animator.SetBool("run", cc.velocity.magnitude > 0.9f);
//            animator.SetBool("sprint", isSprinting);
//            animator.SetBool("air", !cc.isGrounded);
//        }
//    }

//    public void Jump()
//    {
//        if (cc.isGrounded)
//        {
//            isJumping = true;
//        }
//    }

//    public void ToggleSprint()
//    {
//        isSprinting = !isSprinting;
//    }

//    public void ToggleCrouch()
//    {
//        isCrouching = !isCrouching;
//    }

//    private void HeadHittingDetect()
//    {
//        float headHitDistance = 1.1f;
//        Vector3 ccCenter = transform.TransformPoint(cc.center);
//        float hitCalc = cc.height / 2f * headHitDistance;

//        if (Physics.Raycast(ccCenter, Vector3.up, hitCalc))
//        {
//            jumpElapsedTime = 0;
//            isJumping = false;
//        }
//    }
//}
