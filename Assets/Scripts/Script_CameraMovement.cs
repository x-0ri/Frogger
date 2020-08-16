using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Script_CameraMovement : MonoBehaviour
{
    public Camera MainCamera;
    Vector3 DefaultCameraPosition = new Vector3(0, 0, -20);
    Vector3 MoveCameraUp = new Vector3(0, 0.02F, 0);
    public int AllExtraLanes;
    void Start()
    {
        MainCamera.transform.position = DefaultCameraPosition;
        AllExtraLanes = Settings.ExtraLanes[0] + Settings.ExtraLanes[1] + Settings.ExtraLanes[2];   // to find the limit of camera movement
    }

    public IEnumerator Event_CameraMovementUp()                         // is called only when player reaches certain trigger!
    {
        if (Settings.Difficulty > 1)                                    // this only happens when difficulty is 2 and greater. At diff = 1 all board is visible to the player
        {
            while (MainCamera.transform.position.y < AllExtraLanes)    // move camera until it reaches movement limit determined by all extra lanes
            {
                MainCamera.transform.position += MoveCameraUp;
                yield return null;   //Wait for next frame
            }
        }
    }

    public IEnumerator Event_ResetCameraPosition()
    {
        StopCoroutine("Event_CameraMovementUp");    // this version of StopCoroutine call should stop all coroutines with matching name
        if (Settings.Difficulty > 1)                // this only happens when difficulty is 2 and greater. At diff = 1 all board is visible to the player
        {
            while (MainCamera.transform.position.y > DefaultCameraPosition.y)
            {
                MainCamera.transform.position -= MoveCameraUp;
                yield return null;
            }
        }
    }
}
