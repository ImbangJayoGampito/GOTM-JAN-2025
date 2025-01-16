using UnityEngine;
using UnityEngine.UIElements;
public class OverworldMenuController : MonoBehaviour
{
    VisualElement ui;
    VisualElement healthBar;

    VisualElement healthFrame;
    VisualElement staminaBar;
    VisualElement staminaFrame;
    Entity playerEntity;
    PlayerMovement movementStats;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        ui = GetComponent<UIDocument>().rootVisualElement;
    }

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        playerEntity = player.GetComponent<Entity>();
        movementStats = player.GetComponent<PlayerMovement>();
    }
    // Update is called once per frame
    void Update()
    {
        float staminaPercent = (float)movementStats.GetCurrentStamina() / (float)movementStats.maxStamina * 100.0f;

        staminaBar.style.width = new Length(staminaPercent, LengthUnit.Percent);
    }
    void OnEnable()
    {
        healthFrame = ui.Q<VisualElement>("HealthFrame");
        healthBar = ui.Q<VisualElement>("HealthBar");
        staminaFrame = ui.Q<VisualElement>("StaminaFrame");
        staminaBar = ui.Q<VisualElement>("StaminaBar");
    }

}
