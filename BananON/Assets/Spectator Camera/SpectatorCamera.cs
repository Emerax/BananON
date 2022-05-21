using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Mouse/Keyboard Controllable spectator Camera.
/// 3D controls similar to Unity view.
/// </summary>
/// 
public class SpectatorCamera : MonoBehaviour
{
    public float speed = 10;
    public float rotationSpeed = 360;

    private Vector3 inputMoveVector;
    private Vector2 inputRotVector;

    private Vector3 currentRot;

    private void Awake() {
        inputMoveVector = Vector3.zero;
        inputRotVector = Vector2.zero;

        currentRot = transform.rotation.eulerAngles;
    }

    private void Update() {
        //Read inputs
        inputMoveVector = Vector3.zero;
        inputRotVector = Vector2.zero;

        if(Keyboard.current.wKey.isPressed) {
            inputMoveVector.z = 1;
        }
        else if(Keyboard.current.sKey.isPressed) {
            inputMoveVector.z = -1;
        }

        if(Keyboard.current.dKey.isPressed) {
            inputMoveVector.x = 1;
        }
        else if(Keyboard.current.aKey.isPressed) {
            inputMoveVector.x = -1;
        }

        if(Keyboard.current.spaceKey.isPressed) {
            inputMoveVector.y = 1;
        }
        else if(Keyboard.current.shiftKey.isPressed) {
            inputMoveVector.y = -1;
        }

        inputRotVector = Mouse.current.delta.ReadValue();

        //Apply inputs
        //Quaternion rotationMove = Quaternion.Euler(
        //    inputRotVector.x, inputRotVector.y, 0
        //    );
        //transform.rotation = rotationMove * transform.rotation;
        currentRot.x -= inputRotVector.y * rotationSpeed;
        currentRot.y += inputRotVector.x * rotationSpeed;
        currentRot.x = (currentRot.x + 360) % 360;
        currentRot.y = (currentRot.y + 360) % 360;

        currentRot.z = 0;
        transform.rotation = Quaternion.Euler(currentRot);

        Vector3 move = inputMoveVector * speed;
        Quaternion cameraRot = transform.rotation;
        cameraRot = Quaternion.Euler(0, cameraRot.eulerAngles.y, 0);
        move = cameraRot * move;

        transform.position += move * Time.deltaTime;
    }
}
