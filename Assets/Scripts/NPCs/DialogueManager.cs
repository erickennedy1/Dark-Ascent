using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    private Queue<string> _sentences;

    private string _currentSentence;

    [Tooltip("Tempo de espera em segundos, entre cada letra do dialogo")]
    private float textSpeed = 0.05f;

    public bool isDialogueOn {get; private set;} = false;
    private bool _isTyping = false;

    [Header("UI Text")]
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [Tooltip("Não é um botão, é o texto que indica qual botão apertar")]
    [SerializeField] private TextMeshProUGUI _nextButton;

    [Header("Animatior")]
    [SerializeField] private Animator animator;

    void Awake() {
        if(instance == null)
            instance = this;
        else{
            Destroy(gameObject);
            return;
        }
    }

    void Start() {
        _sentences = new Queue<string>();
    }

    void Update(){
        if(isDialogueOn){
            if(Input.GetKeyDown(KeyCode.F)){
                DisplayNextSentence();
            }
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        //Setup Dialogue
        _sentences.Clear();
        _nameText.text = dialogue.name; //Set Name
        _dialogueText.text = "";
        foreach(string sentence in dialogue.sentences) //Set All Texts
            _sentences.Enqueue(sentence);

        //Setup Dialogue On
        StartCoroutine(SetDialogueState(true, DisplayNextSentence));
    }

    public void DisplayNextSentence()
    {
        if(!_isTyping && _sentences.Count == 0){
            EndDialogue();
            return;
        }

        //Se o dialogo ainda estiver sendo escrito
        if(_isTyping)
        {
            StopAllCoroutines();
            _isTyping = false;
            _dialogueText.text = _currentSentence;
        }else{
            _currentSentence = _sentences.Dequeue();
            StartCoroutine(TypeSentence());
        }
    }

    IEnumerator TypeSentence ()
    {
        OnTypeStart();

        _dialogueText.text = "";
        foreach(char letter in _currentSentence.ToCharArray())
        {
            _dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

        OnTypeEnd();
    }

    public void OnTypeStart(){
        _nextButton.gameObject.SetActive(false);
        _isTyping = true;
    }
    public void OnTypeEnd(){
        _nextButton.gameObject.SetActive(true);
        _isTyping = false;
    }    

    public void EndDialogue()
    {
        //Setup Dialogue Off        
        StartCoroutine(SetDialogueState(false));
    }

    //Espera a animação do Dialogo antes e atualizar o State
    IEnumerator SetDialogueState(bool state)
    {
        animator.SetBool("isOpen", state);
        yield return new WaitForSeconds(state?0.2f:0.40f);
        isDialogueOn = state;
    }

    //Espera a animação do Dialogo antes e atualizar o State e chamar a proxima função
    IEnumerator SetDialogueState(bool state, Action doLast)
    {
        animator.SetBool("isOpen", state);
        yield return new WaitForSeconds(state?0.2f:0.40f);
        isDialogueOn = state;
        doLast();
    }
}
