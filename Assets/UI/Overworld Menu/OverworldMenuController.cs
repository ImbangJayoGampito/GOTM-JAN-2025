using UnityEngine;
using UnityEngine.UIElements;
public class OverworldMenuController : MonoBehaviour
{
    public RenderTexture minimapRender;
    public Camera minimapCamera;
    VisualElement ui;
    VisualElement healthBar;
    TimeFunction stopwatch;
    VisualElement healthFrame;
    VisualElement staminaBar;
    VisualElement staminaFrame;
    Entity playerEntity;
    PlayerMovement movementStats;
    Label timeLabel;
    Image minimapImage;
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
        stopwatch = gameObject.AddComponent<TimeFunction>();

    }
    // Update is called once per frame
    void Update()
    {
        float staminaPercent = (float)movementStats.GetCurrentStamina() / (float)movementStats.maxStamina * 100.0f;
        float healthPercent = (float)playerEntity.stats.getHealth() / (float)playerEntity.stats.maxHealth * 100.0f;
        timeLabel.text = TimeFunction.StopwatchFormatter(stopwatch.timePassed);
        staminaBar.style.width = new Length(staminaPercent, LengthUnit.Percent);
        healthBar.style.width = new Length(healthPercent, LengthUnit.Percent);
        Vector3 playerPos = movementStats.gameObject.transform.position;
        minimapCamera.transform.position = new Vector3(playerPos.x, playerPos.y + 20, playerPos.z);
    }
    void OnEnable()
    {
        healthFrame = ui.Q<VisualElement>("HealthFrame");
        healthBar = ui.Q<VisualElement>("HealthBar");
        staminaFrame = ui.Q<VisualElement>("StaminaFrame");
        staminaBar = ui.Q<VisualElement>("StaminaBar");
        minimapImage = ui.Q<Image>("minimap-image");
        timeLabel = ui.Q<Label>("time-check");
        minimapImage.image = minimapRender;
    }

    Texture2D ConvertRenderTextureToTexture2D(RenderTexture renderTexture)

    {

        // Create a new Texture2D with the same dimensions as the RenderTexture

        Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
        RenderTexture old_rt = RenderTexture.active;

        // Set the active RenderTexture to the one we want to read from

        RenderTexture.active = renderTexture;


        // Read the pixels from the RenderTexture

        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);

        texture2D.Apply();
        RenderTexture.active = old_rt;
        return texture2D;

    }
}
