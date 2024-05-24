using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Controll : MonoBehaviour
{
    [SerializeField] private List<Canvas> UIs;
    // Start is called before the first frame update

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void SetStateCanvasList(bool state){
        //Desativa todos os canvas
        foreach(Canvas canvas in UIs){
            canvas.enabled = state;
        }
    }
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            Destroy(gameObject);
            return;
        }
    }
}
