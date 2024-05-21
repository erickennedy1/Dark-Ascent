using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Controll : MonoBehaviour
{
    [SerializeField] private List<Canvas> UIs;
    // Start is called before the first frame update
    public void SetStateCanvasList(bool state){
        //Desativa todos os canvas
        foreach(Canvas canvas in UIs){
            canvas.enabled = state;
        }
    }
}
