using UnityEngine;

public class PlayerEntity : MonoBehaviour
{
    public EntityStats stats;
    private Rigidbody rb;
    private Entity entity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        entity = gameObject.AddComponent<Entity>();
        entity.Initialize(stats);
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void MovePlayer()
    {

    }
}
