//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Lean.Touch;
//
//public class RotateObject : MonoBehaviour {
//
//    public GameObject ToRotate;
//    public GameObject ToLookAt;
//    public float Speed;
////    public float YSpeed;
////    public float ZSpeed;
//
//    protected virtual void OnEnable()
//    {
//        Application.targetFrameRate = 500; //todo: remove on production!
//
//        // Hook events
//        LeanTouch.OnFingerSet += OnFingerSet;
//    }
//
//    protected virtual void OnDisable()
//    {
//        // Unhook events
//        LeanTouch.OnFingerSet -= OnFingerSet;
//    }
//
//    private void OnFingerSet(LeanFinger finger)
//    {
//        WritePositions(finger);
//    }
//
//    void WritePositions(LeanFinger finger)
//    {
//        var start = finger.GetStartWorldPosition(1);
//
//        var end = finger.GetWorldPosition(1);
//
//        //Debug.Log("Lean: " + start + " " + end);
//        //ToRotate.transform.LookAt(ToLookAt.transform);
//        ToRotate.transform.RotateAround(ToLookAt.transform.position,
//            ToLookAt.transform.up,
//            Speed * (end.x - start.x));
//        
//    }
//}
