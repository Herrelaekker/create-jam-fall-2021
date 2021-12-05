using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorwayTransition : MonoBehaviour
{
    public GameObject roomTransitionTo;
    public GameObject doorBlock;
    private Animator anim;
    bool locked = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (roomTransitionTo != null)
            {
                if (!locked)
                    FindObjectOfType<DungeonManager>().RoomTransition(roomTransitionTo);
            }
        }
    }

    public void CloseDoor()
    {
        //this.gameObject.SetActive(false);
        doorBlock.SetActive(true);
        locked = true;
        anim.SetBool("Open", false);
        anim.SetBool("Close", true);
    }

    public void OpenDoor()
    {
        //this.gameObject.SetActive(true);
        doorBlock.SetActive(false);
        locked = false;
        anim.SetBool("Open", true);
        anim.SetBool("Close", false);
    }
}
