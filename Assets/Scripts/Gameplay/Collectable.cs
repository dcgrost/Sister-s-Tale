using UnityEngine;

public class Collectable : MonoBehaviour
{
    public GameObject collectableControl;
    public Vector3 positionOffset, tempPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            collectableControl.GetComponent<CollectableControl>().NewCollectable();
            collectableControl.GetComponent<CollectableControl>().PlaySound();
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
