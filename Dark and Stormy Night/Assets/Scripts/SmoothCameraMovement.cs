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

    [Space(5)]

    [Tooltip("Resets the players rotation to default when they awake")]
    [SerializeField]
    private bool m_bResetRotation = false;
    [Tooltip("Applies no shake effect to the camera")]
    [SerializeField]
    private bool m_bNoShake = false;
    
    [Space(5)]

    [FormerlySerializedAs("m_fSmoothDelay")]
    [Tooltip("How many frames until a mouse movement is removed from the list")]
    [SerializeField]
    private float m_fSmoothingDelay = 15;
    
    [Tooltip("Applies an offset to the look direction in the X axis")]
    public float m_fRotateOffset = 0;

    [Space(5)]

    [Tooltip("The maximum the player can rotate while sitting down")]
    [SerializeField]
    private Vector2 m_v2PublicSittingMaxRotation = new Vector2 (1, 179);

    [Tooltip("The mouse sensitivity in both the horizontal and vertical directions")]
    public Vector2 m_v2Sensitivity = new Vector2(2, 2);

    [Tooltip("The direction the player faces when the spawn in")]
    [SerializeField]
    private Vector2 m_v2StartRotation = new Vector2(85, 0);

    [Space(3)]

    [Tooltip("The minimum and maximum angle the player can look in the X direction in one frame")]
    [SerializeField]
    private Vector2 m_v2RotationRangeX = new Vector2(-360, 360);
    
    [Tooltip("The minimum and maximum angle the player can look in the Y direction in one frame")]
    [SerializeField]
    private Vector2 m_v2RotationRangeY = new Vector2(-75, 75);

    // The current X and Y rotation of the player
    private Vector2 m_v2Rotation;
    
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

        m_v2Rotation.x = m_v2StartRotation.x;
        m_v2Rotation.y = m_v2StartRotation.y;
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
                rotAverageX = 0f;
                rotAverageY = 0f;

                // Slows down camera movement if you're zoomed
                if (CameraMovement.s_bIsZoomed)
                {
                    m_v2Rotation.x += Input.GetAxis("Mouse X") * m_v2Sensitivity.x * 0.5f;
                    m_v2Rotation.y += Input.GetAxis("Mouse Y") * m_v2Sensitivity.y * 0.5f;
                }
                else
                {
                    m_v2Rotation.x += Input.GetAxis("Mouse X") * m_v2Sensitivity.x;
                    m_v2Rotation.y += Input.GetAxis("Mouse Y") * m_v2Sensitivity.y;
                }

                if (!m_bNoShake)
                {
                    m_v2Rotation.y += CameraMovement.s_CamShakeDirection.y;
                    m_v2Rotation.x += CameraMovement.s_CamShakeDirection.x;
                }

                // Clamps the y look to the range extents
                m_v2Rotation.y = ClampAngle(
                    m_v2Rotation.y, m_v2RotationRangeY.x, m_v2RotationRangeY.y);

                // Clamps by the chairs extents
                if (!Movement.s_bCanMove && !s_bIgnoreSittingRotation)
                    m_v2Rotation.x = ClampAngle(
                        m_v2Rotation.x, s_v2SittingMaxRotation.x, s_v2SittingMaxRotation.y);
                
                rotArrayX.Add(m_v2Rotation.x);
                rotArrayY.Add(m_v2Rotation.y);

                if (rotArrayY.Count >= m_fSmoothingDelay)
                    rotArrayY.RemoveAt(0);

                if (rotArrayX.Count >= m_fSmoothingDelay)
                    rotArrayX.RemoveAt(0);

                for (int j = 0; j < rotArrayY.Count; j++)
                    rotAverageY += rotArrayY[j];

                for (int i = 0; i < rotArrayX.Count; i++)
                    rotAverageX += rotArrayX[i];

                rotAverageY /= rotArrayY.Count;
                rotAverageX /= rotArrayX.Count;

                rotAverageX += m_fRotateOffset;

                //if (Movement.s_bCanMove == false)
                //  rotAverageY = ClampAngle(rotAverageY, sittingMaxRotation.x, sittingMaxRotation.y);

                // Clamps the x look to the range extents
                rotAverageX = ClampAngle(rotAverageX, m_v2RotationRangeX.x, m_v2RotationRangeX.y);

                Quaternion xQuaternion = Quaternion.AngleAxis(rotAverageX, Vector3.up);
                Quaternion yQuaternion = Quaternion.AngleAxis(rotAverageY, Vector3.left);

                transform.localRotation = s_qOriginalRotation * xQuaternion * yQuaternion;
            }
            else if (m_raAxes == RotationAxes.MouseX)
            {
                rotAverageX = 0f;

                m_v2Rotation.x += Input.GetAxis("Mouse X") * m_v2Sensitivity.x;

                rotArrayX.Add(m_v2Rotation.x);

                if (rotArrayX.Count >= m_fSmoothingDelay)
                    rotArrayX.RemoveAt(0);

                for (int i = 0; i < rotArrayX.Count; i++)
                    rotAverageX += rotArrayX[i];

                rotAverageX /= rotArrayX.Count;

                rotAverageX = ClampAngle(rotAverageX, m_v2RotationRangeX.x, m_v2RotationRangeX.y);

                Quaternion xQuaternion = Quaternion.AngleAxis(rotAverageX, Vector3.up);
                transform.localRotation = s_qOriginalRotation * xQuaternion;
            }
            else
            {
                rotAverageY = 0f;

                m_v2Rotation.y += Input.GetAxis("Mouse Y") * m_v2Sensitivity.y;

                rotArrayY.Add(m_v2Rotation.y);

                if (rotArrayY.Count >= m_fSmoothingDelay)
                    rotArrayY.RemoveAt(0);

                for (int j = 0; j < rotArrayY.Count; j++)
                    rotAverageY += rotArrayY[j];

                rotAverageY /= rotArrayY.Count;

                rotAverageY = ClampAngle(rotAverageY, m_v2RotationRangeY.x, m_v2RotationRangeY.y);

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
        m_v2Rotation.x = m_v2StartRotation.x;
        m_v2Rotation.y = m_v2StartRotation.y;
        rotArrayX.Clear();
        rotArrayY.Clear();
        CameraMovement.s_CamShakeDirection = Vector2.zero;
    }
}