using UnityEngine;
using System.Collections;
//
public class EffectsController : MonoBehaviour
{
    //
    public SmokeObject smokePuff;
    // 
    //
    public static EffectsController instance;
    void Awake()
    {
        instance = this;
        MathFL.SetupLookupTables();
    }
    //
    public static void CreateSmokePuffs(int count, Vector3 position, float force, Vector3 baseVelocity, float startOffset)
    {
        if (instance != null)
        {
            float startAngle = Random.value * 3;
            for (int i = 0; i < count; i++)
            {
                Vector2 direction = MathFL.Point2OnCircle(startAngle + Mathf.PI * 2 * (i / (float)count), i % 2 == 0 ? force * (0.8f + Random.value * 0.2f) : force * 0.99f * (0.9f + Random.value * 0.1f));
                SmokeObject smoke = Instantiate(instance.smokePuff, position + new Vector3(direction.x * startOffset, 0, direction.y * startOffset), Quaternion.identity) as SmokeObject;
                smoke.deathOnGroundCollision = false;
                Vector3 rVelocity = Random.onUnitSphere * 0.2f;
                if (rVelocity.y < 0) rVelocity *= -1;
                rVelocity.y += force * 0.0f;
                smoke.Setup(new Vector3(direction.x, 0, direction.y) + rVelocity);
            }
        }
        else
        {
            Debug.LogError("No Effects Controller instance in scene.");
        }
    }
}
