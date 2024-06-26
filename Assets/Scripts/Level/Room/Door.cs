using UnityEngine;

public class Door : MonoBehaviour
{
    public enum DoorType{
        top, right, bottom, left
    }

    public DoorType type;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player")
        {
            RoomController.instance.OnPlayerTouchDoor(transform.parent.parent.gameObject.GetComponent<Room>(), type);
        }
    }

    public void CloseDoors(){
        GetComponentInChildren<DoorCollider>().OnClose();
    }

    public void OpenDoors(){
        GetComponentInChildren<DoorCollider>().OnOpen();
    }
}
