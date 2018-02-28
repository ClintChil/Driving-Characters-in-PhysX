using UnityEngine;
using System.Collections;

public class CharacterFaceDirection : MonoBehaviour
{

    new protected Rigidbody rigidbody;
    public Vector3 bodyForward = new Vector3(0, 0, 2);
    public Vector3 facingDirection = Vector3.zero;
    public float facingForce = 800;
    //
    public float leadTime = 0; // *** THIS IS USED TO SLOW DOWN WHEN APPROACHING THE DESIRED DIRECTION, INSTEAD OF OVERSHOOTING BACK AND FORTH **
    //
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    //
    void FixedUpdate()
    {
        if (leadTime == 0)
        {
            // ****** JUST PULL WITH TWO STRINGS TO FACE DIRECTION *****
            //
            if (facingDirection != Vector3.zero)
            {
                // *********************  FACE CHEST TOWARDS THE INPUT DIRECTION *******
                //
                rigidbody.AddForceAtPosition(facingForce * facingDirection * Time.deltaTime, rigidbody.transform.TransformDirection(bodyForward), ForceMode.Impulse);
                rigidbody.AddForceAtPosition(-facingForce * facingDirection * Time.deltaTime, rigidbody.transform.TransformDirection(-bodyForward), ForceMode.Impulse);
                //                   
                //                    
            }
        }
        else
        {
            // ******** TRY PULL TOWARDS DIRECTION FACTORING IN VELOCITY (ie. decelerate towards the target) ***********
            //
            Vector3 targetPoint = transform.position + facingDirection * bodyForward.magnitude;
            Vector3 currentPoint = transform.TransformPoint(bodyForward);
            Vector3 reversePoint = transform.TransformPoint(-bodyForward);
            Vector3 velocity = rigidbody.GetPointVelocity(currentPoint);
            //
            Vector3 diff = targetPoint - (currentPoint + velocity * leadTime);
            //
            //
            rigidbody.AddForceAtPosition(facingForce * diff * Time.deltaTime, currentPoint, ForceMode.Impulse);
            rigidbody.AddForceAtPosition(-facingForce * diff * Time.deltaTime, reversePoint, ForceMode.Impulse);
            //  
            //
        }
    }
}
