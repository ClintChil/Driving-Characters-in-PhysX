using UnityEngine;
using System.Collections;

public class ShaderHelper : MonoBehaviour
{
    public Color ShadowTint;
    public float ShadowClip;
    public bool UpdateEveryFrame = false;

    public void UpdateShaderGlobals()
    {
        Shader.SetGlobalColor("_ShadowCol", ShadowTint);
        Shader.SetGlobalFloat("_ShadowClip", ShadowClip);
    }

	// Use this for initialization
	void Start ()
	{
	    UpdateShaderGlobals();
	}

    void Update()
    {
        if (UpdateEveryFrame)
        {
            UpdateShaderGlobals();
        }
    }
}
