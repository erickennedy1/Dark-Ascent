using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField]
    private List<DialogueData> _dialogueList = new List<DialogueData>();
    private Queue<DialogueData> _dialogueQueue = new Queue<DialogueData>();
    private bool _canTriggerDialogue = false;

    void Start(){
        //Transforma a List em Queue
        //A Unity não permite um SerializeField do tipo Queue
        foreach(DialogueData dialogue in _dialogueList){
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
        DialogueData dialogue = _dialogueQueue.Peek();
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
