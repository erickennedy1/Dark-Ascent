using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static event Action EventEndDialogue;
    public static DialogueManager instance;
    private Queue<Dialogue> _dialogues;

    private Dialogue _currentDialogue;

    [Tooltip("Tempo de espera em segundos, entre cada letra do dialogo")]
    private float textSpeed = 0.05f;

    public bool isDialogueOn {get; private set;} = false;
    private bool _isTyping = false;

    [Header("UI Text")]
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [Tooltip("Não é um botão, é o texto que indica qual botão apertar")]
    [SerializeField] private TextMeshProUGUI _nextButton;
    [SerializeField] private Image _icon;
    [SerializeField] private RectTransform _iconRect;

    [Header("Animatior")]
    [SerializeField] private Animator animator;

    [Header("Data")]
    [Tooltip("Lista contendo personagens, com seus nomes e icones")]
    public CharactersData characters;

    void Awake() {
        if(instance == null)
            instance = this;
        else{
            Destroy(gameObject);
            return;
        }
    }

    void Start() {
        _dialogues = new Queue<Dialogue>();
    }

    void Update(){
        if(isDialogueOn){
            if(Input.GetKeyDown(KeyCode.F)){
                DisplayNextSentence();
            }
        }
    }

    public void StartDialogue(DialogueData dialogue)
    {
        //Pause Player
        GameController.instance.PlayerAcao(false);

        //Setup Dialogue
        _dialogues.Clear();
        _nameText.text = ""; //Set Name
        _dialogueText.text = "";
        foreach(Dialogue dialog in dialogue.sentencesList) //Set All Texts
            _dialogues.Enqueue(dialog);

        //Setup Dialogue On
        StartCoroutine(SetDialogueState(true, DisplayNextSentence));
    }

    public void DisplayNextSentence()
    {
        if(!_isTyping && _dialogues.Count == 0){
            EndDialogue();
            return;
        }

        //Se o dialogo ainda estiver sendo escrito
        if(_isTyping)
        {
            StopAllCoroutines();
            _isTyping = false;
            _dialogueText.text = _currentDialogue.sentence;
        }
        //Nova sentença
        else{
            _currentDialogue = _dialogues.Dequeue();

            _nameText.text = _currentDialogue.name; //Set Name
            //Set Icon
            Character character = characters.characters.Find(x => x.name == _currentDialogue.name);
            _icon.sprite = character.icon;
            _iconRect.sizeDelta = character.iconRect.sizeDelta;

            StartCoroutine(TypeSentence());
        }
    }

    IEnumerator TypeSentence ()
    {
        OnTypeStart();

        _dialogueText.text = "";
        foreach(char letter in _currentDialogue.sentence.ToCharArray())
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
        //Unpause Player
        EventEndDialogue?.Invoke();

        //Setup Dialogue Off
        _nextButton.gameObject.SetActive(false);
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
