using System;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public InputSubscription playerInput;
    Global global;
    public GameObject cameraTarget;
    public int maxStamina;
    private int currentStamina;
    public int staminaRegenerationRate;
    public int GetCurrentStamina()
    {
        return this.currentStamina;
    }
    Cooldown staminaCooldown;
    Cooldown staminaExhaust;
    bool isRunning = false;
    bool onGround = true;
    Vector3 size;
    Entity entity;
    private Vector3 moveDirection = Vector3.zero;
    bool isSprinting;
    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<InputSubscription>();
        global = Global.Instance;
        if (global == null)
        {

            Debug.LogError("Global instance is null. Make sure the Global object is present in the scene.");

            return; // Exit early if global is null

        }
        entity = gameObject.GetComponent<Entity>();
        rb = gameObject.GetComponent<Rigidbody>();
        size = GetGameObjectSize();
        currentStamina = maxStamina;
        staminaExhaust = gameObject.AddComponent<Cooldown>();
        staminaExhaust.CooldownByRate((int)(entity.GetSprintingMultiplier() - 1) * staminaRegenerationRate);
        // Debug.Log((entity.GetSprintingMultiplier() - 1) * staminaRegenerationRate);
        staminaCooldown = gameObject.AddComponent<Cooldown>();
        staminaCooldown.CooldownByRate(staminaRegenerationRate);

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
    private void OnDrawGizmos()
    {
        Vector3 from = new(0, 0, 0);
        Vector3 to = new(5f, 5f, 5f);
        Gizmos.DrawLine(from, to);
    }
    // Update is called once per frame
    void Update()
    {


        if (entity.conditions[Entity.Condition.Frozen] == true || entity.conditions[Entity.Condition.Dead])
        {
            return;
        }
        transform.rotation = cameraTarget.transform.rotation;
        MovePlayer();
        JumpFunction();
        StaminaRegeneration();
        Sprint();
        Debug.Log(playerInput.Dragging);
    }
    void StaminaRegeneration()
    {
        if (isRunning)
        {
            return;
        }
        if (staminaCooldown.IsCooldown())
        {
            return;
        }
        // Debug.Log("stamina regen meow!");
        currentStamina = Math.Clamp(currentStamina + 1, 0, maxStamina);
    }
    void Sprint()
    {
        if (currentStamina <= 0)
        {
            isRunning = false;
            return;
        }
        isRunning = playerInput.Sprint;

        if (!staminaExhaust.IsCooldown() && isRunning)
        {
            // Debug.Log("meow tired!");
            currentStamina = Math.Clamp(currentStamina - 1, 0, maxStamina);
        }
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
        // Debug.DrawRay(transform.position, Vector3.down * size.y / 2 * 1.1f, Color.red);
        if (!onGround)
        {

            return;
        }
        int staminaCost = 10;
        if (playerInput.Jump && currentStamina >= staminaCost)
        {
            // rb.AddForce(Vector3.up * entity.stats.jumpStrength * 1000, ForceMode.Impulse);
            // currentStamina = Math.Clamp(currentStamina - staminaCost, 0, 100);
            // Debug.Log("meowww");

        }
    }

    void MovePlayer()
    {

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);


        Vector2 playerMove = playerInput.MoveInput;
        // Check for sprinting


        moveDirection = ((forward * playerMove.y) + (right * playerMove.x)).normalized;

        // Create the movement vector
        // Move the Rigidbody
    }
    void FixedUpdate()
    {
        float sprintMultiplier = isRunning ? entity.GetSprintingMultiplier() : 1;

        Vector3 desiredVelocity = moveDirection * entity.stats.movementSpeed * sprintMultiplier;

        // Debug.Log( entity.stats.movementSpeed * sprintMultiplier);
        if (desiredVelocity.magnitude <= entity.stats.movementSpeed * sprintMultiplier)
        {

            rb.AddForce(desiredVelocity * Time.fixedDeltaTime * 5.0f, ForceMode.VelocityChange);

        }
        else
        {
            desiredVelocity = desiredVelocity.normalized * entity.stats.movementSpeed;
            rb.linearVelocity = new Vector3(desiredVelocity.x, rb.linearVelocity.y, desiredVelocity.z);
        }
        rb.linearDamping = global.physics.friction * (onGround == true ? global.physics.ground_multiply : 1);
    }
}
