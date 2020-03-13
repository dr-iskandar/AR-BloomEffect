using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WheelState{
    Normal = 1,
    Left = 2,
    Right = 3
}

public class WheelMovement : MonoBehaviour {

    public Transform SteeringWheel;
    public Transform FrontLeftWheel;
    public Transform FrontLeftWheelCaliper;
    public Transform FrontRightWheel;
    public Transform FrontRightWheelCaliper;
    public float Angle = 45f;
    public float Speed = 3f;

    StoreTransform _steeringWheelStable;
    StoreTransform _frontLeftWheelStable;
    StoreTransform _frontLeftWheelCaliperStable;
    StoreTransform _frontRightWheelStable;
    StoreTransform _frontRightWheelCaliperStable;

    WheelState _wheelState = WheelState.Normal; //first state

    void Start()
    {
        _steeringWheelStable = SteeringWheel.SaveLocal();
        _frontLeftWheelStable = FrontLeftWheel.SaveLocal();
        _frontLeftWheelCaliperStable = FrontLeftWheelCaliper.SaveLocal();
        _frontRightWheelStable = FrontRightWheel.SaveLocal();
        _frontRightWheelCaliperStable = FrontRightWheelCaliper.SaveLocal();
    }

    void Update()
    {
        if (_wheelState == WheelState.Left)
        {
            Check(SteeringWheel, _steeringWheelStable, new Vector3(0, 1, 0), -Angle * 2); //steering wheel turns more than wheels :)
            Check(FrontLeftWheel, _frontLeftWheelStable, new Vector3(0, 0, 1), -Angle);
            Check(FrontLeftWheelCaliper, _frontLeftWheelCaliperStable, new Vector3(0, 0, 1), -Angle);
            Check(FrontRightWheel, _frontRightWheelStable, new Vector3(0, 0, 1), -Angle);
            Check(FrontRightWheelCaliper, _frontRightWheelCaliperStable, new Vector3(0, 0, 1), -Angle);
        } else if (_wheelState == WheelState.Right)
        {
            Check(SteeringWheel, _steeringWheelStable, new Vector3(0, 1, 0), Angle * 2); 
            Check(FrontLeftWheel, _frontLeftWheelStable, new Vector3(0, 0, 1), Angle);
            Check(FrontLeftWheelCaliper, _frontLeftWheelCaliperStable, new Vector3(0, 0, 1), Angle);
            Check(FrontRightWheel, _frontRightWheelStable, new Vector3(0, 0, 1), Angle);
            Check(FrontRightWheelCaliper, _frontRightWheelCaliperStable, new Vector3(0, 0, 1), Angle);
        } else if (_wheelState == WheelState.Normal)
        {
            Check(SteeringWheel, _steeringWheelStable, new Vector3(0, 1, 0), 0); 
            Check(FrontLeftWheel, _frontLeftWheelStable, new Vector3(0, 0, 1), 0);
            Check(FrontLeftWheelCaliper, _frontLeftWheelCaliperStable, new Vector3(0, 0, 1), 0);
            Check(FrontRightWheel, _frontRightWheelStable, new Vector3(0, 0, 1), 0);
            Check(FrontRightWheelCaliper, _frontRightWheelCaliperStable, new Vector3(0, 0, 1), 0);
        }
    }

    void Check(Transform transform, StoreTransform store, Vector3 direction, float angle)
    {
        if (transform.localRotation != store.rotation*Quaternion.Euler(direction*angle))
        {
            transform.localRotation = Quaternion.Lerp(
                transform.localRotation,
                store.rotation*Quaternion.Euler(direction*angle), 
                Time.deltaTime * Speed);
        }
    }

    public void ChangeWheelStates()
    {
        if (_wheelState == WheelState.Normal)
        {
            _wheelState = WheelState.Left;
        }
        else if (_wheelState == WheelState.Left)
        {
            _wheelState = WheelState.Right;
        }
        else if (_wheelState == WheelState.Right)
        {
            _wheelState = WheelState.Normal;
        }
    }
}


