using System;
using System.Collections.Generic;
using UnityEngine;

namespace MADGazeSDK
{
    /// <summary>An internal class to receive and store event(Queue)
    /// from <see cref="MADHandGesture"/>,
    /// wait until the right timing has been called.
    /// <see cref="MADUnityIntegrator"/> <seealso cref="Dispatch(Action{HandGestureEvent})"/>
    /// </summary>
    internal class MADUnityEventHandler : IDisposable,
    IBehaviourClickListener, IBehaviourGrapListener, IBehaviourHoldPostListener, IHandGestureDetectedListener, IHandGestureReadyCallback, IHandGestureErrorCallback
    {
        /// <summary>
        /// Initialized when a game is loaded at runtime without action from the user.
        /// <see cref="https://docs.unity3d.com/ScriptReference/RuntimeInitializeOnLoadMethodAttribute.html"/>
        /// </summary>
        [RuntimeInitializeOnLoadMethod]
        static void OnRuntimeMethodLoad()
        {
            // MADUnityIntegrator.EnsureInstanceBeingCreated();
        }
        
        bool IsHandGestureReady = false;

        internal MADUnityEventHandler()
        {
            Debug.Log("Unity:MADHandGesture: MADUnityEventHandler");
            IsHandGestureReady = false;
            MADHandGesture.Instance.init();
            MADHandGesture.Instance.setIHandGestureReadyCallback(this);
            MADHandGesture.Instance.setErrorCallback(this);
            
            // // if(isRegDetectedListenerEnable()){
            //     MADHandGesture.Instance.startHandGesture();
            // // }
        }

        /// <summary>A queue to store all event trigger out of main thread
        /// reason for "static" the gameobject contain <see cref="MADUnityIntegrator"/> could be deleted.
        /// however <see cref="MADUnityIntegrator"/> instance can be re-create in the scene and continue provide service.
        /// </summary>
        private static Queue<HandGestureEvent> m_EventQueue = new Queue<HandGestureEvent>(10);
        internal void Dispatch(Action<HandGestureEvent> method)
        {
            while (m_EventQueue.Count > 0)
            {
                method(m_EventQueue.Dequeue());
            }
        }

        #region I/O redirection
        public void onHandGestureReady()
        {
            Debug.Log("Unity:MADHandGesture: onHandGestureReady");
            // Debug.Log("Unity:MADHandGesture: onHandGestureReady: isRegDetectedListenerEnable() = "  + isRegDetectedListenerEnable());
            // Debug.Log("Unity:MADHandGesture: onHandGestureReady isRegClickListenerEnable() = "+isRegClickListenerEnable());

            // Debug.Log($"{GetType().Name}: onHandGestureReady");
            IsHandGestureReady = true;
            
            MADHandGesture.Instance.addBehavioursHoldPost(this);
            
            
            regClickListener(true);
            
            regDetectedListener(true);

            regGrabListener(true);

            MADHandGesture.Instance.showHandSkeleton(HandGestureManager.Instance.SHOW_SKELETON);
        }

        public void onErrorCallback(int errorCode, string errorMessage)
        {
            Debug.LogError($"{GetType().Name}: onErrorCallback: errorCode = {errorCode} errorMessage = {errorMessage}");
        }

        public void onClick(int index, int x, int y)
            => m_EventQueue.Enqueue(new Click(index, x, y));

        public void onGrabStart(int index, int x, int y)
            => m_EventQueue.Enqueue(new Grab(Grab.GrabStatus.START, index, x, y, 0, 0));

        public void onGrabHolding(int index, int x, int y, int dx, int dy)
            => m_EventQueue.Enqueue(new Grab(Grab.GrabStatus.HOLDING, index, x, y, dx, dy));

        public void onGrabRelease(int index, int x, int y, int dx, int dy)
            => m_EventQueue.Enqueue(new Grab(Grab.GrabStatus.RELEASE, index, x, y, dx, dy));

        public void onGrabCancel(int index, int x, int y, int dx, int dy)
            => m_EventQueue.Enqueue(new Grab(Grab.GrabStatus.CANCEL, index, x, y, dx, dy));

        public void onHold(int index, string handType, int x, int y)
            => m_EventQueue.Enqueue(new Hold(Hold.HoldStatus.START, index, handType, x, y));

        public void onHoldCancel(int index, string handType, int x, int y)
            => m_EventQueue.Enqueue(new Hold(Hold.HoldStatus.CANCEL, index, handType, x, y));

        public void onHandDetected(Hand hand, bool isHand)
            => m_EventQueue.Enqueue(new HandDetected(hand, isHand));

        public void Dispose()
        {
            MADHandGesture.Instance.stopHandGesture();
        }
        #endregion // I/O redirection


        private void checkAndStartHandGesture(){
            Debug.Log("checkAndStartHandGesture");
            if(!MADHandGesture.Instance.isHandGestureRunning()){
                 Debug.Log("checkAndStartHandGesture: startHandGesture");
                MADHandGesture.Instance.startHandGesture();
            } else{
                Debug.Log("checkAndStartHandGesture: null");
            }
        }
        public void regDetectedListener(bool isEnable){
            if(isEnable){
                checkAndStartHandGesture();
            }

            if(IsHandGestureReady){
                if(isEnable){
                        if(isRegDetectedListenerEnable()){
                            MADHandGesture.Instance.addHandGestureDetectedListener(this); 
                        }
                } else {
                    if(isRegDetectedListenerEnable() == false){
                            MADHandGesture.Instance.addHandGestureDetectedListener(null); 
                        }
                }
            }
        }

        private bool isRegDetectedListenerEnable(){
            return HandGestureManager.Instance.isEnabled<HandSignalController>() || 
                            HandGestureManager.Instance.isEnabled<HandCursorController>() ||
                            HandGestureManager.Instance.isEnabled<HandTrackingController>();
        }

        public void regClickListener(bool isEnable){
             if(isEnable){
                checkAndStartHandGesture();
            }

            if(IsHandGestureReady){
                if(isEnable){

                    if(isRegClickListenerEnable()){
                        MADHandGesture.Instance.addBehavioursClick(this);
                    } 

                } else {
                    
                    if(isRegClickListenerEnable() == false){
                        MADHandGesture.Instance.addBehavioursClick(null);
                    }

                }

            }
        }
    
        private bool isRegClickListenerEnable(){
            return HandGestureManager.Instance.isEnabled<HandCursorController>();
        }
 
        public void regGrabListener(bool isEnable){
             if(isEnable){
                checkAndStartHandGesture();
            }

            if(IsHandGestureReady){
                if(isEnable){

                    if(isRegGrabListenerEnable()){
                        MADHandGesture.Instance.addBehavioursGrab(this);
                    } 

                } else {
                    
                    if(isRegGrabListenerEnable() == false){
                        MADHandGesture.Instance.addBehavioursGrab(null);
                    }

                }

            }
        }

        public bool isRegGrabListenerEnable(){
            return HandGestureManager.Instance.isEnabled<HandGrabController>();
        }
    

    }

   


}
