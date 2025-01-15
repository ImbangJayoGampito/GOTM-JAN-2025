using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueNode
{
    public String dialogueId;
    public List<string> dialogue;
    public List<DialogueDirection> dialogueOptions;
    public bool AppendEdge(DialogueDirection nextDialogue)
    {
        int index = dialogueOptions.FindIndex(a => a.targetNode == nextDialogue.targetNode);
        if (index < 0)
        {
            dialogueOptions.Add(nextDialogue);
            return true;
        }
        else
        {
            return false;
        }
    }
}
public class DialogueDirection
{
    public String optionText;
    public String targetNode;
    public DialogueDirection(String targetNode, String optionText)
    {
        this.optionText = optionText;
        this.targetNode = targetNode;
    }
}
public class DialogueGraph
{
    Dictionary<String, DialogueNode> dialogues;
    DialogueNode currentDialogue;
    public void StartDialogue(DialogueNode startingDialogue)
    {
        if (!dialogues.ContainsKey(startingDialogue.dialogueId)) // Check if the dialogue already exists
        {
            dialogues.Add(startingDialogue.dialogueId, startingDialogue);
            currentDialogue = startingDialogue; // Set the current dialogue
        }
    }
    public bool AddDialogueOption(DialogueNode existingDialogue, DialogueNode dialogueToAdd, String dialogueShow)
    {
        DialogueDirection toAdd = new DialogueDirection(dialogueToAdd.dialogueId, dialogueShow);
        bool success = existingDialogue.AppendEdge(toAdd);
        if (success)
        {
            dialogues.Add(dialogueToAdd.dialogueId, dialogueToAdd);
        }
        return success;
    }
    public bool IsEnding()
    {
        return currentDialogue.dialogueOptions?.Count == 0;
    }
    public List<string> ReturnDialogue()
    {
        return currentDialogue?.dialogue;
    }
    public List<DialogueDirection> ReturnDialogueOptions()
    {
        return currentDialogue?.dialogueOptions;
    }
    public bool Next(String name)
    {
        bool foundNext = dialogues.ContainsKey(name);
        if (foundNext)
        {
            currentDialogue = dialogues[name];
        }
        return foundNext;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

}
