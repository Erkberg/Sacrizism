using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb2D;
    public Animator animator;
    public Animator shadowAnimator;
    public PlayerPowerUps playerPowerUps;

    public float moveSpeed = 1f;
    private bool isMoving = false;

    public bool movementEnabled = true;
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if(movementEnabled)
        {
            Vector2 movement = new Vector2(Input.GetAxis(InputConsts.HorizontalMovementAxis), Input.GetAxis(InputConsts.VerticalMovementAxis));

            while(movement.magnitude > 1f)
            {
                movement *= 0.99f;
            }

            //Debug.Log(movement.magnitude);

            rb2D.velocity = movement * (moveSpeed + playerPowerUps.bonusMoveSpeed);

            CheckMovingAnimation();
        }
	}

    public void EnableMovement(bool enabled)
    {
        movementEnabled = enabled;

        if(!enabled)
        {
            rb2D.velocity = Vector2.zero;
            CheckMovingAnimation();
        }
    }

    private void CheckMovingAnimation()
    {
        if (rb2D.velocity.sqrMagnitude < 1f)
        {
            rb2D.velocity = Vector2.zero;
        }

        if (rb2D.velocity == Vector2.zero)
        {
            SetMoving(false);
        }
        else
        {
            SetMoving(true);
        }
    }

    private void SetMoving(bool moving)
    {
        if (moving == isMoving)
        {
            return;
        }

        isMoving = moving;

        animator.SetBool(AnimationBools.MovingBool, moving);
        shadowAnimator.SetBool(AnimationBools.MovingBool, moving);
    }
}
