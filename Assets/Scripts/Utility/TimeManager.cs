using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour
{


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (Time.timeScale < 1)
            {
                Time.timeScale = 1f;
                Time.fixedDeltaTime *= 10f;
            }
            else
            {
                Time.timeScale = 0.1f;
                Time.fixedDeltaTime *= 0.1f;
            }
        }
    }
}
