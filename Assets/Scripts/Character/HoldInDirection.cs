using UnityEngine;
using System.Collections;

public class HoldInDirection : MonoBehaviour
{

    new protected Rigidbody rigidbody;
    public bool holdInDirection = true;
    public Vector3 direction = new Vector3(0.6f, 0, 0);
    public Transform relativeTo = null;
    public float force = 10;
    public float holdOffset = 0.45f;
    public float delay = 0;

    public float dampenAngularForce = 0;
    //
    //  public CharacterArms arms;
    //
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    void Start()
    {

    }
    void Update()
    {

    }
    void FixedUpdate()
    {
        if (delay > 0)
        {
            delay -= Time.deltaTime;
            //
        }
        else if (holdInDirection)
        {
            Vector3 holdingDirection = relativeTo == null ? direction : relativeTo.TransformDirection(direction);
            /*
            if (arms != null && arms.IsGrabbingSomething )
            {
                
                rigidbody.AddForceAtPosition(new Vector3(0, (uprightForce) * Time.deltaTime * 3, 0),
                  transform.position + transform.TransformPoint(new Vector3(0, uprightOffset, 0)), ForceMode.Impulse);
                //
                rigidbody.AddForceAtPosition(new Vector3(0, -uprightForce * Time.deltaTime * 3.1f, 0),
                    transform.position + transform.TransformPoint(new Vector3(0, -uprightOffset, 0)), ForceMode.Impulse);
            }
            else
            {
             */
            rigidbody.AddForceAtPosition(holdingDirection * force,
                    transform.TransformPoint(new Vector3(0, holdOffset, 0)), ForceMode.Force);
            //
            rigidbody.AddForceAtPosition(holdingDirection * -force,
                  transform.TransformPoint(new Vector3(0, -holdOffset,0)), ForceMode.Force);
            // }

        }
        if (dampenAngularForce > 0)
        {
            rigidbody.angularVelocity *= (1 - Time.deltaTime * dampenAngularForce);
        }
    }
}
