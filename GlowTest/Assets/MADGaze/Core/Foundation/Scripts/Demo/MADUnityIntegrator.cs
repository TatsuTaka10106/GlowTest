using UnityEngine;
using System;
using UnityEngine.SceneManagement;

namespace MADGazeSDK
{
    /// <summary>
    /// Convert Hardware callback via Java library,
    /// dispatch those callback as event within engine's main thread.
    /// </summary>
    public class MADUnityIntegrator : MonoBehaviour
    {
        #region Singleton
        private static MADUnityIntegrator m_Instance = null;
        public static MADUnityIntegrator Instance
        {
            get
            {
                if (m_Instance==null && !m_AppExit)
                {
                #if false
                    m_Instance = new GameObject(nameof(MADHandGesture)).AddComponent<UnityAdapter>();
                    DontDestroyOnLoad(m_Instance); // to prevent change scene destroy.
                #else
                    new GameObject(nameof(MADHandGesture), typeof(MADUnityIntegrator));
                    /// <see cref="Awake"/>
                #endif
                }
                return m_Instance;
            }
        }

        private static MADUnityEventHandler m_Handler = null;

        /// <summary>To prevent infinite loop - spawn singleton during application exit</summary>
        private static bool m_AppExit = false;

        internal static void EnsureInstanceBeingCreated()
        {
            // A hack to trigger Instance to create on scene.
            if (Instance == null)
                throw new UnityException($"{nameof(MADUnityIntegrator)} : Instance fail to create.");
        }
        private static void OnActiveSceneChanged(Scene arg0, Scene arg1)
        {
            EnsureInstanceBeingCreated();
        }
        #endregion // Singleton

        #region Unity Program Cycle
        private void Awake()
        {
            if (m_Instance == null)
            {
                m_Instance = this;
                SceneManager.activeSceneChanged += OnActiveSceneChanged;
            }
            else
                throw new UnityException($"Double initialize {nameof(MADUnityIntegrator)}");

            if (m_Handler == null)
                m_Handler = new MADUnityEventHandler();

            DontDestroyOnLoad(this);
        }
        private void Update()
        {
            m_Handler.Dispatch(EventDispatcher);
        }
        private void OnApplicationQuit()
        {
            m_Handler?.Dispose();
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
            m_AppExit = true;
        }
        #endregion // Unity Program Cycle

        #region Event Handler
        public static event Action<HandDetected> EventHandDetected;
        public static event Action<Click> EventClick;
        public static event Action<Grab> EventGrab;
        public static event Action<Hold> EventHold;

        private void EventDispatcher(HandGestureEvent e)
        {
            if (e == null)
            {
                throw new NullReferenceException($"{nameof(HandGestureEvent)} can not be null, please check {nameof(MADUnityIntegrator)} logic.");
            }
            else if (e is HandDetected)
            {
                EventHandDetected?.Invoke((HandDetected)e);
            }
            else if (e is Click)
            {
                EventClick?.Invoke((Click)e);
            }
            else if (e is Grab)
            {
                EventGrab?.Invoke((Grab)e);
            }
            else if (e is Hold)
            {
                EventHold?.Invoke((Hold)e);
            }
            else
            {
                throw new NotImplementedException($"Fail to dispatch {nameof(HandGestureEvent)}, non implement type detected.");
            }
        }
        #endregion // Event Handler


         public void regDetectedListener(bool isEnable){
            if(m_Handler!=null){
                m_Handler.regDetectedListener(isEnable);
            }
        }

         public void regClickListener(bool isEnable){
            if(m_Handler!=null){
                m_Handler.regClickListener(isEnable);
            }
        }

        public void regGrabListener(bool isEnable){
            if(m_Handler!=null){
                m_Handler.regGrabListener(isEnable);
            }
        }
    }
}