using System;
using UnityEngine;

public class Global : MonoBehaviour
{
    public class Controller
    {
        public KeyCode forward = KeyCode.W;

        public KeyCode backwards = KeyCode.S; // Fixed: S for backwards

        public KeyCode left = KeyCode.A;      // Fixed: A for left

        public KeyCode right = KeyCode.D;
        public KeyCode jump = KeyCode.Space;

        public KeyCode sprint = KeyCode.LeftShift;

        public int attack = 0;
        public int moveCamera = 1;

    }
    public Controller controller;
    public class SceneName
    {
        public String mainMenu = "Main Menu";
        public String settings = "Settings";

        public String overworld = "Overworld";
        public String demonWorld = "Demon World";
    }
    SceneName sceneName;
    public class Physics
    {
        public float friction = 2f;
        public float ground_multiply = 3.0f;
        public float sprintMultiplier = 1.25f;
    }
    public class Mechanic
    {
        public float iFrame = 0.5f;
    }
    public Mechanic mechanic;
    public Physics physics;
    private static Global instance;
    public static Global Instance { get { return instance; } }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            controller = new Controller();
            physics = new Physics();
            mechanic = new Mechanic();
            sceneName = new SceneName();
        }
    }
    private void OnDestroy()
    {
        // Clear the instance if this instance is destroyed
        if (instance == this)
        {
            instance = null;
        }
    }
}
