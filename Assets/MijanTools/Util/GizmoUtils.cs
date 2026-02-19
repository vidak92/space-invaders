using UnityEngine;

namespace MijanTools.Util
{
    public static class GizmoUtils
    {
        public static void DrawGizmoRect(Vector3 position, float width, float height, Color color)
        {
            var halfWidth = width / 2f;
            var halfHeight = height / 2f;
            var topLeft = position + new Vector3(-halfWidth, halfHeight, 0f);
            var topRight = position + new Vector3(halfWidth, halfHeight, 0f);
            var bottomRight = position + new Vector3(halfWidth, -halfHeight, 0f);
            var bottomLeft = position + new Vector3(-halfWidth, -halfHeight, 0f);

            Gizmos.color = color;
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
            Gizmos.DrawLine(bottomLeft, topLeft);
        }
    }
}