﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Google VR Walking Character Controller
    By Sam Halperin

    This script allows the user to walk around a VR scene
    by swiping over the control surface of the GVR remote
    as if it were a d-pad.

    To get started, attach this script to the Player object
    in a working GVR project, 
    (see: https://developers.google.com/vr/develop/unity/controller-support)
    or start with the GVR HelloVR demo scene.

    See also:
    * Gvr reference docs:
    * https://developers.google.com/vr/reference/unity/class/GvrControllerInput
 */

public class GvrWalkingCharacterController : MonoBehaviour
{
    string logtag = "GvrWalk";
    public float speed = 3.0f;
    public float rotateSpeed = 3.0f;

    GvrControllerInputDevice device = null;
    CharacterController cc;


    void Start()
    {
        /* Character Controller Boilerplate */
        cc = GetComponent<CharacterController>();

        /* Latch the GVRControllerInputDevice */
        //TODO: If this fails, is there feedback to the user, and
        // is there a retry?  Is there a onConnected callback somewhere?
        device = GvrControllerInput.GetDevice(GvrControllerHand.Dominant);
        if (device == null)
        {
            Debug.LogError($"{logtag} GvrControllerInputDevice is null");
        }
        else
        {
            Debug.LogError($"{logtag} Latched GvrControllerInputDevice.");
        }
    }

    void Update()
    {
        /* 
         * input (a vector2 representing the touchpad surface, used
         * to drive the walking animation.       
         * when not touching control surface : (0,0)
         * when touching: Normalized vector with xpos of 1 = full forward.
         *
         */
        Vector2 input = new Vector2(0, 0);
        if (device != null)
        {

            if (device.GetButton(GvrControllerButton.TouchPadTouch))
            {
                input = device.TouchPos;
            }
            else { input.Set(0, 0);}
        }

        /*
         * Now drive the player with that input.
         */
        transform.Rotate(0, input.x * rotateSpeed, 0);
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        float curSpeed = speed * input.y;
        transform.position += transform.forward * Time.deltaTime * input.y * speed;

    }

}