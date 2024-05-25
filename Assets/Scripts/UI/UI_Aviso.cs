using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Aviso : MonoBehaviour
{
    public static UI_Aviso Instance { get; private set; }

    public GameObject Aviso;
    public TMP_Text Texto;
    public List<string> text = new List<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Texto.text = string.Empty;
    }

    public void SetAviso(bool isActive, string message)
    {
        Aviso.SetActive(isActive);
        Texto.text = isActive ? message : string.Empty;
    }
}
