using System;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public InteractableObject interactableObject; // The object the player is interacting with
    public string dialogueTitle; // The name of the object
    public string dialogueTask;
    
    public string textChange;
    public string textDiscard;
    public string textNothing;
    
    public GameObject buttonChange;
    public GameObject buttonDiscard;
    public GameObject buttonNothing;

    // Called when player interacts with an object
    public void ShowDialogue()
    {
        // Show ActionPanel
        buttonChange.transform.parent.gameObject.SetActive(true);
        buttonChange.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = textChange;
        buttonDiscard.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = textDiscard;
        buttonNothing.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = textNothing;
    }


    public void HideDialogue()
    {
        // Hide ActionPanel
        buttonChange.transform.parent.gameObject.SetActive(false);
    }
}
