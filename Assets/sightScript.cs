using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class sightScript : MonoBehaviour
{

    Vector3 rotate;
    

    // Start is called before the first frame update
    void Start()
    {
        //transform.RotateAroundLocal
        GetComponent<Light2D>().pointLightInnerAngle= 30;
    }

    // Update is called once per frame
    void Update()
    {
        rotate.z = Vector2.SignedAngle(Camera.main.ScreenToWorldPoint(Input.mousePosition)- transform.position, Vector2.up);

        transform.eulerAngles = -rotate;
    }
}
