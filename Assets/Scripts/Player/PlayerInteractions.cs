using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractions : MonoBehaviour
{
    public PlayerUI playerUi;
    private int interactableCollisionCount = 0;
    private List<InteractableObject> currentColliding = new List<InteractableObject>();

    public InteractableObject latestInteractable;

    private InputAction interactAction;

    void Awake()
    {
        interactAction = InputSystem.actions.FindAction("Interact");
    }

    void OnEnable()
    {
        interactAction.Enable();
    }

    void OnDisable()
    {
        interactAction.Disable();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InteractableCollider"))
        {
            Transform parent = other.transform.parent;
            InteractableObject io = parent.GetComponent<InteractableObject>();
            if (io.isInteractable && !currentColliding.Contains(io))
            {
                currentColliding.Add(io);
                interactableCollisionCount++;
                playerUi.showInteract = true;
                if (latestInteractable) latestInteractable.isSelected = false;
                latestInteractable = io;
                latestInteractable.isSelected = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("InteractableCollider"))
        {
            Transform parent = other.transform.parent;
            InteractableObject io = parent.GetComponent<InteractableObject>();
            if (io.isInteractable && currentColliding.Contains(io))
            {
                currentColliding.Remove(io);
                interactableCollisionCount--;
                if (interactableCollisionCount == 0)
                {
                    playerUi.showInteract = false;
                    latestInteractable.isSelected = false;
                    latestInteractable = null;
                }
                else
                {
                    if (latestInteractable == io)
                    {
                        latestInteractable.isSelected = false;
                        latestInteractable = currentColliding[currentColliding.Count - 1];
                        latestInteractable.isSelected = true;
                    }
                }
            }
        }
    }

    void Update()
    {
        if (UIManager.instance != null && UIManager.instance.isInUI) return;

        if (interactAction.WasPressedThisFrame() && interactAction.IsPressed())
        {
            if (latestInteractable != null && latestInteractable.isInteractable)
            {
                                // Show task menu first
                TaskMenu tm = FindFirstObjectByType<TaskMenu>();
                UIManager ui = FindFirstObjectByType<UIManager>();
                tm.onTaskSelected = () =>
                {
                    DialogueManager dm = FindFirstObjectByType<DialogueManager>();
                    dm.interactableObject = latestInteractable;
                    dm.dialogueTask = tm.selectedTask;
                    ui.ShowDialogue();
                };
                playerUi.showInteract = false;
                ui.ShowTaskMenu();
            }
        }
    }
}
