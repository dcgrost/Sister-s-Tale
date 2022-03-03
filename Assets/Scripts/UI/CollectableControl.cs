using UnityEngine;
using UnityEngine.UI;

public class CollectableControl : MonoBehaviour
{
    public Text updateText;
    public int currentCollectable;

    private void Awake()
    {
        currentCollectable = 0;
    }
    public void NewCollectable()
    {
        currentCollectable++;
        updateText.text = currentCollectable.ToString();
    }
}
