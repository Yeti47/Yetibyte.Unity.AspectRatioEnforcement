using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yetibyte.Unity.AspectRatioEnforcement {

    public class AspectRatioEnforcer : MonoBehaviour {

        #region Constants

        private const string ERR_MSG_NO_MAIN_CAM = "Could not find the main camera.";

        private const string BG_CAMERA_NAME = "__YB_AspectRatioBgCam";

        private const float EPSILON = 0.005f;
        private const int CULLING_MASK_ID_NOTHING = 0;

        #endregion

        #region Fields

        [SerializeField]
        private Camera _targetCamera;

        [SerializeField]
        private Color _backgroundColor = Color.black;

        [SerializeField]
        private AspectRatio _targetAspectRatio = AspectRatio.Ratio16To9;

        private Camera _backgroundCam;

        protected int? _previousScreenWidth;
        protected int? _previousScreenHeight;

        #endregion

        #region Events

        public event EventHandler<ScreenSizeChangedEventArgs> ScreenSizeChanged;

        public event EventHandler<CameraUpdatedEventArgs> CameraUpdated;

        #endregion

        #region Props

        public Camera TargetCamera {
            get => _targetCamera;
            set {

                _targetCamera = value;

                if (_targetCamera == null)
                    FindMainCamera();

                UpdateCameraSize();

            }
        }

        public AspectRatio TargetAspectRatio {
            get => _targetAspectRatio;
            set {
                _targetAspectRatio = value;
                UpdateCameraSize();
            }
        }

        #endregion

        #region Methods

        private void Awake() {
            
        }

        private void Start() {

            InitializeBackgroundCamera();
            UpdateCameraSize();
        
        }

        private void Update() {

            HandleScreenResize();
            
        }

        private bool HandleScreenResize() {

            bool isInitialCheck = !_previousScreenHeight.HasValue || !_previousScreenWidth.HasValue;

            bool screenSizeChanged = !isInitialCheck && (Screen.width != _previousScreenWidth || Screen.height != _previousScreenHeight);

            if (screenSizeChanged) {
                OnScreenSizeChanged(_previousScreenWidth.Value, _previousScreenHeight.Value);
            }

            _previousScreenWidth = Screen.width;
            _previousScreenHeight = Screen.height;

            return screenSizeChanged;

        }

        private void InitializeBackgroundCamera() {

            _backgroundCam = GameObject.Find(BG_CAMERA_NAME)?.GetComponent<Camera>();

            if(_backgroundCam == null) {

                GameObject bgCamObj = new GameObject(BG_CAMERA_NAME);
                bgCamObj.AddComponent<Camera>();

                _backgroundCam = bgCamObj.GetComponent<Camera>();

            }

            _backgroundCam.backgroundColor = _backgroundColor;
            _backgroundCam.enabled = true;
            _backgroundCam.clearFlags = CameraClearFlags.SolidColor;
            _backgroundCam.orthographic = true;
            _backgroundCam.depth = int.MinValue;
            _backgroundCam.cullingMask = CULLING_MASK_ID_NOTHING;
            
        }

        private void FindMainCamera() {

            _targetCamera = Camera.main;

            if (_targetCamera == null)
                throw new AspectRatioEnforcementException(ERR_MSG_NO_MAIN_CAM);


        }

        private void UpdateCameraSize() {

            if (_targetCamera == null)
                FindMainCamera();

            _backgroundCam.orthographicSize = _targetCamera.orthographicSize;

            Rect originalPixelRect = _targetCamera.pixelRect;

            float screenHeight = Screen.height;
            float screenWidth = Screen.width;

            float originalAspectRatioValue = screenWidth / screenHeight;

            float desiredHeight = screenHeight / originalAspectRatioValue * _targetAspectRatio;
            float desiredWidth = screenWidth / originalAspectRatioValue * _targetAspectRatio;

            // e. g. the actual aspect ratio is 16:9, but we want 4:3
            if((originalAspectRatioValue - _targetAspectRatio.Value) > EPSILON ) { 

                float effectiveWidth = screenWidth * desiredHeight / screenHeight;

                _targetCamera.pixelRect = new Rect(0 + (screenWidth - effectiveWidth)/2, 0, (float)Math.Floor(effectiveWidth), (float)Math.Floor(screenHeight));


            }
            // e. g. the actual aspect ratio is 4:3, but we want 16:9
            else if (_targetAspectRatio.Value - originalAspectRatioValue > EPSILON) { 

                float effectiveHeight = screenHeight * screenWidth / desiredWidth;

                _targetCamera.pixelRect = new Rect(0, 0 + (screenHeight - effectiveHeight) / 2, (float)Math.Floor(screenWidth), (float)Math.Floor(effectiveHeight));


            }
            else {

                _targetCamera.pixelRect = new Rect(0, 0, screenWidth, screenHeight);

            }

            OnCameraUpdated(originalPixelRect);
            
        }

        protected virtual void OnScreenSizeChanged(Vector2 originalScreenSize) => OnScreenSizeChanged(originalScreenSize.x, originalScreenSize.y);

        protected virtual void OnScreenSizeChanged(float originalScreenWidth, float originalScreenHeight) {

            Debug.Log($"{nameof(AspectRatioEnforcer)}: Screen resize detected. Raising ScreenSizeChanged event...");

            var handler = ScreenSizeChanged;
            handler?.Invoke(this, new ScreenSizeChangedEventArgs(originalScreenWidth, originalScreenHeight, Screen.width, Screen.height));

            UpdateCameraSize();

        }

        protected virtual void OnCameraUpdated(Rect originalPixelRect) {

            Debug.Log($"{nameof(AspectRatioEnforcer)}: Camera aspect ratio adjusted to {_targetAspectRatio}.");

            var handler = CameraUpdated;
            handler?.Invoke(this, new CameraUpdatedEventArgs(_targetCamera, originalPixelRect, _targetCamera.pixelRect, originalPixelRect.width / originalPixelRect.height, _targetAspectRatio));
            
        }

        #endregion

    }


}
