using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pull : MonoBehaviour
{
    public Transform target;
    public Transform pullDetectLeft;
    public Transform pullDetectRight;
    public float rayDist;
    

    public Vector3 offset;      //where the box is held
    public float damping;       //for smooth pulling

    
    private Vector3 speed = Vector3.zero;

    

    void FixedUpdate()
    {
        //Casts out a ray that checks for collisions.
        RaycastHit2D checkR = Physics2D.Raycast(pullDetectRight.position, Vector2.right, rayDist);
        RaycastHit2D checkL = Physics2D.Raycast(pullDetectLeft.position, Vector2.right, rayDist);
        if(checkL.collider != null || checkR.collider != null) 
        {
            if(Input.GetKey(KeyCode.Space))     // Spacebar is the button to pull the object with you
            {
                if(checkR.collider != null && checkL.collider == null && checkR.collider.tag =="Player")        //if the player is right side of the box, the box should be held at the left side
                {
                    offset.x = -0.9f;
                    Vector3 movePosition = target.position + offset;
                    transform.position = Vector3.SmoothDamp(transform.position, movePosition, ref speed, damping);
                }
                else if(checkR.collider == null && checkL.collider != null && checkL.collider.tag =="Player")   //if the player is left side of the box, the box should be held at the right side
                {
                    offset.x = 0.9f;
                    Vector3 movePosition = target.position + offset;
                    transform.position = Vector3.SmoothDamp(transform.position, movePosition, ref speed, damping);
                }
                else if(checkR.collider != null && checkL.collider != null && checkL.collider.tag =="Player" && checkR.collider.tag =="Wall")       //if the player pushes the box into the wall that it does not faze through
                {
                    if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
                    {
                        offset.x = -0.9f;
                        Vector3 movePosition = target.position + offset;
                        transform.position = Vector3.SmoothDamp(transform.position, movePosition, ref speed, damping);
                    }
                }
                else if(checkR.collider != null && checkL.collider != null && checkR.collider.tag =="Player" && checkL.collider.tag =="Wall")       //if the player pushes the box into the wall that it does not faze through
                {
                   if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
                    {
                        offset.x = 0.9f;
                        Vector3 movePosition = target.position + offset;
                        transform.position = Vector3.SmoothDamp(transform.position, movePosition, ref speed, damping);
                    } 
                }

               
            }
        }
        
        
    }
        
}

