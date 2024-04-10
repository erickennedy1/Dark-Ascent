using UnityEngine;
using UnityEngine.UI;

public class MinimapController : MonoBehaviour
{
    public bool isMapOn;

    void Start(){
        if(!isMapOn)
        {
            GetComponentInChildren<RawImage>().enabled = false;
        }
    }

    public void EnableMap()
    {
        isMapOn = true;
        GetComponentInChildren<RawImage>().enabled = true;
    }
}
