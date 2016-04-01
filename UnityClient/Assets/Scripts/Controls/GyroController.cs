using UnityEngine;

namespace Assets.Scripts.Controls
{
    /// <summary>
    ///     Gyroscope controller that works with any device orientation.
    /// </summary>
    public class GyroController : MonoBehaviour
    {
        #region [Private fields]

        private bool _gyroEnabled = true;
        private const float LowPassFilterFactor = 0.1f;

        private readonly Quaternion _baseIdentity = Quaternion.Euler(90, 0, 0);

        private Quaternion _cameraBase = Quaternion.identity;
        private Quaternion _calibration = Quaternion.identity;
        private Quaternion _baseOrientation = Quaternion.Euler(90, 0, 0);
        private Quaternion _baseOrientationRotationFix = Quaternion.identity;

        private Quaternion _referanceRotation = Quaternion.identity;
        [SerializeField] private readonly bool _debug = false;

        #endregion

        #region [Unity events]

        protected void Start()
        {
            AttachGyro();
        }

        public void Toggle()
        {
            enabled = !enabled;
        }

        protected void Update()
        {
            if (!_gyroEnabled)
                return;
            transform.rotation = Quaternion.Slerp(transform.rotation,
                _cameraBase*(ConvertRotation(_referanceRotation*Input.gyro.attitude)*GetRotFix()), LowPassFilterFactor);
        }

        protected void OnGUI()
        {
            if (!_debug)
                return;

            GUILayout.Label("Orientation: " + Screen.orientation);
            GUILayout.Label("Calibration: " + _calibration);
            GUILayout.Label("Camera base: " + _cameraBase);
            GUILayout.Label("input.gyro.attitude: " + Input.gyro.attitude);
            GUILayout.Label("transform.rotation: " + transform.rotation);

            if (GUILayout.Button("On/off gyro: " + Input.gyro.enabled, GUILayout.Height(100)))
            {
                Input.gyro.enabled = !Input.gyro.enabled;
            }

            if (GUILayout.Button("On/off gyro controller: " + _gyroEnabled, GUILayout.Height(100)))
            {
                if (_gyroEnabled)
                {
                    DetachGyro();
                }
                else
                {
                    AttachGyro();
                }
            }

            if (GUILayout.Button("Update gyro calibration (Horizontal only)", GUILayout.Height(80)))
            {
                UpdateCalibration(true);
            }

            if (GUILayout.Button("Update camera base rotation (Horizontal only)", GUILayout.Height(80)))
            {
                UpdateCameraBaseRotation(true);
            }

            if (GUILayout.Button("Reset base orientation", GUILayout.Height(80)))
            {
                ResetBaseOrientation();
            }

            if (GUILayout.Button("Reset camera rotation", GUILayout.Height(80)))
            {
                transform.rotation = Quaternion.identity;
            }
        }

        #endregion

        #region [Public methods]

        /// <summary>
        ///     Attaches gyro controller to the transform.
        /// </summary>
        private void AttachGyro()
        {
            _gyroEnabled = true;
            ResetBaseOrientation();
            UpdateCalibration(true);
            UpdateCameraBaseRotation(true);
            RecalculateReferenceRotation();
            Input.gyro.enabled = true;
        }

        /// <summary>
        ///     Detaches gyro controller from the transform
        /// </summary>
        private void DetachGyro()
        {
            _gyroEnabled = false;
        }

        #endregion

        #region [Private methods]

        /// <summary>
        ///     Update the gyro calibration.
        /// </summary>
        private void UpdateCalibration(bool onlyHorizontal)
        {
            if (onlyHorizontal)
            {
                var fw = (Input.gyro.attitude)*(-Vector3.forward);
                fw.z = 0;
                if (fw == Vector3.zero)
                {
                    _calibration = Quaternion.identity;
                }
                else
                {
                    _calibration = (Quaternion.FromToRotation(_baseOrientationRotationFix*Vector3.up, fw));
                }
            }
            else
            {
                _calibration = Input.gyro.attitude;
            }
        }

        /// <summary>
        ///     Update the camera base rotation.
        /// </summary>
        /// <param name='onlyHorizontal'>
        ///     Only y rotation.
        /// </param>
        private void UpdateCameraBaseRotation(bool onlyHorizontal)
        {
            if (onlyHorizontal)
            {
                var fw = transform.forward;
                fw.y = 0;
                if (fw == Vector3.zero)
                {
                    _cameraBase = Quaternion.identity;
                }
                else
                {
                    _cameraBase = Quaternion.FromToRotation(Vector3.forward, fw);
                }
            }
            else
            {
                _cameraBase = transform.rotation;
            }
        }

        /// <summary>
        ///     Converts the rotation from right handed to left handed.
        /// </summary>
        /// <returns>
        ///     The result rotation.
        /// </returns>
        /// <param name='q'>
        ///     The rotation to convert.
        /// </param>
        private static Quaternion ConvertRotation(Quaternion q)
        {
            return new Quaternion(q.x, q.y, -q.z, -q.w);
        }

        /// <summary>
        ///     Gets the rot fix for different orientations.
        /// </summary>
        /// <returns>
        ///     The rot fix.
        /// </returns>
        private Quaternion GetRotFix()
        {
            return Quaternion.identity;
        }

        /// <summary>
        ///     Recalculates reference system.
        /// </summary>
        private void ResetBaseOrientation()
        {
            _baseOrientationRotationFix = GetRotFix();
            _baseOrientation = _baseOrientationRotationFix*_baseIdentity;
        }

        /// <summary>
        ///     Recalculates reference rotation.
        /// </summary>
        private void RecalculateReferenceRotation()
        {
            _referanceRotation = Quaternion.Inverse(_baseOrientation)*Quaternion.Inverse(_calibration);
        }

        #endregion
    }
}