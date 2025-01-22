using System;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyInfoController : MonoBehaviour
{
    Camera cam;
    VisualElement root;
    Entity entity;
    Label entityNameLabel;
    ProgressBar healthBar;
    Vector2 size = new Vector2(450, 100);
    int maxLength = 100;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = cam.ScreenPointToRay(mousePos);
        Vector2 newPosition = RuntimePanelUtils.ScreenToPanel(root.panel, new Vector2(mousePos.x, Screen.height - mousePos.y));
        // newPosition.x -= root.resolvedStyle.width / 2; // Center the element
        // newPosition.y = Screen.height - newPosition.y; // Flip Y coordinate
        root.transform.position = new Vector2(newPosition.x - root.resolvedStyle.width / 2, newPosition.y - root.resolvedStyle.height / 2);
        // Debug.DrawRay(cam.transform.position, mousePos - cam.transform.position, Color.red);

        RaycastHit hit;
        // Debug.Log("x = " + mousePos.x + " y = " + (Screen.height - mousePos.y));
        if (Physics.Raycast(ray, out hit, 100))
        {
            GameObject objHit = hit.collider.gameObject;
            // Debug.Log(hit.collider.gameObject);
            Entity entity = ObjectHit(objHit, Input.mousePosition);
            if (entity == null)
            {
                root.style.display = DisplayStyle.None;
                return;
            }

            Enemy enemy = objHit.GetComponent<Enemy>();
            Color color = entityNameLabel.resolvedStyle.color;
            String name = entity.stats.name;
            if (enemy != null)
            {
                color = enemy.GetDisplayColor();
                name = enemy.GetEnemyName();
            }
            entityNameLabel.text = name;
            entityNameLabel.style.color = color;
            float healthPercent = (float)entity.stats.getHealth() / (float)entity.stats.maxHealth * 100f;
           //  Debug.Log("current health = " + healthPercent);
            // Debug.Log("max health = " )
            healthBar.value = healthPercent;
            root.style.display = DisplayStyle.Flex;
        }
    }
    public Entity ObjectHit(GameObject hit, Vector2 mousePos)
    {
        Entity entity = hit.GetComponent<Entity>();

        PlayerEntity playerEntity = hit.GetComponent<PlayerEntity>();
        if (playerEntity != null)
        {
            return null;
        }
        return entity;
    }
    void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        root.style.display = DisplayStyle.None;
        root.style.width = new Length(size.x, LengthUnit.Pixel);
        root.style.height = new Length(size.y, LengthUnit.Pixel);
        root.style.position = Position.Absolute;
        entityNameLabel = root.Q<Label>("Name");
        healthBar = root.Q<ProgressBar>("HealthBar");
    }
}
