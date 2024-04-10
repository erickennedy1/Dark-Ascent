using UnityEngine;

public class Player : MonoBehaviour
{

    //Gostaria que essa tivesse
    public bool canMove = true;

    //Função chamada antes do Start, Caso já exista um player na Scene, o novo Player se auto Destroy
    //Caso contrário ele não é destruído ao carregar uma nova Scene
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Player");

        if (objs.Length > 1)
        {
            Debug.Log("Destroy, Player > 1: "+objs.Length);
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);
    }

    void Update()
    {
        //Debug Keybind
        //Único proposito de auxiliar no desenvolvimento
        //NÃO DEVE FAZER PARTE DO PLAYER FINAL
        if(Input.GetKeyDown(KeyCode.BackQuote))
        {
            GameController.instance.SceneCountTotal();
            GameController.instance.NextLevel();
        }
    }
    //Função que reseta informações do player, chamado pelo GameController
    public void ResetPlayer()
    {
        transform.position = Vector3.zero;
    }
}
