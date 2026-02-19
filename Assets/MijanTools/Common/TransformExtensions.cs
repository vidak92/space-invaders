using UnityEngine;

namespace MijanTools.Common
{
    public static class TransformExtensions
    {
        // Set Position
        public static void SetPositionX(this Transform transform, float x)
        {
            var tempPosition = transform.position;
            tempPosition.x = x;
            transform.position = tempPosition;
        }

        public static void SetPositionY(this Transform transform, float y)
        {
            var tempPosition = transform.position;
            tempPosition.y = y;
            transform.position = tempPosition;
        }

        public static void SetPositionZ(this Transform transform, float z)
        {
            var tempPosition = transform.position;
            tempPosition.z = z;
            transform.position = tempPosition;
        }

        public static void SetPositionXY(this Transform transform, float x, float y)
        {
            transform.position = new Vector3(x, y, transform.position.z);
        }

        public static void SetPositionXZ(this Transform transform, float x, float z)
        {
            transform.position = new Vector3(x, transform.position.y, z);
        }

        public static void SetPositionYZ(this Transform transform, float y, float z)
        {
            transform.position = new Vector3(transform.position.x, y, z);
        }

        public static void SetPositionXYZ(this Transform transform, float x, float y, float z)
        {
            transform.position = new Vector3(x, y, z);
        }

        // Add Position
        public static void AddPositionX(this Transform transform, float x)
        {
            var tempPosition = transform.position;
            tempPosition.x += x;
            transform.position = tempPosition;
        }

        public static void AddPositionY(this Transform transform, float y)
        {
            var tempPosition = transform.position;
            tempPosition.y += y;
            transform.position = tempPosition;
        }

        public static void AddPositionZ(this Transform transform, float z)
        {
            var tempPosition = transform.position;
            tempPosition.z += z;
            transform.position = tempPosition;
        }

        public static void AddPositionXY(this Transform transform, float x, float y)
        {
            var tempPosition = transform.position;
            tempPosition.x += x;
            tempPosition.y += y;
            transform.position = tempPosition;
        }

        public static void AddPositionXZ(this Transform transform, float x, float z)
        {
            var tempPosition = transform.position;
            tempPosition.x += x;
            tempPosition.z += z;
            transform.position = tempPosition;
        }

        public static void AddPositionYZ(this Transform transform, float y, float z)
        {
            var tempPosition = transform.position;
            tempPosition.y += y;
            tempPosition.z += z;
            transform.position = tempPosition;
        }

        public static void AddPositionXYZ(this Transform transform, float x, float y, float z)
        {
            var tempPosition = transform.position;
            tempPosition.x += x;
            tempPosition.y += y;
            tempPosition.z += z;
            transform.position = tempPosition;
        }

        // Clamp Position
        public static void ClampPositionX(this Transform transform, float minX, float maxX)
        {
            var tempPosition = transform.position;
            tempPosition.x = Mathf.Clamp(tempPosition.x, minX, maxX);
            transform.position = tempPosition;
        }

        public static void ClampPositionY(this Transform transform, float minY, float maxY)
        {
            var tempPosition = transform.position;
            tempPosition.y = Mathf.Clamp(tempPosition.y, minY, maxY);
            transform.position = tempPosition;
        }

        public static void ClampPositionZ(this Transform transform, float minZ, float maxZ)
        {
            var tempPosition = transform.position;
            tempPosition.z = Mathf.Clamp(tempPosition.z, minZ, maxZ);
            transform.position = tempPosition;
        }
        // TODO: Clamp Position: XY, XZ, YZ, XYZ

        // Set Local Position
        public static void SetLocalPositionX(this Transform transform, float x)
        {
            var tempPosition = transform.localPosition;
            tempPosition.x = x;
            transform.localPosition = tempPosition;
        }

        public static void SetLocalPositionY(this Transform transform, float y)
        {
            var tempPosition = transform.localPosition;
            tempPosition.y = y;
            transform.localPosition = tempPosition;
        }

        public static void SetLocalPositionZ(this Transform transform, float z)
        {
            var tempPosition = transform.localPosition;
            tempPosition.z = z;
            transform.localPosition = tempPosition;
        }

        public static void SetLocalPositionXY(this Transform transform, float x, float y)
        {
            transform.localPosition = new Vector3(x, y, transform.localPosition.z);
        }

        public static void SetLocalPositionXZ(this Transform transform, float x, float z)
        {
            transform.localPosition = new Vector3(x, transform.localPosition.y, z);
        }

        public static void SetLocalPositionYZ(this Transform transform, float y, float z)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, y, z);
        }

        public static void SetLocalPositionXYZ(this Transform transform, float x, float y, float z)
        {
            transform.localPosition = new Vector3(x, y, z);
        }

        // Add Local Position
        public static void AddLocalPositionX(this Transform transform, float x)
        {
            var tempPosition = transform.localPosition;
            tempPosition.x += x;
            transform.localPosition = tempPosition;
        }

        public static void AddLocalPositionY(this Transform transform, float y)
        {
            var tempPosition = transform.localPosition;
            tempPosition.y += y;
            transform.localPosition = tempPosition;
        }

        public static void AddLocalPositionZ(this Transform transform, float z)
        {
            var tempPosition = transform.position;
            tempPosition.z += z;
            transform.position = tempPosition;
        }

        public static void AddLocalPositionXY(this Transform transform, float x, float y)
        {
            var tempPosition = transform.localPosition;
            tempPosition.x += x;
            tempPosition.y += y;
            transform.localPosition = tempPosition;
        }

        public static void AddLocalPositionXZ(this Transform transform, float x, float z)
        {
            var tempPosition = transform.localPosition;
            tempPosition.x += x;
            tempPosition.z += z;
            transform.localPosition = tempPosition;
        }

        public static void AddLocalPositionYZ(this Transform transform, float y, float z)
        {
            var tempPosition = transform.localPosition;
            tempPosition.y += y;
            tempPosition.z += z;
            transform.localPosition = tempPosition;
        }

        public static void AddLocalPositionXYZ(this Transform transform, float x, float y, float z)
        {
            var tempPosition = transform.localPosition;
            tempPosition.x += x;
            tempPosition.y += y;
            tempPosition.z += z;
            transform.localPosition = tempPosition;
        }

        // Clamp Local Position
        public static void ClampLocalPositionX(this Transform transform, float minX, float maxX)
        {
            var tempPosition = transform.localPosition;
            tempPosition.x = Mathf.Clamp(tempPosition.x, minX, maxX);
            transform.localPosition = tempPosition;
        }

        public static void ClampLocalPositionY(this Transform transform, float minY, float maxY)
        {
            var tempPosition = transform.localPosition;
            tempPosition.y = Mathf.Clamp(tempPosition.y, minY, maxY);
            transform.localPosition = tempPosition;
        }

        public static void ClampLocalPositionZ(this Transform transform, float minZ, float maxZ)
        {
            var tempPosition = transform.localPosition;
            tempPosition.z = Mathf.Clamp(tempPosition.z, minZ, maxZ);
            transform.localPosition = tempPosition;
        }
        // TODO: Clamp Local Position: XY, XZ, YZ, XYZ

        // Set Rotation
        public static void SetRotationX(this Transform transform, float eulerX)
        {
            var tempEulerAngles = transform.rotation.eulerAngles;
            tempEulerAngles.x = eulerX;
            transform.rotation = Quaternion.Euler(tempEulerAngles);
        }

        public static void SetRotationY(this Transform transform, float eulerY)
        {
            var tempEulerAngles = transform.rotation.eulerAngles;
            tempEulerAngles.y = eulerY;
            transform.rotation = Quaternion.Euler(tempEulerAngles);
        }

        public static void SetRotationZ(this Transform transform, float eulerZ)
        {
            var tempEulerAngles = transform.rotation.eulerAngles;
            tempEulerAngles.z = eulerZ;
            transform.rotation = Quaternion.Euler(tempEulerAngles);
        }

        public static void SetRotationXY(this Transform transform, float eulerX, float eulerY)
        {
            var tempEulerAngles = transform.rotation.eulerAngles;
            tempEulerAngles.x = eulerX;
            tempEulerAngles.y = eulerY;
            transform.rotation = Quaternion.Euler(tempEulerAngles);
        }

        public static void SetRotationXZ(this Transform transform, float eulerX, float eulerZ)
        {
            var tempEulerAngles = transform.rotation.eulerAngles;
            tempEulerAngles.x = eulerX;
            tempEulerAngles.z = eulerZ;
            transform.rotation = Quaternion.Euler(tempEulerAngles);
        }

        public static void SetRotationYZ(this Transform transform, float eulerY, float eulerZ)
        {
            var tempEulerAngles = transform.rotation.eulerAngles;
            tempEulerAngles.y = eulerY;
            tempEulerAngles.z = eulerZ;
            transform.rotation = Quaternion.Euler(tempEulerAngles);
        }

        public static void SetRotationXYZ(this Transform transform, float eulerX, float eulerY, float eulerZ)
        {
            var tempEulerAngles = transform.rotation.eulerAngles;
            tempEulerAngles.x = eulerX;
            tempEulerAngles.y = eulerY;
            tempEulerAngles.z = eulerZ;
            transform.rotation = Quaternion.Euler(tempEulerAngles);
        }
        // TODO: Add Rotation
        // TODO: Clamp Rotation

        // Set Local Rotation
        public static void SetLocalRotationX(this Transform transform, float eulerX)
        {
            var tempEulerAngles = transform.localRotation.eulerAngles;
            tempEulerAngles.x = eulerX;
            transform.localRotation = Quaternion.Euler(tempEulerAngles);
        }

        public static void SetLocalRotationY(this Transform transform, float eulerY)
        {
            var tempEulerAngles = transform.localRotation.eulerAngles;
            tempEulerAngles.y = eulerY;
            transform.localRotation = Quaternion.Euler(tempEulerAngles);
        }

        public static void SetLocalRotationZ(this Transform transform, float eulerZ)
        {
            var tempEulerAngles = transform.localRotation.eulerAngles;
            tempEulerAngles.z = eulerZ;
            transform.localRotation = Quaternion.Euler(tempEulerAngles);
        }

        public static void SetLocalRotationXY(this Transform transform, float eulerX, float eulerY)
        {
            var tempEulerAngles = transform.localRotation.eulerAngles;
            tempEulerAngles.x = eulerX;
            tempEulerAngles.y = eulerY;
            transform.localRotation = Quaternion.Euler(tempEulerAngles);
        }

        public static void SetLocalRotationXZ(this Transform transform, float eulerX, float eulerZ)
        {
            var tempEulerAngles = transform.localRotation.eulerAngles;
            tempEulerAngles.x = eulerX;
            tempEulerAngles.z = eulerZ;
            transform.localRotation = Quaternion.Euler(tempEulerAngles);
        }

        public static void SetRLocalotationYZ(this Transform transform, float eulerY, float eulerZ)
        {
            var tempEulerAngles = transform.localRotation.eulerAngles;
            tempEulerAngles.y = eulerY;
            tempEulerAngles.z = eulerZ;
            transform.localRotation = Quaternion.Euler(tempEulerAngles);
        }

        public static void SetLocalRotationXYZ(this Transform transform, float eulerX, float eulerY, float eulerZ)
        {
            var tempEulerAngles = transform.localRotation.eulerAngles;
            tempEulerAngles.x = eulerX;
            tempEulerAngles.y = eulerY;
            tempEulerAngles.z = eulerZ;
            transform.localRotation = Quaternion.Euler(tempEulerAngles);
        }
        // TODO: Add Local Rotation
        // TODO: Clamp Local Rotation

        // Set Local Scale
        public static void SetLocalScaleX(this Transform transform, float x)
        {
            var tempLocalScale = transform.localScale;
            tempLocalScale.x = x;
            transform.localScale = tempLocalScale;
        }

        public static void SetLocalScaleY(this Transform transform, float y)
        {
            var tempLocalScale = transform.localScale;
            tempLocalScale.y = y;
            transform.localScale = tempLocalScale;
        }

        public static void SetLocalScaleZ(this Transform transform, float z)
        {
            var tempLocalScale = transform.localScale;
            tempLocalScale.z = z;
            transform.localScale = tempLocalScale;
        }

        public static void SetLocalScaleXY(this Transform transform, float x, float y)
        {
            var tempLocalScale = transform.localScale;
            tempLocalScale.x = x;
            tempLocalScale.y = y;
            transform.localScale = tempLocalScale;
        }

        public static void SetLocalScaleXZ(this Transform transform, float x, float z)
        {
            var tempLocalScale = transform.localScale;
            tempLocalScale.x = x;
            tempLocalScale.z = z;
            transform.localScale = tempLocalScale;
        }

        public static void SetLocalScaleYZ(this Transform transform, float y, float z)
        {
            var tempLocalScale = transform.localScale;
            tempLocalScale.y = y;
            tempLocalScale.z = z;
            transform.localScale = tempLocalScale;
        }

        public static void SetLocalScaleXYZ(this Transform transform, float x, float y, float z)
        {
            transform.localScale = new Vector3(x, y, z);
        }

        public static void SetLocalScale(this Transform transform, float scale)
        {
            transform.localScale = new Vector3(scale, scale, scale);
        }

        // Add Local Scale
        public static void AddLocalScaleX(this Transform transform, float x)
        {
            var tempLocalScale = transform.localScale;
            tempLocalScale.x += x;
            transform.localScale = tempLocalScale;
        }

        public static void AddLocalScaleY(this Transform transform, float y)
        {
            var tempLocalScale = transform.localScale;
            tempLocalScale.y += y;
            transform.localScale = tempLocalScale;
        }

        public static void AddLocalScaleZ(this Transform transform, float z)
        {
            var tempLocalScale = transform.localScale;
            tempLocalScale.z += z;
            transform.localScale = tempLocalScale;
        }

        public static void AddLocalScaleXY(this Transform transform, float x, float y)
        {
            var tempLocalScale = transform.localScale;
            tempLocalScale.x += x;
            tempLocalScale.y += y;
            transform.localScale = tempLocalScale;
        }

        public static void AddLocalScaleXZ(this Transform transform, float x, float z)
        {
            var tempLocalScale = transform.localScale;
            tempLocalScale.x += x;
            tempLocalScale.z += z;
            transform.localScale = tempLocalScale;
        }

        public static void AddLocalScaleYZ(this Transform transform, float y, float z)
        {
            var tempLocalScale = transform.localScale;
            tempLocalScale.y += y;
            tempLocalScale.z += z;
            transform.localScale = tempLocalScale;
        }

        public static void AddLocalScaleXYZ(this Transform transform, float x, float y, float z)
        {
            var tempLocalScale = transform.localScale;
            tempLocalScale.x += x;
            tempLocalScale.y += y;
            tempLocalScale.z += z;
            transform.localScale = tempLocalScale;
        }

        // Clamp Local Scale
        public static void ClampLocalScaleX(this Transform transform, float minX, float maxX)
        {
            var tempScale = transform.localScale;
            tempScale.x = Mathf.Clamp(tempScale.x, minX, maxX);
            transform.localScale = tempScale;
        }

        public static void ClampLocalScaleY(this Transform transform, float minY, float maxY)
        {
            var tempScale = transform.localScale;
            tempScale.y = Mathf.Clamp(tempScale.y, minY, maxY);
            transform.localScale = tempScale;
        }

        public static void ClampLocalScaleZ(this Transform transform, float minZ, float maxZ)
        {
            var tempScale = transform.localScale;
            tempScale.z = Mathf.Clamp(tempScale.z, minZ, maxZ);
            transform.localScale = tempScale;
        }

        // Set Anchored Position
        public static void SetAnchoredPositionX(this RectTransform rectTransform, float x)
        {
            rectTransform.anchoredPosition = new Vector2(x, rectTransform.anchoredPosition.y);
        }

        public static void SetAnchoredPositionY(this RectTransform rectTransform, float y)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, y);
        }

        public static void SetAnchoredPositionXY(this RectTransform rectTransform, float x, float y)
        {
            rectTransform.anchoredPosition = new Vector2(x, y);
        }

        // Add Anchored Position
        public static void AddAnchoredPositionX(this RectTransform rectTransform, float x)
        {
            rectTransform.anchoredPosition += new Vector2(x, 0f);
        }

        public static void AddAnchoredPositionY(this RectTransform rectTransform, float y)
        {
            rectTransform.anchoredPosition += new Vector2(0f, y);
        }

        public static void AddAnchoredPositionXY(this RectTransform rectTransform, float x, float y)
        {
            rectTransform.anchoredPosition += new Vector2(x, y);
        }

        // Set Size Delta
        public static void SetSizeDeltaX(this RectTransform rectTransform, float x)
        {
            rectTransform.sizeDelta = new Vector2(x, rectTransform.sizeDelta.y);
        }

        public static void SetSizeDeltaY(this RectTransform rectTransform, float y)
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, y);
        }

        public static void SetSizeDeltaXY(this RectTransform rectTransform, float x, float y)
        {
            rectTransform.sizeDelta = new Vector2(x, y);
        }

        // Add Size Delta
        public static void AddSizeDeltaX(this RectTransform rectTransform, float x)
        {
            rectTransform.sizeDelta += new Vector2(x, 0f);
        }

        public static void AddSizeDeltaY(this RectTransform rectTransform, float y)
        {
            rectTransform.sizeDelta += new Vector2(0f, y);
        }

        public static void AddSizeDeltaXY(this RectTransform rectTransform, float x, float y)
        {
            rectTransform.sizeDelta += new Vector2(x, y);
        }
    }
}