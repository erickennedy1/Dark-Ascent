using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Light2D light2d;

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
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            Destroy(gameObject);
            return;
        }
    }

        public void ResetPlayer()
    {
        transform.position = Vector3.zero;
    }

    public void SetLightState(bool state)
    {
        light2d.enabled = state;
    }
}
