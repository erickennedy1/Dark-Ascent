using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField]
    private List<Dialogue> _dialogueList = new List<Dialogue>();
    private Queue<Dialogue> _dialogueQueue = new Queue<Dialogue>();
    private bool _canTriggerDialogue = false;

    void Start(){
        //Transforma a List em Queue
        //A Unity não permite um SerializeField do tipo Queue
        foreach(Dialogue dialogue in _dialogueList){
            _dialogueQueue.Enqueue(dialogue);
        }
    }

    void Update() {
        if(_canTriggerDialogue && !DialogueManager.instance.isDialogueOn){
            if(Input.GetKeyDown(KeyCode.F))
                TriggerDialogue();
        }
    }

    public void TriggerDialogue()
    {
        Dialogue dialogue = _dialogueQueue.Peek();
        if(!dialogue.loop){
            dialogue = _dialogueQueue.Dequeue();
        }

        DialogueManager.instance.StartDialogue(dialogue);

        //Se não houver mais dialogs disponíveis, desativa o GameObject
        if(_dialogueQueue.Count == 0)
            gameObject.SetActive(false);
    }

    #region Enable/Disable Triggers

    public void EnableTriggerDialogue()
    {
        _canTriggerDialogue = true;
    }

    public void DisableTriggerDialogue()
    {
        _canTriggerDialogue = false;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "PlayerCollider")
            EnableTriggerDialogue();
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "PlayerCollider")
            DisableTriggerDialogue();
    }

    #endregion
}
