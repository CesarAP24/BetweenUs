using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsPErsonaje : MonoBehaviour
{
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") !=0){
            anim.SetBool("Walking", true);
        } else {
            anim.SetBool("Walking", false);
        }
    }
}
