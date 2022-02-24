using UnityEngine;

public class Collectable : MonoBehaviour
{
    public Vector3 positionOffset, tempPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        positionOffset = transform.localPosition;
    }
    private void FixedUpdate()
    {
        tempPosition = positionOffset;
        tempPosition.y += Mathf.Sin(Time.fixedTime * Mathf.PI) * 0.25f;
        transform.localPosition = tempPosition;
    }
}
