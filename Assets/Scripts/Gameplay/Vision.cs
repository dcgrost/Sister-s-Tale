using UnityEngine;

public class Vision : MonoBehaviour
{
    private Vector3 initialScale;

    private void Awake()
    {
        initialScale = transform.localScale;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Descubierto");
        }
    }
}
