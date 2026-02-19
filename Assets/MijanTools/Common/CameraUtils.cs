using UnityEngine;

namespace MijanTools.Common
{
    public static class CameraUtils
    {
        private static Camera _mainCamera;

        public static Camera MainCamera
        {
            get
            {
                if (_mainCamera == null)
                {
                    _mainCamera = Camera.main;
                }
                return _mainCamera;
            }
        }

        public static Rect GetMainCameraOrthrographicBounds()
        {
            var height = MainCamera.orthographicSize * 2f;
            var width = MainCamera.aspect * height;
            var position = MainCamera.transform.position;
            var bounds = new Rect(position.x - width / 2f, position.y - height / 2f, width, height);
            return bounds;
        }
    }
}