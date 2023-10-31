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
    //[SerializeField]
    //private float m_maxDistance = 1.0f;
    [SerializeField]
    private float m_lerpSpeed = 0.05f;
    [SerializeField]
    private Vector2 m_zoomClampValues = new Vector2(2.0f, 15.0f);

    private float m_desiredDistance = 10.0f;

    public bool AcceptInput = true;


    private void Awake()
    {
    }


    // Update is called once per frame
    void Update()
    {
        if (!AcceptInput)
        {
            return;
        }
        UpdateHorizontalMovements();
        UpdateVerticalMovements();
        UpdateCameraScroll();
    }

    private void FixedUpdate()
    {
        FixedUpdateCameraLerp();
        MoveCameraInFrontOfObstructionsFUpdate();
    }

    private void FixedUpdateCameraLerp()
    {
        var desiredPosition = m_objectToLookAt.position - (transform.forward * m_desiredDistance);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, m_lerpSpeed);
    }

    private void UpdateHorizontalMovements()
    {
        float currentAngleX = Input.GetAxis("Mouse X") * m_rotationSpeed;
        transform.RotateAround(m_objectToLookAt.position, m_objectToLookAt.up, currentAngleX);
        //character's last rotation value
        ////Quaternion rotation = m_objectToLookAt.rotation;

        // rotate character
        ////float currentAngleX = Input.GetAxis("Mouse X") * m_rotationSpeed;
        ////m_objectToLookAt.transform.Rotate(0, currentAngleX, 0);

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
        /*if (Input.mouseScrollDelta.y != 0)
        {
            //TODO: Faire une verification selon la distance la plus proche ou la plus eloignee
            //Que je souhaite entre ma camera et mon objet

            //TODO: Lerp plutot que d'effectuer immediatement la translation de la camera
            
            // if camera is too near or too far from the character, or between min and max value, then scroll works
            
            Vector3 targetPosition = m_objectToLookAt.position - (transform.forward * 10.0f);
            float distance = Utils.Distance(targetPosition, m_objectToLookAt.position);
            if(distance <= m_maxDistance && distance >= m_minDistance ||
                distance > m_maxDistance && Input.mouseScrollDelta.y > 0 ||
                distance < m_minDistance && Input.mouseScrollDelta.y < 0)
            {
                //transform.position = Vector3.Slerp(transform.position, targetPosition, Input.mouseScrollDelta.y * Time.deltaTime);
                //transform.Translate(Vector3.forward * Input.mouseScrollDelta.y, Space.Self);
                transform.position = Vector3.Lerp(transform.position, targetPosition, m_lerpSpeed);
            }
        }*/
        m_desiredDistance += Input.mouseScrollDelta.y;
        m_desiredDistance = Mathf.Clamp(m_desiredDistance, m_zoomClampValues.x, m_zoomClampValues.y);
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
