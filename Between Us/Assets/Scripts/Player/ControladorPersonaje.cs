using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorPersonaje : MonoBehaviour
{
    public GameObject relativeCamera;
    private Rigidbody Rb;
    private Transform Transform, cameraTransform;

    public float jumpforce = 1, speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        Transform = gameObject.GetComponent<Transform>();
        cameraTransform = relativeCamera.GetComponent<Transform>();
        Rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.Space)){
            Rb.velocity = new Vector3(Rb.velocity.x,0,Rb.velocity.z);
            Rb.AddForce(Vector3.up*jumpforce, ForceMode.Impulse);
            //Debug.Log("Salto");
        }
    }

    void FixedUpdate(){
        Rb.MovePosition(positionObjective(Transform, cameraTransform, speed, Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));



        
        Rb.MoveRotation(Quaternion.Euler(rotationObjective()));

    }

    Vector3 positionObjective(Transform local, Transform camera, float velocidad, float x_input, float y_input){

        float y_rotation_Camera = camera.localEulerAngles.y*Mathf.Deg2Rad;

        float x_position = local.position.x + (Mathf.Sin(y_rotation_Camera)*y_input + Mathf.Sin(y_rotation_Camera+90*Mathf.Deg2Rad)*x_input)*speed*Time.fixedDeltaTime;
        float y_position = local.position.y;
        float z_position = local.position.z + (Mathf.Cos(y_rotation_Camera)*y_input + Mathf.Cos(y_rotation_Camera+90*Mathf.Deg2Rad)*x_input)*speed*Time.fixedDeltaTime;

        return new Vector3(x_position,y_position,z_position);
    }

    Vector3 rotationObjective(){

        float x_rotation = Transform.localEulerAngles.x;
        float z_rotation = Transform.localEulerAngles.z;

        float y_rotation = Transform.localEulerAngles.y;

        if (Input.GetAxis("Horizontal") > 0){
            if (Input.GetAxis("Vertical") > 0){
                y_rotation = cameraTransform.localEulerAngles.y+45;
            } else if (Input.GetAxis("Vertical") < 0){
                y_rotation = cameraTransform.localEulerAngles.y+90+45;
            } else {
                y_rotation = cameraTransform.localEulerAngles.y + 90;
            }
        } else if (Input.GetAxis("Horizontal") < 0) {
            if (Input.GetAxis("Vertical") > 0){
                y_rotation = cameraTransform.localEulerAngles.y-45;
            } else if (Input.GetAxis("Vertical") < 0){
                y_rotation = cameraTransform.localEulerAngles.y-45-90;
            } else {
                y_rotation = cameraTransform.localEulerAngles.y - 90;
            }
        } else {
            if (Input.GetAxis("Vertical") > 0){
                y_rotation = cameraTransform.localEulerAngles.y;
            } else if (Input.GetAxis("Vertical") < 0){
                y_rotation = cameraTransform.localEulerAngles.y+180;
            }
        }

        return new Vector3(x_rotation, y_rotation, z_rotation);
    }
}
