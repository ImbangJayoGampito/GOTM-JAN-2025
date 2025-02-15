using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using NUnit.Framework.Constraints;
using Ink.Parsed;
public class DialogueUIController : MonoBehaviour
{
    DialogueGraph playingDialogue;
    VisualElement root;
    Label dialogueContent;
    Label subjectSpeaking;
    Button option;
    List<Button> otherOptions = new List<Button>();
    VisualElement optionContainer;
    string targetLine;
    Entity other;
    float typingSpeed = 0.05f;
    int dialogueIndex = 0;
    void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        root.style.display = DisplayStyle.None;
        // ScrollView text = root.Q<ScrollView>("texts");
        dialogueContent = root.Q<Label>("dialogue-text");
        subjectSpeaking = root.Q<Label>("subject");
        ScrollView optionTab = root.Q<ScrollView>("option-tab");
        optionContainer = optionTab.contentContainer;


    }
    void Update()
    {
        if (playingDialogue == null)
        {
            return;
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            End();
            return;
        }
        if (Input.GetMouseButtonDown((int)MouseButton.LeftMouse))
        {
            // List<Dialogue> current = playingDialogue.ReturnDialogue();

            if (otherOptions.Count > 0)
            {
                if (dialogueContent.text != targetLine)
                {
                    StopAllCoroutines();
                    GetNextLine(true);
                }
                return;
            }

            // Debug.Log("dialogue box meow = " + dialogueContent.text + "dialogue meow meow: " + current[dialogueIndex].message);
            // Debug.Log("length = " + current.Count + "index = " + dialogueIndex + " text = " + current[dialogueIndex].message);
            if (dialogueContent.text == targetLine)
            {
                dialogueIndex++;
                GetNextLine(false);
            }
            else
            {
                StopAllCoroutines();
                // dialogueContent.text = current[dialogueIndex].message;
                GetNextLine(true);
            }
        }
    }
    void GetNextLine(bool skipped)
    {
        List<Dialogue> current = playingDialogue.ReturnDialogue();
        if (current != null && dialogueIndex >= current.Count - 1 && otherOptions.Count <= 0)
        {
            ShowOptions(playingDialogue.ReturnDialogueOptions());
        }
        if (skipped)
        {
            dialogueContent.text = targetLine;
            // dialogueIndex++;
        }
        else
        {
            ShowDialogue(current[dialogueIndex]);

        }
        // dialogueContent.text = current[dialogueIndex].message;




    }
    void ShowDialogue(Dialogue dialogue)
    {
        // List<Dialogue> current = playingDialogue.ReturnDialogue();

        StartCoroutine(TypeLine(dialogue));


    }
    void CreateOption(string innerText, EventCallback<ClickEvent> callback)
    {
        Button button = new Button();
        button.text = innerText;
        button.style.color = Color.gray;
        button.style.fontSize = 25;
        button.RegisterCallback<ClickEvent>(callback); // Register the callback here
        otherOptions.Add(button);
        optionContainer.Add(button);
    }
    void ShowOptions(List<DialogueDirection> directions)
    {
        RemoveOptions();
        if (directions == null)
        {
            return;
        }

        if (directions.Count <= 0)
        {
            CreateOption("Goodbye!", ev => End());


        }

        foreach (DialogueDirection dir in directions)
        {
            CreateOption(dir.optionText, ev =>
            {
                NextDialogue(dir.targetNode);
            });

        }
    }
    private IEnumerator TypeLine(Dialogue line)
    {
        subjectSpeaking.text = GetSubject(other, line.subject);
        targetLine = line.message;
        Debug.Log("Playing dialogue = " + line.message);
        dialogueContent.text = "";
        foreach (char letter in line.message.ToCharArray())
        {
            dialogueContent.text += letter; // Add one letter at a time
            yield return new WaitForSeconds(typingSpeed); // Wait for the specified typing speed
        }

        // dialogueIndex++;
        // Debug.Log("current index = " + dialogueIndex);
    }
    void NextDialogue(string nextId)
    {
        Debug.Log(nextId);
        bool success = playingDialogue.Next(nextId);
        if (!success)
        {
            EmptyDialogue("There isn't really anything to talk right now", "I have to go!");
            return;
        }
        RemoveOptions();
        dialogueIndex = 0;
        GetNextLine(false);

    }
    public void EmptyDialogue(string message, string closingText)
    {
        Dialogue cantTalk = new Dialogue(message, DialogueSubject.Player);
        root.style.display = DisplayStyle.Flex;
        ShowDialogue(cantTalk);
        RemoveOptions();
        CreateOption(closingText, ev => End());

    }
    string GetSubject(Entity other, DialogueSubject subject)
    {
        string name = "";
        switch (subject)
        {
            case DialogueSubject.Player:
                name = "You";
                break;
            case DialogueSubject.Npc:
                name = other == null ? "Unknown" : other.stats.name;
                break;
            default:
                name = "Unknown";
                break;
        }
        return name;
    }

    public void Trigger(Entity other, DialogueGraph dialogue)
    {
        dialogueIndex = 0;
        if (dialogue == null)
        {
            EmptyDialogue("I can't talk to them!", "Return");
            return;
        }
        this.playingDialogue = dialogue;
        if (dialogue.dialogueFrame == null)
        {
            EmptyDialogue("There is nothing to talk about!", "Return");
            return;
        }
        if (playingDialogue != null)
        {
            playingDialogue.End();
        }
        this.other = other;

        root.style.display = DisplayStyle.Flex;
        playingDialogue.Trigger();
        GetNextLine(false);

    }
    void RemoveOptions()
    {
        foreach (Button option in otherOptions)
        {
            option.RemoveFromHierarchy();
        }
        otherOptions.Clear();
    }
    public void End()
    {
        if (playingDialogue != null)
        {
            playingDialogue.End();
        }
        RemoveOptions();
        playingDialogue = null;
        other = null;
        StopAllCoroutines();
        root.style.display = DisplayStyle.None;
        dialogueIndex = 0;
        dialogueContent.text = "";
        subjectSpeaking.text = "";
    }
}
