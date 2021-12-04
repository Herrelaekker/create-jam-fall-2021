using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorwayTransition : MonoBehaviour
{
    public GameObject roomTransitionTo;
    public GameObject doorBlock;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (roomTransitionTo != null)
            {
                FindObjectOfType<DungeonManager>().RoomTransition(roomTransitionTo);
            }
        }
    }

    public void CloseDoor()
    {
        this.gameObject.SetActive(false);
        doorBlock.SetActive(true);
    }

    public void OpenDoor()
    {
        this.gameObject.SetActive(true);
        doorBlock.SetActive(false);
    }
}
