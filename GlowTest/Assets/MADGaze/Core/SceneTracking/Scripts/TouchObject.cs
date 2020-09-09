using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TouchScript.Behaviors;
using TouchScript.Gestures.TransformGestures;

public class TouchObject : MonoBehaviour
    {
        private TransformGesture gesture;
        private Transformer transformer;
        //private Rigidbody rb;

        private void OnEnable()
        {
            // The gesture
            gesture = GetComponent<TransformGesture>();
            // Transformer component actually MOVES the object
            transformer = GetComponent<Transformer>();
            //rb = GetComponent<Rigidbody>();

            transformer.enabled = false;
            //rb.isKinematic = false;

            // Subscribe to gesture events
            gesture.TransformStarted += transformStartedHandler;
            gesture.TransformCompleted += transformCompletedHandler;
        }

        private void OnDisable()
        {
            // Unsubscribe from gesture events
            gesture.TransformStarted -= transformStartedHandler;
            gesture.TransformCompleted -= transformCompletedHandler;
        }

        private void transformStartedHandler(object sender, EventArgs e)
        {
            // When movement starts we need to tell physics that now WE are moving this object manually
            //rb.isKinematic = true;
            transformer.enabled = true;
        }

        private void transformCompletedHandler(object sender, EventArgs e)
        {
            transformer.enabled = false;
            //rb.isKinematic = false;
            //rb.WakeUp();
        }
    }
