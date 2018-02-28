using UnityEngine;
using System.Collections;

public class SoccerBall : MonoBehaviour
{

    public float velocityCutOff = 4;
    public float extraUpwardForce = 4;
    public float extraSideForce = 5;//
    // 
    public float multiplyHorizontalVelocity = 1;
    //
    public float smokeCollisionSpeedCutOff = 7;
    public float smokeCollisionOffset = 0.1f;
    public int smokeCollisionPuffCount = 8;
    public float rotationSpeedAtStart = 0;
    //  
    //
    new protected Rigidbody rigidbody;
    //
    protected float lastSmokeTime = 0;
    //
    protected void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddRelativeTorque(Random.onUnitSphere * rotationSpeedAtStart, ForceMode.VelocityChange);
        // 
    }
    //
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Ground"))
        {
            //
            // **** USE HIT VELOCITY ****
            //
            Vector3 velocityPlaned = collision.relativeVelocity;
            velocityPlaned.y = 0;
            //
            if (velocityPlaned.magnitude > velocityCutOff)
            {
                velocityPlaned *= multiplyHorizontalVelocity;
                //
                GetComponent<Rigidbody>().AddForce(Vector3.up * extraUpwardForce + extraSideForce * velocityPlaned.normalized, ForceMode.Impulse);
                Rigidbody otherBody = gameObject.GetComponent<Rigidbody>();
                //
                if (otherBody != null)
                {
                    otherBody.AddForce(extraSideForce * -0.75f * velocityPlaned.normalized, ForceMode.Impulse);
                }
                //
            }
            //
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (collision.contacts[0].normal.y > 0.6f && collision.relativeVelocity.y > smokeCollisionSpeedCutOff && Time.time - lastSmokeTime > 0.1f)
            {
                lastSmokeTime = Time.time;
                EffectsController.CreateSmokePuffs(smokeCollisionPuffCount + Random.Range(0, smokeCollisionPuffCount / 2), new Vector3(transform.position.x, collision.contacts[0].point.y, transform.position.z), 3, Vector3.up, smokeCollisionOffset);
            }
        }
    }
}
