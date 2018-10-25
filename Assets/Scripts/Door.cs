using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    private bool inDoor = false;
    [SerializeField] private string loadRoom;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hello");
        if (other.CompareTag("Player"))
        {
            inDoor = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Goodbye");
        if (other.CompareTag("Player"))
        {
            inDoor = false;
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.LeftControl))
        {
            Debug.Log(inDoor);
            if (inDoor)
            {
                Debug.Log("Hey");
                SceneManager.LoadScene(loadRoom);
            }
        }
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  