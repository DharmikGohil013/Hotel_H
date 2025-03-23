using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of movement
    private Vector2 touchStartPos;
    private Vector2 touchEndPos;
    private Vector3 moveDirection;
    private Animator animator; // Reference to Animator

    void Start()
    {
        animator = GetComponent<Animator>(); // Get Animator component
    }

    void Update()
    {
        HandleTouchInput();
    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
                animator.SetBool("isRunning", true); // ✅ Play Slow Run animation immediately
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                touchEndPos = touch.position;
                Vector2 direction = touchEndPos - touchStartPos;

                direction.Normalize();
                moveDirection = new Vector3(direction.x, 0, direction.y); // Convert to 3D
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                moveDirection = Vector3.zero; // Stop moving when touch is released
                animator.SetBool("isRunning", false); // ✅ Return to Idle animation
            }
        }

        MovePlayer();
    }

    void MovePlayer()
    {
        if (moveDirection != Vector3.zero) // Rotate only if moving
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * 10f);
        }

        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}
