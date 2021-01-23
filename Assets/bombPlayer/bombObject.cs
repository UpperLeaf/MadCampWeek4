using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class bombObject : MonoBehaviour
{
    private Vector3 onAirVelocity = new Vector3(0,0,0);
    private float init_ydirspeed;

    private float bombSpeed = 5f;

    private float bombheight = 3f;

    private float acceleration;

    public Vector2 startposition;
    public Vector2 Target;

    private Vector3 displacementdir;

    

    // Start is called before the first frame update
    void Start()
    {
        transform.position = startposition;

        float Distance = Vector2.Distance(startposition, Target);
        acceleration = -8 * bombheight * bombSpeed * bombSpeed / (Distance * Distance);

        displacementdir =new Vector3((Target - startposition).normalized.x, (Target - startposition).normalized.y,0) ;

        onAirVelocity.y = 4 * bombSpeed * bombheight / Distance;
        init_ydirspeed = onAirVelocity.y;

    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(Target, transform.position)>0.1f)
        {

            //if (transform.position.y < Target.y)
            //{
            //    transform.position = Target;
            //    GetComponent<Animator>().SetTrigger("blowUp");
            //    return;
            //}
            Debug.Log("movin");
            transform.position += bombSpeed * Time.deltaTime * displacementdir + onAirVelocity*Time.deltaTime;
            onAirVelocity.y += acceleration * Time.deltaTime;

        }
        
        else
        {


            GetComponent<Animator>().SetTrigger("blowUp");
        }


    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

}
