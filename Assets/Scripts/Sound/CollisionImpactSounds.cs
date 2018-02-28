using UnityEngine;
using System.Collections;

public class CollisionImpactSounds : MonoBehaviour
{

    public AudioClip[] clips;
    //
    public float minVolume = 0.2f;
    public float maxVolume = 0.3f;
    //
    public float minVelocity = 1;
    public float maxVelocity = 10;
    //
    protected float lastImpactTime = 0.5f;
    public float minImpactDelay = 0.15f;
    //
    public float pitch = 1;

    void Start()
    {
        lastImpactTime = Time.time;
    }
    //
    // *************   THIS IS JUST AN EXAMPLE OF A COMMON METHOD OF PLAYING SOUNDS FROM PHYSX ****
    //
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (enabled && Time.time - lastImpactTime > minImpactDelay)
        {
            PlayImpactSound(collision.contacts[0].point, collision.relativeVelocity.magnitude, collision.collider.tag);
            //
            lastImpactTime = Time.time;
        }
    }
    //
    public void PlayImpactSound(Vector3 collisionPoint, float relativeVolocityMagnitude, string tag)
    {
        if (relativeVolocityMagnitude > minVelocity)
        {
            //
            float m = Mathf.Clamp01((relativeVolocityMagnitude - minVelocity) / (maxVelocity - minVelocity));
            float volumeM = minVolume + (maxVolume - minVolume) * m;
            //
            GameObject gO = new GameObject("OneShotAudio");
            gO.transform.position = transform.position;
            AudioSource source = gO.AddComponent<AudioSource>();
            source.loop = false;
            source.dopplerLevel = 0.1f;
            source.volume = volumeM;
            source.pitch = pitch;
            source.rolloffMode = AudioRolloffMode.Linear;
            source.minDistance = 2000;
            source.maxDistance = 2010;
            source.spatialBlend = 0.5f;
            int clipIndex = Mathf.FloorToInt(m * 0.999f * clips.Length);
            if (clipIndex < clips.Length && clipIndex >= 0)
            {
                source.clip = clips[Mathf.FloorToInt(m * 0.999f * clips.Length)]; // **** INDEX 0 to 1 less than number of clips ***
            }
            else
            {
                Debug.LogError("clipIndex " + clipIndex + "  clips.Length " + clips.Length);
            }
            source.Play();
            //
            Destroy(gO, source.clip.length + 0.1f);
            //
        }
    }
}
