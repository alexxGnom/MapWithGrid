using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;


namespace EnixanTest
{
    public class InputController : MonoBehaviour
    {
        #region Private fields

        private Vector3 _mouseOnTerrainPos = Vector3.zero;

        private Camera _camera;
        private Ray _ray;
        private RaycastHit[] _hits;

        private float _cameraZ;
        private Vector3 _cameraAngle;

        private int _lastFingerId = -1;

        private GraphicRaycaster _graphicRaycaster;

        private CameraController _camController;

        #endregion

        #region Interface

        public Vector3 GetMousePosOnTerrain()
        {
            return _mouseOnTerrainPos;
        }

        public bool CheckUIRaycast()
        {
            if (_graphicRaycaster == null)
                return false;

            PointerEventData pointerData = new PointerEventData(EventSystem.current);

            pointerData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            return results.Count > 0;
        }

        public Vector3 GetCameraSkewOffset()
        {
            return new Vector3(_cameraZ * Mathf.Tan(_cameraAngle.x * Mathf.Deg2Rad), _cameraZ * Mathf.Tan(_cameraAngle.y * Mathf.Deg2Rad), 0f);
        }

        #endregion

        private void Awake()
        {
            _camera = GetComponent<Camera>();

            _cameraZ = _camera.transform.localPosition.z;

            _cameraAngle = _camera.transform.localEulerAngles;

            _graphicRaycaster = FindObjectOfType<GraphicRaycaster>();

            _camController = CameraController.Instance;
        }

        private void Update()
        {
            CheckHits();

            if (!CheckUIRaycast())
            {
                if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
                    MobileUpdate();
                else
                    DesktopUpdate();
            }
        }

        private void MobileUpdate()
        {
            if (Input.touchCount == 1)
            {
                var touch0 = Input.GetTouch(0);

                if (_lastFingerId != touch0.fingerId || touch0.phase == TouchPhase.Began)
                {
                    _camController.StopDrag();
                }

                if (IsTouching(touch0))
                {
                    _camController.DragCamera(touch0.position);
                }
            }
            else if (Input.touchCount == 2)
            {
                CameraController.Instance.StopDrag();

                var touch0 = Input.GetTouch(0);
                var touch1 = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touch0.position - touch0.deltaPosition;
                Vector2 touchOnePrevPos = touch1.position - touch1.deltaPosition;

                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touch0.position - touch1.position).magnitude;

                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                _camController.AddZoom(deltaMagnitudeDiff);
            }

            _lastFingerId = Input.touchCount == 1 ? Input.GetTouch(0).fingerId : -1;
        }

        private void DesktopUpdate()
        {
            if (Input.GetMouseButton(0))
            {
                _camController.DragCamera(Input.mousePosition);
            }
            else
            {
                _camController.StopDrag();
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                _camController.AddZoom(-1);
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                _camController.AddZoom(1);
            }
        }

        private void CheckHits()
        {
            _ray = _camera.ScreenPointToRay(Input.mousePosition);

            _hits = Physics.RaycastAll(_ray, 10000.0f);
            if (_hits.Length > 0)
            {
                foreach (var hit in _hits)
                {
                    if (hit.collider.tag == "Terrain")
                        _mouseOnTerrainPos = new Vector3(hit.point.x, hit.point.z, hit.point.y);

                    if (Input.GetMouseButtonDown(0) && !CheckUIRaycast())
                    {
                        if ((hit.collider.tag == "Unit"))
                        {
                            var unit = hit.collider.gameObject.GetComponent<BaseUnit>();

                            if (unit != null)
                                Debug.Log(unit.ToString());
                        }
                    }
                }
            }
        }

        private bool IsTouching(Touch touch)
        {
            return touch.phase == TouchPhase.Began ||
                    touch.phase == TouchPhase.Moved ||
                    touch.phase == TouchPhase.Stationary;
        }
    }
}