using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorwayTransition : MonoBehaviour
{
    public GameObject roomTransitionTo;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            FindObjectOfType<DungeonManager>().RoomTransition(roomTransitionTo);
        }
    }

        // Update is called once per frame
        void Update()
    {
        
    }
}
