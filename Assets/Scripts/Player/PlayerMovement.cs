using System;
using NUnit.Framework.Constraints;
using UnityEditor.Callbacks;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    Global global;
    public GameObject cameraTarget;
    bool onGround = true;
    Vector3 size;
    Entity entity;
    private Vector3 moveDirection = Vector3.zero;
    bool isSprinting;
    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        global = Global.Instance;
        if (global == null)
        {

            Debug.LogError("Global instance is null. Make sure the Global object is present in the scene.");

            return; // Exit early if global is null

        }
        entity = gameObject.GetComponent<Entity>();
        rb = gameObject.GetComponent<Rigidbody>();
        size = GetGameObjectSize();

    }
    Vector3 GetGameObjectSize()

    {
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            return collider.bounds.size; // Returns the size of the bounds
        }
        else
        {
            Debug.LogWarning("No Collider found on this GameObject.");
            return Vector3.zero; // Return zero if no collider is found
        }

    }
    // Update is called once per frame
    void Update()
    {

        if (entity.conditions[Entity.Condition.Frozen] == true)
        {
            return;
        }
        transform.rotation = cameraTarget.transform.rotation;
        MovePlayer();
        JumpFunction();
    }
    void JumpFunction()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, size.y / 2 * 1.05f))
        {
            onGround = true;
        }
        else
        {
            onGround = false;
        }
        Debug.Log(onGround);
        // Debug.DrawRay(transform.position, Vector3.down * size.y / 2 * 1.1f, Color.red);
        if (!onGround)
        {

            return;
        }
        if (Input.GetKeyDown(global.controller.jump))
        {
            rb.AddForce(Vector3.up * entity.stats.movementSpeed, ForceMode.Impulse);
            Debug.Log("meowww");

        }
    }

    void MovePlayer()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float moveX = 0;
        float moveZ = 0;

        if (Input.GetKey(global.controller.forward))
        {
            moveZ += 1; // Move forward
        }
        if (Input.GetKey(global.controller.backwards))
        {
            moveZ -= 1; // Move backward
        }
        if (Input.GetKey(global.controller.left))
        {
            moveX -= 1; // Move left
        }
        if (Input.GetKey(global.controller.right))
        {
            moveX += 1; // Move right
        }

        // Check for sprinting
        if (Input.GetKey(global.controller.sprint))
        {
            moveX *= global.physics.sprintMultiplier; // Apply sprint multiplier
            moveZ *= global.physics.sprintMultiplier; // Apply sprint multiplier
        }
        moveDirection = ((transform.forward * moveZ) + (transform.right * moveX)).normalized;
        // Create the movement vector
        rb.MovePosition(rb.position + moveDirection * Time.deltaTime * entity.stats.movementSpeed); // Move the Rigidbody
    }




}
