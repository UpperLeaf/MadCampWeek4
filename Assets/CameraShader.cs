using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraShader : MonoBehaviour
{
    public Camera camera;

    public Shader EffectShader { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        camera.SetReplacementShader (EffectShader, "RenderType");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
