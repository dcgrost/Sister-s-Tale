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
    public GameObject player;
    public GameObject particleFather;
    [Header("General")]
    public bool isTargetable;
    public float actionForce = 10f;
    [Header("Effects")]
    public GameObject pullParticle;
    public GameObject pushParticle;

    GameObject activeParticle;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            isTargetable = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        RotateParticles();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isTargetable = false;
        }
    }
    private void Start()
    {
        ParticleSelector();
    }
    private void FixedUpdate()
    {
        ParticleActiver();
    }
    private void ParticleSelector()
    {
        if (interactableType == InteractableType.Push)
        {
            activeParticle = pushParticle;
        }
        if (interactableType == InteractableType.Pull)
        {
            activeParticle = pullParticle;
        }
    }
    private void ParticleActiver()
    {
        if (isTargetable)
        {
            activeParticle.SetActive(true);
        }
        else
        {
            activeParticle.SetActive(false);
        }
    }
    private void RotateParticles()
    {
        particleFather.transform.LookAt(player.transform);
    }
    public void Action(Vector3 playerPosition)
    {
        Vector3 direction = new Vector3(gameObject.transform.position.x - playerPosition.x, 0, gameObject.transform.position.z - playerPosition.z);
        if (interactableType == InteractableType.Push)
        {
            this.gameObject.GetComponent<Rigidbody>().AddForce(direction.normalized * actionForce, ForceMode.Impulse);
        }
        if (interactableType == InteractableType.Pull)
        {
            this.gameObject.GetComponent<Rigidbody>().AddForce(-direction.normalized * actionForce, ForceMode.Impulse);
        }
    }
}
