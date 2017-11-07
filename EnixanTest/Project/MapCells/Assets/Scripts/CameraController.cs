using UnityEngine;

namespace EnixanTest
{
    public class CameraController : MonoSingleton<CameraController>
    {
        #region Unity properties

        [SerializeField]
        private Transform startCameraPoint;

        [SerializeField]
        private float minZoomSize = 5f;

        [SerializeField]
        private float maxZoomSize = 20f;

        [SerializeField]
        private float startZoomSize = 10f;

        [SerializeField]
        private float zoomSpeed = 3f;

        [SerializeField]
        private float moveSpeed = 3f;

        [SerializeField]
        private float moveLerpValue = 7f;

        [SerializeField]
        private float zoomLerpValue = 3f;

        [SerializeField]
        private InputController inputController;

        [SerializeField]
        private Rect cameraVisibleRect;

        [SerializeField]
        private float maxToleranceForCameraMovement = 5f;

        [SerializeField]
        private float epsilon = 0.1f;

        #endregion

        #region Private fields

        private Camera _camera;

        private float _ascpectRatio;

        private Rect _cameraVisibleRect;

        private Vector2 _targetCameraPosition;
        private Vector2 _initialCameraPosition;
        private Vector3 _initialTouchPosition;

        private float _targetZoomSize;

        private float _cameraHigh;

        private bool _drag;

        #endregion

        #region Public properties

        public bool IsMovingToTarget { get; private set; }
        public bool IsZooming { get; private set; }

        #endregion

        #region Interface

        public void AddZoom(float zoom)
        {
            _targetZoomSize += zoom * zoomSpeed;
        }

        public void DragCamera(Vector3 point)
        {
            if (!_drag)
            {
                _initialTouchPosition = point;
                _initialCameraPosition = transform.localPosition;
                _drag = true;
            }
            else
            {
                Vector3 delta = _camera.ScreenToViewportPoint(_initialTouchPosition - point);

                Vector3 newPos = _initialCameraPosition;

                newPos += delta * moveSpeed;

                _targetCameraPosition = newPos;
            }

        }

        public void StopDrag()
        {
            _drag = false;
        }

        #endregion

        protected override sealed void Init()
        {
            base.Init();

            _camera = GetComponent<Camera>();

            _targetZoomSize = startZoomSize;

            _cameraHigh = transform.localPosition.z;

            _ascpectRatio = (float) Screen.width / Screen.height;

            _cameraVisibleRect = cameraVisibleRect;
        }

        private void Start()
        {
            _targetCameraPosition = startCameraPoint.localPosition - inputController.GetCameraSkewOffset();
        }

        private void LateUpdate()
        {
            UpdateCameraZoom();
            UpdateCameraPosition();
        }

        private void ClampTargetPosition()
        {
            if (_drag)
            {
                _targetCameraPosition.x = Mathf.Clamp(_targetCameraPosition.x, _cameraVisibleRect.xMin - maxToleranceForCameraMovement, _cameraVisibleRect.xMax + maxToleranceForCameraMovement);
                _targetCameraPosition.y = Mathf.Clamp(_targetCameraPosition.y, _cameraVisibleRect.yMin - maxToleranceForCameraMovement, _cameraVisibleRect.yMax + maxToleranceForCameraMovement);
            }
            else
            {
                _targetCameraPosition.x = Mathf.Clamp(_targetCameraPosition.x, _cameraVisibleRect.xMin, _cameraVisibleRect.xMax);
                _targetCameraPosition.y = Mathf.Clamp(_targetCameraPosition.y, _cameraVisibleRect.yMin, _cameraVisibleRect.yMax);
            }
        }

        private void ClampCameraZoom()
        {
            _targetZoomSize =  Mathf.Clamp(_targetZoomSize, minZoomSize, maxZoomSize);
        }

        private void UpdateCameraPosition()
        {
            ClampTargetPosition();
            var currentDistance = Vector2.Distance(transform.localPosition, _targetCameraPosition);

            IsMovingToTarget = currentDistance > epsilon;

            if (IsMovingToTarget)
            {
                var newPosition = Vector2.Lerp(transform.localPosition, _targetCameraPosition, moveLerpValue * Time.deltaTime);
                transform.localPosition = new Vector3(newPosition.x, newPosition.y, _cameraHigh);
            }
        }

        private void UpdateCameraZoom()
        {
            
            ClampCameraZoom();

            var zoomSizeDelta = Mathf.Abs(_targetZoomSize - _camera.fieldOfView);
            IsZooming = zoomSizeDelta > epsilon;

            if (IsZooming)
            {
                _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, _targetZoomSize, zoomLerpValue * Time.deltaTime);
                RecalculateCameraRectLimits();
            }
        }

        private void RecalculateCameraRectLimits()
        {
            var zoom = maxZoomSize - _camera.fieldOfView;

            var x = cameraVisibleRect.x - _ascpectRatio * zoom;
            var y = cameraVisibleRect.y - zoom;

            var width = cameraVisibleRect.width + 2 * zoom * _ascpectRatio;
            var heigh = cameraVisibleRect.height + 2 * zoom;

            _cameraVisibleRect = new Rect(x, y, width, heigh);
        }
    }
}
