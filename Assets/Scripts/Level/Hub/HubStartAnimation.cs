using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubStartAnimation : MonoBehaviour
{
    [SerializeField] private DialogueTrigger _dialogueTrigger;
    [SerializeField] private Animator _animatorDark;
    [SerializeField] private UI_Controll _controllUIs;

    [SerializeField] private TutorialController _tutorial;
    // Start is called before the first frame update
    void Start()
    {
        //Desativa todos os canvas
        _controllUIs.SetStateCanvasList(false);

        //Começa animação inicial
        StartCoroutine(Step01());
    }

    IEnumerator Step01(){
        //Escuridão por um tempo
        yield return new WaitForSeconds(3);
        //Associa o Step02 ao evento fim do dialogo
        DialogueManager.EventEndDialogue += Step02;
        //Começa Diálogo ???
        _dialogueTrigger.TriggerDialogue();
    }

    public void Step02(){
        //Chamado ao terminar o diálogo ???
        DialogueManager.EventEndDialogue -= Step02;
        SoundTrackManager.Instance.PlayMusic("Hub");
        StartCoroutine(Step03());
    }

    IEnumerator Step03(){
        yield return new WaitForSeconds(1);
        //Ilumina tudo ao redor
        _animatorDark.SetBool("startTransition", true);
        yield return new WaitForSeconds(5);
        //Começa o diálogo Introdutório
        DialogueManager.EventEndDialogue += Step04;
        _dialogueTrigger.TriggerDialogue();
    }

    public void Step04(){
        //Chamado ao terminar o diálogo Introdutório
        DialogueManager.EventEndDialogue -= Step04;
        _tutorial.gameObject.SetActive(true);
        _tutorial.EventEndTutorial += Step05;
        _tutorial.StartTutorial();
    }

    public void Step05(){
        _tutorial.EventEndTutorial -= Step05;
        EndAnimation();
    }

    public void EndAnimation()
    {
        //Adiciona um event ao terminar o dialogo para Liberar o player
        DialogueManager.EventEndDialogue += GameController.instance.EnablePlayerInput;

        //Ativa as UIs
        _controllUIs.SetStateCanvasList(true);

        //Libera o Player
        GameController.instance.EnablePlayerInput();
    }

    private void OnDestroy() {
        DialogueManager.EventEndDialogue -= Step02;
        DialogueManager.EventEndDialogue -= Step04;
        _tutorial.EventEndTutorial -= Step05;
    }
}

