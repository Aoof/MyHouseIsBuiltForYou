using System;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [NonSerialized]
    public InteractableObject interactableObject; // The object the player is interacting with

    [NonSerialized]
    public string dialogueTitle; // The name of the object
    [NonSerialized]
    public string dialogueTask;
    
    [NonSerialized]
    public string textNothing = "Back";
    
    [Header("UI Elements")]
    [SerializeField] private GameObject buttonChange;
    [SerializeField] private GameObject buttonDestroy;
    [SerializeField] private GameObject buttonNothing;

    public GameObject dialoguePanel => buttonChange?.transform.parent.gameObject;
    void Start()
    {
        buttonChange.GetComponent<Button>().onClick.AddListener(OnChangeClicked);
        buttonDestroy.GetComponent<Button>().onClick.AddListener(OnDestroyClicked);
        buttonNothing.GetComponent<Button>().onClick.AddListener(OnNothingClicked);
    }

    // Called when player interacts with an object
    public void ShowDialogue()
    {
        UIManager ui = FindFirstObjectByType<UIManager>();
        ui.ShowDialogue();
        DialogueOptions options = interactableObject.GetCurrentOptions();
        if (options != null)
        {
            buttonChange.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = options.changeString;
            buttonDestroy.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = options.destroyString;
        }
        buttonNothing.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = textNothing;
    }


    public void HideDialogue()
    {
        UIManager ui = FindFirstObjectByType<UIManager>();
        ui.HideAll();
    }

    public void OnChangeClicked()
    {
        interactableObject.ChangeVariant();
        interactableObject.CurrentState = PointsSystem.ObjectState.Changed;
        HideDialogue();
    }

    public void OnDestroyClicked()
    {
        interactableObject.DestroyVariant();
        interactableObject.CurrentState = PointsSystem.ObjectState.Destroyed;
        HideDialogue();
    }

    public void OnNothingClicked()
    {
        HideDialogue();
    }
}
