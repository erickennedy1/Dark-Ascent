using UnityEngine;

public class CameraController : MonoBehaviour
{

    public static CameraController instance;

    public Room currentRoom;
    private float smoothTime;
    private float unpauseDistance; //Distância mínima para despausar o player
    private Vector3 velocity = Vector3.zero;

    public bool isTransitioning = false;

    void Awake()
    {
        instance = this;
    }

    void FixedUpdate()
    {
        UpdatePosition();
    }

    void UpdatePosition(){
        if(currentRoom == null)
            return;

        Vector3 desiredPosition = GetCameraTargetPosition();
        if(!isTransitioning && transform.position != desiredPosition)
        {
            isTransitioning = true;
            GameController.instance.isGamePaused=true;

            if(!currentRoom.hasBattle || currentRoom.isClear)
            {
                smoothTime = 0.15f;
                unpauseDistance = 0.035f;
            }
            else
            {
                smoothTime = 0.30f;
                unpauseDistance = 0.01f;
            }
            
        }

        if(isTransitioning)
        {                
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);

            transform.position = smoothedPosition;

            // Verifica se a câmera está em transição
            float distance = Vector3.Distance(transform.position,desiredPosition);

            if(distance <= unpauseDistance)
                GameController.instance.isGamePaused = false;

            if (distance <= 0.01f) {
                transform.position = desiredPosition;
                isTransitioning = false;
            }
        }
        
    }

    Vector3 GetCameraTargetPosition(){
        if(currentRoom == null)
            return Vector3.zero;

        Vector3 targetPos = currentRoom.GetRoomCenter();
        targetPos.z = transform.position.z;

        return targetPos;
    }
}
