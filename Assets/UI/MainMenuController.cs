using UnityEngine;
using UnityEngine.UIElements; // Ensure this namespace is included for UI Toolkit

public class MainMenuController : MonoBehaviour
{
    private VisualElement ui;

    private Button playButton;
    private Button optionsButton;
    private Button quitButton;

    private void Awake()
    {
        // Get the root visual element from the UIDocument
        ui = GetComponent<UIDocument>().rootVisualElement;
    }

    private void OnEnable()
    {
        // Assign buttons by querying their names from the UI
        playButton = ui.Q<Button>("PlayButton");
        playButton.clicked += OnPlayButtonClicked;

        optionsButton = ui.Q<Button>("OptionsButton");
        optionsButton.clicked += OnOptionsButtonClicked;

        quitButton = ui.Q<Button>("QuitButton");
        quitButton.clicked += OnQuitButtonClicked;
    }



    private void OnQuitButtonClicked()
    {
        // Exit the application
        Application.Quit();

#if  UNITY_EDITOR 
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void OnOptionsButtonClicked()
    {
        Debug.Log("Options!");
    }

    private void OnPlayButtonClicked()
    {
        // Hide the current GameObject
        gameObject.SetActive(false);
    }
}
