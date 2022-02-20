using UnityEngine;

public class EnemyTower : MonoBehaviour
{
    void Update()
    {
        transform.GetChild(0).gameObject.transform.Rotate(0f, 0f, 50 * Time.deltaTime);
    }
}
