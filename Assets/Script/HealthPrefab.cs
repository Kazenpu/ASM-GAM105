using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class HealthPrefab : MonoBehaviour
{
    public GameObject healthBar;
    public Vector3 offset;
    public float maxHealth;
    private float currentHealth;

    private void Awake()
    {
        CreatHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
    }
    void CreatHealthBar()
    {
        Instantiate(healthBar, this.transform.position + offset, Quaternion.identity, this.transform);
    }
}
