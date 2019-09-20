using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SmoothCameraMovement : MonoBehaviour
{
    public static bool s_bIgnoreSittingRotation = false;
    public static bool s_bFlipOnReset = false;
    public static float s_fGravDirection = 0;
    public static float s_fTurnAroundValue = 0;
    public static Vector2 s_v2SittingMaxRotation;
    public static Quaternion s_qOriginalRotation;

    private enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 };
    
    [Tooltip("The input axes used for camera rotation")]
    [SerializeField]
    private RotationAxes m_raAxes = RotationAxes.MouseXAndY;

    [Space(3)]

    [FormerlySerializedAs("m_bReset")]
    [Tooltip("Resets the players rotation to default when they awake")]
    [SerializeField]
    private bool m_bResetRotation = false;
    [Tooltip("Applies no shake effect to the camera")]
    [SerializeField]
    private bool m_bNoShake = false;

    [Space(3)]

    [Tooltip("The mouse sensitivity in the X direction")]
    public float m_fSensitivityX = 2;
    [Tooltip("The mouse sensitivity in the Y direction")]
    public float m_fSensitivityY = 2;

    [Space(3)]

    [Tooltip("The X direction the player faces when the spawn in")]
    [SerializeField]
    private float m_fStartRotX = 0;
    [Tooltip("The Y direction the player faces when the spawn in")]
    [SerializeField]
    private float m_fStartRotY= 85;

    [Space(3)]

    [Tooltip("The minimum the player can rotate in the X direction in one frame")]
    [SerializeField]
    private float m_fMinimumX = -360;
    [Tooltip("The maximum the player can rotate in the X direction in one frame")]
    [SerializeField]
    private float m_fMaximumX = 360;

    [Space(3)]

    [Tooltip("The minimum the player can rotate in the Y direction in one frame")]
    [SerializeField]
    private float m_fMinimumY = -75;
    [Tooltip("The maximum the player can rotate in the Y direction in one frame")]
    [SerializeField]
    private float m_fMaximumY = 75;

    [Space(3)]

    [Tooltip("The maximum the player can rotate while sitting down")]
    [SerializeField]
    private Vector2 m_v2PublicSittingMaxRotation = new Vector2 (1, 179);
    [Tooltip("")]
    public float m_fRotateOffset;

    [Space(3)]

    [FormerlySerializedAs("frameCounter")]
    [Tooltip("How many frames until a mouse movement is removed from the list")]
    [SerializeField]
    private float m_fSmoothDelay = 15;

    // The current rotation of the player in the X direction
    private float m_fRotationX = 0F;
    // The current rotation of the player in the Y direction
    private float m_fRotationY = 0F;
    
    private List<float> rotArrayX = new List<float>();
    private float rotAverageX = 0F;

    private List<float> rotArrayY = new List<float>();
    private float rotAverageY = 0F;

    // Called once before the first frame
    private void Awake()
    {
        s_bIgnoreSittingRotation = false;
        s_v2SittingMaxRotation = m_v2PublicSittingMaxRotation;
        Rigidbody rb = GetComponent<Rigidbody>();
        s_qOriginalRotation = Quaternion.Euler(Vector3.zero);

        if (rb)
            rb.freezeRotation = true;
        
        if (m_bResetRotation)
            resetRotation();

        if (s_bFlipOnReset)
        {
            s_bFlipOnReset = false;
            s_fTurnAroundValue = 180;
        }
        else
            s_fTurnAroundValue = 0;

        m_fRotationX = m_fStartRotY;
        m_fRotationY = m_fStartRotX;
    }

    // Called once per frame
    private void Update()
    {
        //if (!GravityTunnel.s_bInGravityTunnel)
        //    originalRotation = Quaternion.Euler(0, turnAroundValue, Mathf.Lerp(
        //        originalRotation.eulerAngles.z, gravDirection, Time.deltaTime * 2));

        s_qOriginalRotation = Quaternion.Euler(0, s_fTurnAroundValue, Mathf.Lerp(
            s_qOriginalRotation.eulerAngles.z, s_fGravDirection, Time.deltaTime * 2));

        CameraUpdate();
    }

    /// <summary>
    /// Updates the camera movement
    /// </summary>
    private void CameraUpdate()
    {
        if (CameraMovement.s_CanMove)
        {
            if (m_raAxes == RotationAxes.MouseXAndY)
            {
                rotAverageY = 0f;
                rotAverageX = 0f;

                // Slows down camera movement if you're zoomed
                if (CameraMovement.s_bIsZoomed)
                {
                    m_fRotationY += Input.GetAxis("Mouse Y") * m_fSensitivityY * 0.5f;
                    m_fRotationX += Input.GetAxis("Mouse X") * m_fSensitivityX * 0.5f;
                }
                else
                {
                    m_fRotationY += Input.GetAxis("Mouse Y") * m_fSensitivityY;
                    m_fRotationX += Input.GetAxis("Mouse X") * m_fSensitivityX;
                }

                if (!m_bNoShake)
                {
                    m_fRotationY += CameraMovement.s_CamShakeDirection.y;
                    m_fRotationX += CameraMovement.s_CamShakeDirection.x;
                }

                m_fRotationY = ClampAngle(m_fRotationY, m_fMinimumY, m_fMaximumY);

                if (!Movement.canMove && !s_bIgnoreSittingRotation)
                    m_fRotationX = ClampAngle(m_fRotationX, s_v2SittingMaxRotation.x, s_v2SittingMaxRotation.y);
                
                rotArrayY.Add(m_fRotationY);
                rotArrayX.Add(m_fRotationX);

                if (rotArrayY.Count >= m_fSmoothDelay)
                    rotArrayY.RemoveAt(0);

                if (rotArrayX.Count >= m_fSmoothDelay)
                    rotArrayX.RemoveAt(0);

                for (int j = 0; j < rotArrayY.Count; j++)
                    rotAverageY += rotArrayY[j];

                for (int i = 0; i < rotArrayX.Count; i++)
                    rotAverageX += rotArrayX[i];

                rotAverageY /= rotArrayY.Count;
                rotAverageX /= rotArrayX.Count;

                rotAverageX += m_fRotateOffset;
                
                //if (Movement.canMove == false)
                //  rotAverageY = ClampAngle(rotAverageY, sittingMaxRotation.x, sittingMaxRotation.y);
                rotAverageX = ClampAngle(rotAverageX, m_fMinimumX, m_fMaximumX);

                Quaternion yQuaternion = Quaternion.AngleAxis(rotAverageY, Vector3.left);
                Quaternion xQuaternion = Quaternion.AngleAxis(rotAverageX, Vector3.up);

                transform.localRotation = s_qOriginalRotation * xQuaternion * yQuaternion;
            }
            else if (m_raAxes == RotationAxes.MouseX)
            {
                rotAverageX = 0f;

                m_fRotationX += Input.GetAxis("Mouse X") * m_fSensitivityX;

                rotArrayX.Add(m_fRotationX);

                if (rotArrayX.Count >= m_fSmoothDelay)
                    rotArrayX.RemoveAt(0);

                for (int i = 0; i < rotArrayX.Count; i++)
                    rotAverageX += rotArrayX[i];

                rotAverageX /= rotArrayX.Count;

                rotAverageX = ClampAngle(rotAverageX, m_fMinimumX, m_fMaximumX);

                Quaternion xQuaternion = Quaternion.AngleAxis(rotAverageX, Vector3.up);
                transform.localRotation = s_qOriginalRotation * xQuaternion;
            }
            else
            {
                rotAverageY = 0f;

                m_fRotationY += Input.GetAxis("Mouse Y") * m_fSensitivityY;

                rotArrayY.Add(m_fRotationY);

                if (rotArrayY.Count >= m_fSmoothDelay)
                    rotArrayY.RemoveAt(0);

                for (int j = 0; j < rotArrayY.Count; j++)
                    rotAverageY += rotArrayY[j];

                rotAverageY /= rotArrayY.Count;

                rotAverageY = ClampAngle(rotAverageY, m_fMinimumY, m_fMaximumY);

                Quaternion yQuaternion = Quaternion.AngleAxis(rotAverageY, Vector3.left);
                transform.localRotation = s_qOriginalRotation * yQuaternion;
            }
        }
    }

    /// <summary>
    /// Clamps the angle given between two values
    /// </summary>
    /// <param name="pAngle">The value to be clamped</param>
    /// <param name="pMin">The minimum value allowed</param>
    /// <param name="pMax">The maximum value allowed</param>
    /// <returns>The clamped angle</returns>
    public static float ClampAngle(float pAngle, float pMin, float pMax)
    {
        pAngle = pAngle % 360;

        if ((pAngle >= -360f) && (pAngle <= 360f))
        {
            if (pAngle < -360f)
                pAngle += 360f;

            if (pAngle > 360f)
                pAngle -= 360f;
        }

        return Mathf.Clamp(pAngle, pMin, pMax);
    }

    /// <summary>
    /// Snaps the gravity to a set direction
    /// </summary>
    /// <param name="pGravSnapDirection">The desired gravity direction</param>
    public static void GravSnap(float pGravSnapDirection)
    {
        s_qOriginalRotation = Quaternion.Euler(0, 0, pGravSnapDirection);
        s_fGravDirection = pGravSnapDirection;
    }

    /// <summary>
    /// Resets the rotation variables
    /// </summary>
    public void resetRotation()
    {
        m_fRotationX = m_fStartRotX;
        m_fRotationY = m_fStartRotY;
        rotArrayX.Clear();
        rotArrayY.Clear();
        CameraMovement.s_CamShakeDirection = Vector2.zero;
    }
}