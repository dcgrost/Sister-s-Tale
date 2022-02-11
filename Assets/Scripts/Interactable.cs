using UnityEngine;

public enum InteractableType
{
    Push,
    Pull
}
public class Interactable : MonoBehaviour
{
    [Header("References")]
    public InteractableType interactableType;
    [Header("General")]
    public bool isTargetable;
    public float actionForce = 10f;
    [Header("Effects")]
    public GameObject activeObject;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            isTargetable = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isTargetable = false;
        }
    }
    private void FixedUpdate()
    {
        if (isTargetable)
        {
            activeObject.SetActive(true);
        }
        else
        {
            activeObject.SetActive(false);
        }
    }
    public void Action(Vector3 playerPosition)
    {
        if(interactableType == InteractableType.Push)
        {
            this.gameObject.GetComponent<Rigidbody>().AddForce((this.gameObject.transform.position - playerPosition).normalized * actionForce, ForceMode.Impulse);
        }
        if (interactableType == InteractableType.Pull)
        {
            this.gameObject.GetComponent<Rigidbody>().AddForce(-(this.gameObject.transform.position - playerPosition).normalized * actionForce, ForceMode.Impulse);
        }
    }
}
