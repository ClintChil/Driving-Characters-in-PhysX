using UnityEngine;
using System.Collections;

public class ClickAndPull : MonoBehaviour
{
    protected Rigidbody selectedBody;
    protected CharacterMaintainHeight maintainHeight;
    //
    //
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.collider.GetComponent<Rigidbody>() != null)
                {
                    maintainHeight = hit.collider.gameObject.AddComponent<CharacterMaintainHeight>();
                    maintainHeight.desiredHeight = 1.4f;
                    maintainHeight.pullUpForce = 100;
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (maintainHeight != null)
            {
                Destroy(maintainHeight);
            }

        }
    }
}
