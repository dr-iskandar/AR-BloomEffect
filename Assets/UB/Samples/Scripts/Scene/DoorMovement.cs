using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorState{
    BothClosed = 1,
    LeftOpen = 2,
    RightOpen = 3,
    LeftClosed = 4,
    RightClosed = 5
}

public class DoorMovement : MonoBehaviour {

    public Transform LeftDoor;
    public Transform RightDoor;
    public float Angle = 50f;
    public float Speed = 3f;

    StoreTransform _leftDoorStable;
    StoreTransform _rightDoorStable;

    bool _isAnimating = false;
    DoorState _doorState = DoorState.BothClosed; //first state

    void Start()
    {
        _leftDoorStable = LeftDoor.SaveLocal();
        _rightDoorStable = RightDoor.SaveLocal();
    }

    void Update()
    {
        if (_isAnimating)
        {
            if (_doorState == DoorState.LeftOpen)
            {
                Check(LeftDoor, _leftDoorStable, new Vector3(0, 0, 1), Angle);
            }
            else if (_doorState == DoorState.RightOpen)
            {
                Check(RightDoor, _rightDoorStable, new Vector3(0, 0, 1), -Angle);
            }
            else if (_doorState == DoorState.LeftClosed)
            {
                Check(LeftDoor, _leftDoorStable, new Vector3(0, 0, 1), 0);
            }
            else if (_doorState == DoorState.RightClosed)
            {
                Check(RightDoor, _rightDoorStable, new Vector3(0, 0, 1), 0);
            }
        }
    }

    void Check(Transform transform, StoreTransform store, Vector3 direction, float angle)
    {
        if (transform.localRotation == store.rotation*Quaternion.Euler(direction*angle)) 
        {
            _isAnimating = false;
            if (_doorState == DoorState.RightClosed)
            {
                _doorState = DoorState.BothClosed; //loop ended
            }
        } else
        {
            transform.localRotation = Quaternion.Lerp(
                transform.localRotation,
                store.rotation*Quaternion.Euler(direction*angle), 
                Time.deltaTime * Speed);
        }
    }

    public void ChangeDoorStates()
    {
        if (_isAnimating)
        {
            return;
        }

        if (_doorState == DoorState.BothClosed)
        {
            _doorState = DoorState.LeftOpen;
        }
        else if (_doorState == DoorState.LeftOpen)
        {
            _doorState = DoorState.RightOpen;
        }
        else if (_doorState == DoorState.RightOpen)
        {
            _doorState = DoorState.LeftClosed;
        }
        else if (_doorState == DoorState.LeftClosed)
        {
            _doorState = DoorState.RightClosed; //will fall back to bothclosed
        }
        _isAnimating = true;
    }
}


