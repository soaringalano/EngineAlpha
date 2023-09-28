using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform m_objectToLookAt;
    [SerializeField]
    private float m_rotationSpeed = 1.0f;
    [SerializeField]
    private Vector2 m_clampingXRotationValues = Vector2.zero;
    [SerializeField]
    private float m_minDistance = 1.0f;
    [SerializeField]
    private float m_maxDistance = 1.0f;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHorizontalMovements();
        UpdateVerticalMovements();
        UpdateCameraScroll();
    }

    private void FixedUpdate()
    {
        MoveCameraInFrontOfObstructionsFUpdate();
    }

    private void UpdateHorizontalMovements()
    {
        //character's last rotation value
        Quaternion rotation = m_objectToLookAt.rotation;

        // rotate character
        float currentAngleX = Input.GetAxis("Mouse X") * m_rotationSpeed;
        m_objectToLookAt.transform.Rotate(0, currentAngleX, 0);

        // the camera should be always behind the character, and this method seems easy and works well
        transform.RotateAround(m_objectToLookAt.position, transform.up, m_objectToLookAt.transform.rotation.y - rotation.y);

        // tried many ways but all failed

        //rotation = Quaternion.RotateTowards(transform.rotation, m_objectToLookAt.transform.rotation, m_rotationSpeed);
        //rotation.x = transform.rotation.x;
        //rotation.z = transform.rotation.z;
        //rotation.w = transform.rotation.w;
        //transform.SetPositionAndRotation(transform.position, rotation);

        // calculate new position of camera
        //float distance = Vector3.Distance(transform.position, m_objectToLookAt.position);
        //Vector3 a = (m_objectToLookAt.transform.rotation.eulerAngles - m_lastFrameTargetRoation);

        //Vector3 newpos = a.normalized * distance;
        //transform.position = newpos;
        //transform.LookAt(m_objectToLookAt.transform.position);

        //transform.position = Vector3.Slerp(transform.position, newpos, m_rotationSpeed * Time.deltaTime);
        //transform.LookAt(m_objectToLookAt.position);
        
        //transform.RotateAround(m_objectToLookAt.position, m_objectToLookAt.up, currentAngleX);
        //transform.RotateAround(m_objectToLookAt.position, Vector3.up, m_objectToLookAt.transform.eulerAngles.y - m_lastFrameTargetRoation.y);

        //m_lastFrameTargetRoation = m_objectToLookAt.transform.rotation.eulerAngles;
    }

    /**
     * code from exercise
     */
    private void UpdateVerticalMovements()
    {
        float currentAngleY = Input.GetAxis("Mouse Y") * m_rotationSpeed;
        float eulersAngleX = transform.rotation.eulerAngles.x;

        float comparisonAngle = eulersAngleX + currentAngleY;

        comparisonAngle = ClampAngle(comparisonAngle);

        if ((currentAngleY < 0 && comparisonAngle < m_clampingXRotationValues.x)
            || (currentAngleY > 0 && comparisonAngle > m_clampingXRotationValues.y))
        {
            return;
        }
        transform.RotateAround(m_objectToLookAt.position, transform.right, currentAngleY);
    }

    private void UpdateCameraScroll()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            //TODO: Faire une verification selon la distance la plus proche ou la plus eloignee
            //Que je souhaite entre ma camera et mon objet

            //TODO: Lerp plutot que d'effectuer immediatement la translation de la camera
            /**
             * if camera is too near or too far from the character, or between min and max value, then scroll works
             */
            Vector3 targetPosition = Vector3.forward * Input.mouseScrollDelta.y + transform.position;
            float distance = Utils.Distance(targetPosition, m_objectToLookAt.position);
            if(distance <= m_maxDistance && distance >= m_minDistance ||
                distance > m_maxDistance && Input.mouseScrollDelta.y > 0 ||
                distance < m_minDistance && Input.mouseScrollDelta.y < 0)
            {
                //transform.position = Vector3.Slerp(transform.position, targetPosition, Input.mouseScrollDelta.y * Time.deltaTime);
                transform.Translate(Vector3.forward * Input.mouseScrollDelta.y, Space.Self);
            }
        }
    }

    private void MoveCameraInFrontOfObstructionsFUpdate()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        RaycastHit hit;

        var vecteurDiff = transform.position - m_objectToLookAt.position;
        var distance = vecteurDiff.magnitude;

        if (Physics.Raycast(m_objectToLookAt.position, vecteurDiff, out hit, distance, layerMask))
        {
            //if an object is between camera and character, then camera roll close to the character, but... see else
            Debug.DrawRay(m_objectToLookAt.position, vecteurDiff.normalized * hit.distance, Color.yellow);
            transform.SetPositionAndRotation(hit.point, transform.rotation);
        }
        else
        {
            // if the distance is too close, without this code block, the camera won't roll back to the proper distance
            Debug.DrawRay(m_objectToLookAt.position, vecteurDiff, Color.white);
            if (distance < m_minDistance)
            {
                Vector3 off = vecteurDiff.normalized * m_minDistance;
                transform.SetPositionAndRotation(off + m_objectToLookAt.position, transform.rotation);
                //Vector3.Slerp(transform.position, off + m_objectToLookAt.position, off.magnitude);
            }
        }
    }

    private float ClampAngle(float angle)
    {
        if (angle > 180)
        {
            angle -= 360;
        }
        return angle;
    }
}
