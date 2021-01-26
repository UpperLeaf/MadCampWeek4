using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class HideLight : MonoBehaviour
{
    // Start is called before the first frame update

    Light2D[] lights;

    Camera camera;


    void Start()
    {
        lights = FindObjectsOfType<Light2D>();
        camera = GetComponent<Camera>();
    }

    private void OnPreCull()
    {
        foreach(Light2D light in lights)
        {
            light.enabled = false;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
