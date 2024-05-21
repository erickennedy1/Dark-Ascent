using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubStartAnimation : MonoBehaviour
{
    [SerializeField] private DialogueTrigger _dialogueTrigger;
    [SerializeField] private Animator _animatorDark;
    [SerializeField] private UI_Controll _controllUIs;
    // Start is called before the first frame update
    void Start()
    {
        //Desativa todos os canvas
        _controllUIs.SetStateCanvasList(false);

        //Associa o Step02 ao evento fim do dialogo
        DialogueManager.EventEndDialogue += Step02;
        //Começa animação inicial
        StartCoroutine(Step01());
    }

    

    IEnumerator Step01(){
        //Escuridão por um tempo
        yield return new WaitForSeconds(4);
        //Animação do player ressurgindo
        //animatorPlayer
        yield return new WaitForSeconds(3);
        //Começa Diálogo ???
        _dialogueTrigger.TriggerDialogue();
    }

    public void Step02(){
        //Chamado ao terminar o diálogo ???
        DialogueManager.EventEndDialogue -= Step02;
        DialogueManager.EventEndDialogue += Step04;
        StartCoroutine(Step03());
    }

    IEnumerator Step03(){
        yield return new WaitForSeconds(3);
        //Ilumina tudo ao redor
        _animatorDark.SetBool("startTransition", true);
        yield return new WaitForSeconds(5);
        //Começa o diálogo Introdutório
        DialogueManager.EventEndDialogue += GameController.instance.EnablePlayerInput;
        _dialogueTrigger.TriggerDialogue();
    }

    public void Step04(){
        //Chamado ao terminar o diálogo Introdutório
        DialogueManager.EventEndDialogue -= Step04;
        _controllUIs.SetStateCanvasList(true);
    }
}

