using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorPersonaje : MonoBehaviour
{
    //Velocidades
    public float speed=5, rotationSpeed=1;

    //Variables
    public Transform Transform;


    void Start()
    {
        Transform = gameObject.GetComponent<Transform>();
    }


    void Update()
    {
        //mover personaje
        moverse(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), speed*Time.deltaTime);

        //rotar personaje
        Transform.Rotate(new Vector3(0,rotationSpeed*Input.GetAxis("Mouse X"),0), Space.Self);
    }

    void moverse(float x, float y, float speed){

        float Horizontal = speed*Input.GetAxis("Horizontal");
        float Vertical = speed*Input.GetAxis("Vertical");

        if ((y==0) || (x==0)) {
            Transform.Translate(new Vector3(Horizontal,0,Vertical));
        } else {
            Transform.Translate(new Vector3(Horizontal/1.4142f,0,Vertical/1.4142f));
        }
    }
}
