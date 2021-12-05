using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dialogue : MonoBehaviour
{
    public GameObject[] dialogue;
    public int curDialogue = 0;
    public string nextRoomName;

    private void Start()
    {
        for (int i = 0; i < dialogue.Length; i++)
            dialogue[i].SetActive(false);

        dialogue[0].SetActive(true);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            curDialogue++;

            if (curDialogue < dialogue.Length)
            {
                for (int i = 0; i < dialogue.Length; i++)
                    dialogue[i].SetActive(false);

                dialogue[curDialogue].SetActive(true);
            }
            else
            {
                SceneManager.LoadScene(nextRoomName);
            }
        }
    }
}
