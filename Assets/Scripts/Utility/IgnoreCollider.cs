using UnityEngine;
using System.Collections;

public class IgnoreCollider : MonoBehaviour
{

    public Collider ignoreCollider;
    void Awake()
    {
        Collider thisCollider = GetComponentInChildren<Collider>();
        Physics.IgnoreCollision(thisCollider, ignoreCollider);
    }

}
