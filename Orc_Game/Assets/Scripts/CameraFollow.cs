using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
   
    // Update is called once per frame
    // However it's usually considered best practice to do Camera Movement after everything else in the Scene was updated.
    void LateUpdate() {
        // Afterwards we will simply use the Update function to always set the Camera's X position to the target's X position
        // Note: the X position is the horizontal position, while the Y position is our vertical position.
        if (target == null) return;
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);

        // Note: We check to see if the target variable (which is a Transform) is null. If it evaluates to true, we tell Unity to stop processing the LateUpdate function. 
        // Otherwise, we continue execution of the LateUpdate function. By doing this, known as null checking, you can avoid a flood of errors in the Unity Console 
        // complaining that something wasn't set correctly.
    }
}
