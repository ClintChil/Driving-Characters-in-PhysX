using UnityEngine;
using System.Collections;

public class SmokeObject : MonoBehaviour {

    protected float currentScale;
    protected float counter = 0;
    public float maxLife = 2;
    public float growthCutoff = 0.4f;
    public float lerpSpeed = 3;
    public float maxScale = 0.25f;
    public float startScale = 0.02f;
    public float upwardGravity = 2;
    public float dampening = 2;
    protected Vector3 velocity;
    public bool deathOnGroundCollision = true;
	void Awake () {
        transform.localScale = new Vector3(startScale, startScale, startScale);
	}

    public void Setup(Vector3 velocity)
    {
        maxLife *= 0.9f + Random.value * 0.25f;
        this.velocity = velocity;
        currentScale = startScale;
        transform.localScale = new Vector3(startScale, startScale, startScale);
    }
    void Update()
    {
        velocity *= (1 - Time.deltaTime * dampening);
        velocity.y += upwardGravity * Time.deltaTime;
        transform.Translate(velocity * Time.deltaTime);
        //
        counter += Time.deltaTime;
        //
        if (deathOnGroundCollision&& Physics.OverlapSphere(transform.position, Mathf.Clamp( currentScale - 0.06f, 0.01f, 100), 1 << LayerMask.NameToLayer("Ground")).Length > 0)
        {
            Destroy(gameObject);
        }
        else
        {
            //
            if (counter < maxLife * growthCutoff)
            {
                currentScale = Mathf.Lerp(currentScale, maxScale, Time.deltaTime * lerpSpeed);
                transform.localScale = new Vector3(currentScale, currentScale, currentScale);
            }
            else if (counter < maxLife)
            {
                currentScale = Mathf.Lerp(currentScale, 0, Time.deltaTime * lerpSpeed * 1.1f);
                transform.localScale = new Vector3(currentScale, currentScale, currentScale);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
