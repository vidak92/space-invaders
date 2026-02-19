using System.Collections.Generic;
using UnityEngine;

namespace MijanTools.Util
{
    public class DebugDraw : MonoBehaviour
    {
        private struct DrawLineData
        {
            public Vector3 Point1;
            public Vector3 Point2;
            public Color Color;
            public int SortOrder;

            public DrawLineData(Vector3 point1, Vector3 point2, Color color, int sortOrder)
            {
                Point1 = point1;
                Point2 = point2;
                Color = color;
                SortOrder = sortOrder;
            }
        }

        private struct DrawCircleData
        {
            public Vector3 Center;
            public float Radius;
            public Color Color;
            public int SortOrder;

            public DrawCircleData(Vector3 center, float radius, Color color, int sortOrder)
            {
                Center = center;
                Radius = radius;
                Color = color;
                SortOrder = sortOrder;
            }
        }

        private static DebugDraw _instance;

        public static bool IsEnabled;
        public static float LineWidth = 0.1f;

        private Transform _activeParent;
        private Transform _pooledParent;

        private int _maxLines;
        private List<LineRenderer> _pooledLines;
        private List<LineRenderer> _activeLines;

        private Material _lineMaterial;
        private List<DrawLineData> _linesToDraw;
        private List<DrawCircleData> _circlesToDraw;
        private int _totalDrawn;

        private void LateUpdate()
        {
            // Return lines to pool.
            for (int i = _activeLines.Count - 1; i >= 0; i--)
            {
                var line = _activeLines[i];
                line.SetPosition(0, Vector3.zero);
                line.SetPosition(1, Vector3.zero);
                _activeLines.RemoveAt(i);
                line.transform.parent = _pooledParent;
                _pooledLines.Add(line);
            }
            _totalDrawn = 0;

            // Update lines visibility.
            _activeParent.gameObject.SetActive(IsEnabled);

            // Update for DrawLine.
            foreach (var drawLineData in _linesToDraw)
            {
                var color = drawLineData.Color;
                var p1 = drawLineData.Point1;
                var p2 = drawLineData.Point2;
                var sortOrder = drawLineData.SortOrder;

                var line = GetLineObjectFromPool();
                line.positionCount = 2;
                line.useWorldSpace = true;
                line.startColor = color;
                line.endColor = color;
                line.startWidth = LineWidth;
                line.endWidth = LineWidth;
                line.SetPosition(0, p1);
                line.SetPosition(1, p2);
                line.sortingOrder = sortOrder;
                line.transform.parent = _activeParent;
                _activeLines.Add(line);
            }
            _linesToDraw.Clear();

            // Update for DrawCircle.
            foreach (var drawCircleData in _circlesToDraw)
            {
                var center = drawCircleData.Center;
                var radius = drawCircleData.Radius;
                var color = drawCircleData.Color;
                var sortOrder = drawCircleData.SortOrder;

                var n = 50;
                var line = GetLineObjectFromPool();
                line.positionCount = n + 1;
                line.startColor = color;
                line.endColor = color;
                line.startWidth = 0.1f;
                line.endWidth = 0.1f;
                for (int i = 0; i <= n; i++)
                {
                    var angle = (float)i / n * Mathf.Deg2Rad * 360f;
                    var x = Mathf.Sin(angle) * radius;
                    var y = Mathf.Cos(angle) * radius;
                    line.SetPosition(i, new Vector3(x, y, 0f) + center);
                }
                line.sortingOrder = sortOrder;
                line.transform.parent = _activeParent;
                _activeLines.Add(line);
            }
            _circlesToDraw.Clear();
        }

        private static void Init()
        {
            if (_instance == null)
            {
                _instance = new GameObject("DebugDraw").AddComponent<DebugDraw>();
                _instance.InitState();
                DontDestroyOnLoad(_instance.gameObject);
            }
        }

        private void InitState()
        {
            _activeParent = new GameObject("Active").transform;
            _activeParent.parent = transform;

            _pooledParent = new GameObject("Pooled").transform;
            _pooledParent.parent = transform;
            _pooledParent.gameObject.SetActive(false);

            _lineMaterial = new Material(Shader.Find("Sprites/Default"));

            _maxLines = 100;
            _pooledLines = new List<LineRenderer>();
            _activeLines = new List<LineRenderer>();
            for (int i = 0; i < _maxLines; i++)
            {
                InstantiateLineObjectInPool();
            }

            _linesToDraw = new List<DrawLineData>();
            _circlesToDraw = new List<DrawCircleData>();
        }

        public static void DrawLine(Vector3 p1, Vector3 p2, Color color)
        {
            Init();

            _instance._linesToDraw.Add(new DrawLineData(p1, p2, color, _instance._totalDrawn));
            _instance._totalDrawn++;
        }

        public static void DrawCircle(Vector3 center, float radius, Color color)
        {
            Init();

            _instance._circlesToDraw.Add(new DrawCircleData(center, radius, color, _instance._totalDrawn));
            _instance._totalDrawn++;
        }

        private LineRenderer GetLineObjectFromPool()
        {
            // If pool is empty, instantiate a new line.
            if (_pooledLines.Count == 0)
            {
                InstantiateLineObjectInPool();
            }

            // Get last line from pool.
            var lastIndex = _pooledLines.Count - 1;
            var line = _pooledLines[lastIndex];
            line.transform.parent = null;
            _pooledLines.RemoveAt(lastIndex);
            return line;
        }

        private LineRenderer InstantiateLineObjectInPool()
        {
            var lineObject = new GameObject($"Line{_pooledLines.Count}");
            lineObject.transform.parent = _pooledParent;

            var line = lineObject.AddComponent<LineRenderer>();
            line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            line.receiveShadows = false;
            line.motionVectorGenerationMode = MotionVectorGenerationMode.Camera;
            line.sharedMaterial = _lineMaterial;
            line.useWorldSpace = true;
            line.loop = false;
            line.sortingLayerName = "Debug";

            _pooledLines.Add(line);

            return line;
        }
    }
}