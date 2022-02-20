using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public GameObject player;
    public FixedTouchField touchField;

    public Vector3 positionOffset = new Vector3(0.0f, 2.0f, -2.5f);
    public Vector3 angleOffset = new Vector3(0.0f, 0.0f, 0.0f);
    [Header("Follow Independent Rotation")]
    public float minPitch = -30.0f;
    public float maxPitch = 30.0f;
    public float rotationSpeed = 5.0f;
    private float angleX = 0.0f;
    private Quaternion initialState;

    private void Start()
    {
        initialState = transform.localRotation;
    }
    void LateUpdate()
    {
        if (player.GetComponent<Player>().isMoving)
        {
            ResetRotation();
        }
        else
        {
            CameraRotation();
        }
    }
    void CameraRotation()
    {
        float mx = 0, my = 0;
        mx = touchField.TouchDist.x * Time.deltaTime;
        my = touchField.TouchDist.y * Time.deltaTime;

        Quaternion initialRotation = Quaternion.Euler(angleOffset);
        Vector3 eu = transform.localRotation.eulerAngles;
        angleX -= my * rotationSpeed;
        angleX = Mathf.Clamp(angleX, minPitch, maxPitch);
        eu.y += mx * rotationSpeed;
        Quaternion newRot = Quaternion.Euler(angleX, eu.y, 0.0f) * initialRotation;
        transform.localRotation = newRot;
    }
    private void ResetRotation()
    {
        angleX = 0f;
        transform.localRotation = Quaternion.Lerp(transform.localRotation, initialState, 10 * Time.deltaTime);
    }
}