using UnityEngine;

public class EnemyTower : MonoBehaviour
{
    public float turnSpeed = 50f;
    void Update()
    {
        transform.GetChild(0).gameObject.transform.Rotate(0f, 0f, turnSpeed * Time.deltaTime);
    }
}
