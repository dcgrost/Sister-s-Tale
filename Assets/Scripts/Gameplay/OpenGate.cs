using UnityEngine;

public class OpenGate : MonoBehaviour
{
    public GameObject gate;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Interactable")
        {
            other.gameObject.SetActive(false);
            gate.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
