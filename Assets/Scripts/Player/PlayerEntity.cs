using UnityEngine;

public class PlayerEntity : MonoBehaviour
{
    private Rigidbody rb;
    private Entity entity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        rb = gameObject.GetComponent<Rigidbody>();

        entity = gameObject.GetComponent<Entity>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void MovePlayer()
    {

    }
    public void DamagePlayer()
    {

    }

    private void OnCollisionEnter(Collision collision)

    {

        // Called when the collider/rigidbody enters the trigger
        Entity other = collision.gameObject.GetComponent<Entity>();
        if (other != null)
        {
            if (other.type == entity.type)
            {
                return;
            }
            // other.Damage(10);
            Debug.Log("Ouch! my health is now: " + other.stats.getHealth());
        }

    }
}
