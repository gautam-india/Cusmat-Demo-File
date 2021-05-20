using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAnimation : MonoBehaviour
{

    Animator animator;
    private VirtualJoystick virtualJoystick;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        virtualJoystick = GameObject.FindGameObjectWithTag("VirtualJoystick").GetComponent<VirtualJoystick>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Vertical", virtualJoystick.Vertical());
        animator.SetFloat("Horizontal", virtualJoystick.Horizontal());
    }
}
