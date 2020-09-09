using System.Collections.Generic;
using UnityEngine;
using MADGazeSDK;

public class HandGestureDebugger : MonoBehaviour
{
    public Color m_FingerColorOnPitch = new Color(1f, 1f, 1f, .3f);
    public float m_FingerColorDuration = 1f;

    public Color m_PitchColor = Color.red;
    public float m_PitchDuration = 3f;

    // Draws a line from "startVertex" var to the curent mouse position.
    public Material mat;
    private void Start()
    {
        if (!mat)
        {
            Debug.LogWarning("Please Assign a material on the inspector");
            mat = new Material(Shader.Find("Standard"));
        }
        MADUnityIntegrator.EventHandDetected += OnMADHGHandDetectedEvent;
        MADUnityIntegrator.EventClick += this.OnMADHGClickEvent;
    }

    private void OnDestroy()
    {
        MADUnityIntegrator.EventHandDetected -= OnMADHGHandDetectedEvent;
        MADUnityIntegrator.EventClick -= this.OnMADHGClickEvent;
    }

    private static Vector3 ToGIPoint(Vector2 p)
        => Camera.main.ScreenToViewportPoint(p);
        // => new Vector3(p.x / Screen.width, p.y / Screen.height, 0f);

    private static Vector2 FlipY(Point p)
        => new Vector2((float)p.X, Screen.height - (float)p.Y);
    private struct GIHand
    {
        public Vector2 palmCenterPoint;
        public Vector2[] fingers;
        public GIHand(HandData handData)
        {
            palmCenterPoint = FlipY(handData.palmCenterPoint);
            fingers = new Vector2[handData.fingers.Count];
            for (int i=0; i< handData.fingers.Count; i++)
                fingers[i] = FlipY(handData.fingers[i].point);
        }

        public void DrawGI(float id)
        {
            for (int i = 0; i < fingers.Length; i++)
            {
                Color color = new Color(
                    1f, // R
                    Mathf.Clamp01(id), // G
                    Mathf.Clamp01((float)i / fingers.Length), // B
                    1f);
                GL.Begin(GL.LINES);
                GL.Color(color);
                GL.Vertex(ToGIPoint(palmCenterPoint));
                GL.Vertex(ToGIPoint(fingers[i]));
                GL.End();
            }
        }
    }
    private struct GIPoint
    {
        public Vector2 point;
        public Color color;
        public float startTime;
        public float endTime;
        public float duration;
        public float pt => Mathf.Clamp01((Time.timeSinceLevelLoad - startTime) / duration);
        private static readonly Color clear = Color.clear;
        public Color fadeColor => Color.Lerp(this.color, clear, pt);
        public GIPoint(Point _point, float duration, Color _color)
        {
            startTime = Time.timeSinceLevelLoad;
            this.duration = duration;
            endTime = startTime + duration;
            point = FlipY(_point);
            color = _color;
        }

        public void DrawGI()
        {
            Vector3 p = ToGIPoint(point);
            GL.Begin(GL.LINES);
            GL.Color(fadeColor);
            GL.Vertex(p + offsetY1);
            GL.Vertex(p + offsetY2);
            GL.End();
            GL.Begin(GL.LINES);
            GL.Color(fadeColor);
            GL.Vertex(p + offsetX1);
            GL.Vertex(p + offsetX2);
            GL.End();
        }

        private const float l = 0.01f;
        private static readonly Vector3 offsetY1 = new Vector3(0f, -l, 0f);
        private static readonly Vector3 offsetY2 = new Vector3(0f, l, 0f);
        private static readonly Vector3 offsetX1 = new Vector3(-l, 0f, 0f);
        private static readonly Vector3 offsetX2 = new Vector3(l, 0f, 0f);
    }
    List<GIHand> m_Hands;
    List<GIPoint> m_Points = new List<GIPoint>(10);
    void OnMADHGHandDetectedEvent(HandDetected handDetected)
    {
        if (handDetected != null)
        {
            Hand m_Hand = handDetected.hand;
            if (m_Hand != null)
            {
                int count = m_Hand.handDatas.Count;
                m_Hands = new List<GIHand>(count);
                for (int i = 0; i < count; i++)
                {
                    m_Hands.Add(new GIHand(m_Hand.handDatas[i]));
                    if (m_Hand.handDatas[i].isPinch)
                    {
                        for (int k = 0; k < m_Hand.handDatas[i].fingers.Count; k++)
                        {
                            m_Points.Add(new GIPoint(m_Hand.handDatas[i].fingers[k].point, m_FingerColorDuration, m_FingerColorOnPitch));
                        }
                    }
                }
            }
        }
    }

    void OnMADHGClickEvent(Click click)
    {
        m_Points.Add(new GIPoint(new Point(click.x, click.x), m_PitchDuration, m_PitchColor));
    }

    void OnPostRender()
    {
        
        GL.PushMatrix();
        mat.SetPass(0);
        GL.LoadOrtho();

        if (m_Hands != null)
        {
            for (int i = 0; i < m_Hands.Count; i++)
            {
                float f = (float)m_Hands.Count - ((float)i / (float)m_Hands.Count);
                m_Hands[i].DrawGI(f);
            }
        }

        if (m_Points.Count > 0)
        {
            int k = m_Points.Count;
            while (k-- > 0)
            {
                if (Time.timeSinceLevelLoad < m_Points[k].endTime)
                    m_Points[k].DrawGI();
                else
                    m_Points.RemoveAt(k);
            }
        }
        
        GL.PopMatrix();
    }
}
