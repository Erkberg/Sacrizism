using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFacing : MonoBehaviour
{
    private Vector3 initialScale;
    private Vector3 mirroredScale;

    private Facing currentFacing;

    private void Awake()
    {
        initialScale = transform.localScale;
        mirroredScale = initialScale;
        mirroredScale.x *= -1f;
    }

    public void SetFacing(Facing facing)
    {
        if(facing != currentFacing)
        {
            currentFacing = facing;

            if (facing == Facing.Right)
            {
                transform.localScale = mirroredScale;
            }
            else
            {
                transform.localScale = initialScale;
            }
        }
    }
}

public enum Facing
{
    None,
    Right,
    Left
}