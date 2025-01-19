using UnityEngine;

public class PlayerEntity : MonoBehaviour
{
    private Rigidbody rb;
    private Entity entity;
    DialogueUIController dialogueUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        rb = gameObject.GetComponent<Rigidbody>();

        entity = gameObject.GetComponent<Entity>();
        dialogueUI = GameObject.Find("Dialogue").GetComponent<DialogueUIController>();
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
    public DialogueGraph GetDialogue(GameObject objCollision)
    {
        Entity other = objCollision.GetComponent<Entity>();
        if (other == null)
        {
            return null;
        }
        DialogueGraph graph = objCollision.GetComponent<DialogueGraph>();
        return graph;
    }
    private void OnCollisionEnter(Collision collision)
    {

        DialogueGraph dialogueGraph = GetDialogue(collision.gameObject);
        if (dialogueGraph != null)
        {
            dialogueUI.Trigger(collision.gameObject.GetComponent<Entity>(), dialogueGraph);
        }


    }
    private void OnCollisionExit(Collision collision)
    {
        DialogueGraph dialogueGraph = GetDialogue(collision.gameObject);
        if (dialogueGraph != null)
        {
            dialogueUI.End();
        }
    }
}
