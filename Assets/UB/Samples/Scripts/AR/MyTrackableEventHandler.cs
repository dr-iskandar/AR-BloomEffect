///*==============================================================================
//Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
//All Rights Reserved.
//Confidential and Proprietary - Qualcomm Connected Experiences, Inc.
//==============================================================================*/
//
//using UnityEngine;
//using System.Linq;
//using System.Collections.Generic;
//
//namespace Vuforia
//{
//    /// <summary>
//    /// A custom handler that implements the ITrackableEventHandler interface.
//    /// </summary>
//    public class MyTrackableEventHandler : MonoBehaviour,
//    ITrackableEventHandler
//    {
//        #region PRIVATE_MEMBER_VARIABLES
//
//        private TrackableBehaviour mTrackableBehaviour;
//
//        #endregion // PRIVATE_MEMBER_VARIABLES
//
//        #region UNTIY_MONOBEHAVIOUR_METHODS
//
//        public static bool IsTracking = false;
//
//
//        void Awake()
//        {
//        }
//
//        void Start()
//        {
//            mTrackableBehaviour = GetComponent<TrackableBehaviour>();
//            if (mTrackableBehaviour)
//            {
//                mTrackableBehaviour.RegisterTrackableEventHandler(this);
//            }
//        }
//
//        #endregion // UNTIY_MONOBEHAVIOUR_METHODS
//
//
//
//        #region PUBLIC_METHODS
//
//        /// <summary>
//        /// Implementation of the ITrackableEventHandler function called when the
//        /// tracking state changes.
//        /// </summary>
//        public void OnTrackableStateChanged(
//            TrackableBehaviour.Status previousStatus,
//            TrackableBehaviour.Status newStatus)
//        {
//            if (newStatus == TrackableBehaviour.Status.DETECTED ||
//                newStatus == TrackableBehaviour.Status.TRACKED ||
//                newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
//            {
//                OnTrackingFound();
//            }
//            else
//            {
//                OnTrackingLost();
//            }
//        }
//
//        #endregion // PUBLIC_METHODS
//
//
//
//        #region PRIVATE_METHODS
//
//
//        private void OnTrackingFound()
//        {
//            TrackerFound();
//
//            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
//        }
//
//
//        private void OnTrackingLost()
//        {
//            TrackerLost();
//
//            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
//        }
//
//        #endregion // PRIVATE_METHODS
//
//        public void TrackerLost()
//        {
//            IsTracking = false;
//
//            var arSceneElements = FindObjectsOfType<ArSceneElement>().ToList();
//            var renderers = new List<Renderer>();
//            foreach (var arSceneElement in arSceneElements)
//            {
//                var arSceneElementRenderers = arSceneElement.gameObject.GetComponentsInChildren<Renderer>();
//                foreach (var arSceneElementRenderer in arSceneElementRenderers)
//                {
//                    renderers.Add(arSceneElementRenderer);
//                }
//            }
//
//            // Disable rendering:
//            foreach (Renderer component in renderers)
//            {
//                component.enabled = false;
//            }
//        }
//
//        public void TrackerFound()
//        {
//            IsTracking = true;
//
//            var arSceneElements = FindObjectsOfType<ArSceneElement>().ToList();
//            var renderers = new List<Renderer>();
//            foreach (var arSceneElement in arSceneElements)
//            {
//                var arSceneElementRenderers = arSceneElement.gameObject.GetComponentsInChildren<Renderer>();
//                foreach (var arSceneElementRenderer in arSceneElementRenderers)
//                {
//                    renderers.Add(arSceneElementRenderer);
//                }
//            }
//
//            // Enable rendering:
//            foreach (Renderer component in renderers)
//            {
//                component.enabled = true;
//            }
//        }
//
//        public static void StaticArCamera()
//        {
//            CameraDevice.Instance.SetFocusMode( 
//                CameraDevice.FocusMode.FOCUS_MODE_TRIGGERAUTO);
//        }
//    }
//}
