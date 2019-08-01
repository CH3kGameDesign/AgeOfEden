using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraMovement : MonoBehaviour
{
    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float startRotX = 40;
    public float startRotY= 85;

    public float minimumY = -60F;
    public float maximumY = 60F;

    public float rotationX = 0F;
    public float rotationY = 0F;

    public Vector2 publicSittingMaxRotation = new Vector2 (-60, 60);
    public static Vector2 sittingMaxRotation;
    public float rotateOffset;

    private List<float> rotArrayX = new List<float>();
    float rotAverageX = 0F;

    private List<float> rotArrayY = new List<float>();
    float rotAverageY = 0F;

    public float frameCounter = 20;

    public static Quaternion originalRotation;

    public static float gravDirection = 0;

    public static float turnAroundValue = 0;

    // Called once before the first frame
    private void Start ()
    {
        sittingMaxRotation = publicSittingMaxRotation;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb)
            rb.freezeRotation = true;
        originalRotation = Quaternion.Euler(Vector3.zero);
        rotationX = startRotY;
        rotationY = startRotX;
    }

    // Called once per frame
    private void Update ()
    {
        if (!GravityTunnel.inGravTunnel)
            originalRotation = Quaternion.Euler(
                0, turnAroundValue, Mathf.Lerp(
                    originalRotation.eulerAngles.z, gravDirection, Time.deltaTime * 2));

        CameraUpdate();
    }

    /// <summary>
    /// Updates the camera movement
    /// </summary>
    public void CameraUpdate ()
    {
        if (CameraMovement.canMove)
        {
            if (axes == RotationAxes.MouseXAndY)
            {
                rotAverageY = 0f;
                rotAverageX = 0f;

                rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                rotationX += Input.GetAxis("Mouse X") * sensitivityX;

                rotationY += CameraMovement.camShakeDirection.y;
                rotationX += CameraMovement.camShakeDirection.x;

                rotationY = ClampAngle(rotationY, minimumY, maximumY);

                if (!Movement.canMove)
                    rotationX = ClampAngle(rotationX, sittingMaxRotation.x, sittingMaxRotation.y);
                
                rotArrayY.Add(rotationY);
                rotArrayX.Add(rotationX);

                if (rotArrayY.Count >= frameCounter)
                    rotArrayY.RemoveAt(0);

                if (rotArrayX.Count >= frameCounter)
                    rotArrayX.RemoveAt(0);

                for (int j = 0; j < rotArrayY.Count; j++)
                    rotAverageY += rotArrayY[j];

                for (int i = 0; i < rotArrayX.Count; i++)
                    rotAverageX += rotArrayX[i];

                rotAverageY /= rotArrayY.Count;
                rotAverageX /= rotArrayX.Count;

                rotAverageX += rotateOffset;

                //if (Movement.canMove == false)
                  //  rotAverageY = ClampAngle(rotAverageY, sittingMaxRotation.x, sittingMaxRotation.y);
                rotAverageX = ClampAngle(rotAverageX, minimumX, maximumX);

                Quaternion yQuaternion = Quaternion.AngleAxis(rotAverageY, Vector3.left);
                Quaternion xQuaternion = Quaternion.AngleAxis(rotAverageX, Vector3.up);

                transform.localRotation = originalRotation * xQuaternion * yQuaternion;
            }
            else if (axes == RotationAxes.MouseX)
            {
                rotAverageX = 0f;

                rotationX += Input.GetAxis("Mouse X") * sensitivityX;

                rotArrayX.Add(rotationX);

                if (rotArrayX.Count >= frameCounter)
                    rotArrayX.RemoveAt(0);

                for (int i = 0; i < rotArrayX.Count; i++)
                    rotAverageX += rotArrayX[i];

                rotAverageX /= rotArrayX.Count;

                rotAverageX = ClampAngle(rotAverageX, minimumX, maximumX);

                Quaternion xQuaternion = Quaternion.AngleAxis(rotAverageX, Vector3.up);
                transform.localRotation = originalRotation * xQuaternion;
            }
            else
            {
                rotAverageY = 0f;

                rotationY += Input.GetAxis("Mouse Y") * sensitivityY;

                rotArrayY.Add(rotationY);

                if (rotArrayY.Count >= frameCounter)
                    rotArrayY.RemoveAt(0);

                for (int j = 0; j < rotArrayY.Count; j++)
                    rotAverageY += rotArrayY[j];

                rotAverageY /= rotArrayY.Count;

                rotAverageY = ClampAngle(rotAverageY, minimumY, maximumY);

                Quaternion yQuaternion = Quaternion.AngleAxis(rotAverageY, Vector3.left);
                transform.localRotation = originalRotation * yQuaternion;
            }
        }
    }

    /// <summary>
    /// Clamps the angle given between two values
    /// </summary>
    /// <param name="pAngle">The value to be clamped</param>
    /// <param name="pMin">The minimum value allowed</param>
    /// <param name="pMax">The maximum value allowed</param>
    /// <returns></returns>
    public static float ClampAngle (float pAngle, float pMin, float pMax)
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
    /// <param name="pGravSnapDirection"></param>
    public static void gravSnap (float pGravSnapDirection)
    {
        originalRotation = Quaternion.Euler(0, 0, pGravSnapDirection);
        gravDirection = pGravSnapDirection;
    }

    /// <summary>
    /// Resets the rotation variables
    /// </summary>
    public void resetRotation ()
    {
        rotationX = 0;
        rotationY = 0;
        rotArrayX.Clear();
        rotArrayY.Clear();
    }
}