using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target_obj;
    private Transform target_pos, camera_pos;

    public float distance = 8, min_distance = 6, max_distance = 20, zoom_speed = 1;
    public float angle_x = -180, angle_y = 30, x_sensibility = 150, y_sensibility = -150;
    public float max_angle_aperture;

    void Start()
    {
        camera_pos = gameObject.GetComponent<Transform>();
        target_pos = target_obj.GetComponent<Transform>();

        //Bloquear mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        //actualiza los valores de la camara
        camera_pos.position = positionObjective(target_pos, camera_pos, distance, angle_x, angle_y);
        camera_pos.eulerAngles = rotationObjective(target_pos, camera_pos);
 
        //mover camara       
        angle_x += Input.GetAxis("Mouse X")*Time.deltaTime*x_sensibility;
        angle_y += Input.GetAxis("Mouse Y")*Time.deltaTime*y_sensibility;


        //Actualizar zoom
        if (distance <= min_distance){
            distance = min_distance;
            if (Input.GetAxis("Mouse ScrollWheel") <= 0){
                distance -= Input.GetAxis("Mouse ScrollWheel")*zoom_speed*Time.deltaTime;
            }
        } else if (distance >= max_distance){
            distance = max_distance;
            if (Input.GetAxis("Mouse ScrollWheel") >= 0){
                distance -= Input.GetAxis("Mouse ScrollWheel")*zoom_speed*Time.deltaTime;
            }
        } else {
            distance -= Input.GetAxis("Mouse ScrollWheel")*zoom_speed*Time.deltaTime;
        }
    }

    //devuelve un vector con x y z de rotacion
    Vector3 rotationObjective(Transform target, Transform origin){

        float x_rotation=0, y_rotation=0;

        float x_diference = origin.position.x-target.position.x;
        float y_diference = origin.position.y-target.position.y;
        float z_diference = origin.position.z-target.position.z;

        float x_z_distance = Mathf.Sqrt(z_diference*z_diference+x_diference*x_diference);

        if (target.position.z < origin.position.z) {

            x_rotation = Mathf.Rad2Deg*Mathf.Atan(y_diference/x_z_distance);
            y_rotation = 180+Mathf.Rad2Deg*Mathf.Atan(x_diference/z_diference);

        } else if (target.position.z > origin.position.z) {

            x_rotation = Mathf.Rad2Deg*Mathf.Atan(y_diference/x_z_distance);
            y_rotation = Mathf.Rad2Deg*Mathf.Atan(x_diference/z_diference);

        } else if (z_diference == 0){

            if (target.position.x > origin.position.x){
                y_rotation = 90;

            } else if (target.position.x < origin.position.x){
                y_rotation = -90;
            } 

            if (x_diference == 0) {
                if (target.position.y > origin.position.y) {
                    x_rotation = -90;
                } else if (target.position.y < origin.position.y) {
                    x_rotation = 90;
                } 
            } else {
                x_rotation = Mathf.Rad2Deg*Mathf.Atan(y_diference/x_z_distance);
            }

        }


        return new Vector3(x_rotation, y_rotation, 0);
    }

    //devuelve un vector con x y z de posision
    Vector3 positionObjective(Transform target, Transform origin, float offset, float angulo_x, float angulo_y){

        angulo_x = angulo_x*Mathf.Deg2Rad;
        angulo_y = angulo_y*Mathf.Deg2Rad;

        if (Mathf.Abs(angle_y) >= max_angle_aperture/2){
            if (camera_pos.position.y > target_pos.position.y){
                angle_y = max_angle_aperture/2;
                angulo_y = max_angle_aperture/2*Mathf.Deg2Rad;
            } else {
                angle_y = -max_angle_aperture/2;
                angulo_y = -max_angle_aperture/2*Mathf.Deg2Rad;
            }
        }

        float x_out = target.position.x + Mathf.Sin(angulo_x)*offset*Mathf.Cos(angulo_y);
        float y_out = target.position.y + Mathf.Sin(angulo_y)*offset;
        float z_out = target.position.z + Mathf.Cos(angulo_x)*offset*Mathf.Cos(angulo_y);


        RaycastHit hit;

        if (Physics.Raycast(target.position, new Vector3(x_out, y_out, z_out)-target.position, out hit) && hit.distance <= offset){
            //Debug.Log("Colision: " + hit.point);
            Debug.DrawRay(target.position, new Vector3(x_out, y_out, z_out)-target.position, Color.green);

            return hit.point;
            
        } else {
            Debug.DrawRay(target.position, new Vector3(x_out, y_out, z_out)-target.position, Color.red);
        }

        //Debug.Log("Camera: " + new Vector3(x_out, y_out, z_out));
        return new Vector3(x_out, y_out, z_out);
    }
}
