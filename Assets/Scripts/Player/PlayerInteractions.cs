using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractions : MonoBehaviour
{
    public PlayerUI playerUi;
    private int interactableCollisionCount = 0;

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
            if (parent.GetComponent<InteractableObject>().isInteractable)
            {
                interactableCollisionCount++;
                playerUi.showInteract = true;
                if (latestInteractable) latestInteractable.isSelected = false;
                latestInteractable = parent.GetComponent<InteractableObject>();
                latestInteractable.isSelected = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("InteractableCollider"))
        {
            Transform parent = other.transform.parent;
            if (parent.GetComponent<InteractableObject>().isInteractable)
            {
                interactableCollisionCount--;
                if (interactableCollisionCount == 0)
                {
                    playerUi.showInteract = false;
                    latestInteractable.isSelected = false;
                    latestInteractable = null;
                }
            }
        }
    }

    void Update()
    {
        if (UIManager.instance != null && UIManager.instance.isInUI) return;

        if (interactAction.WasPressedThisFrame() && interactAction.IsPressed())
        {
            if (latestInteractable != null && latestInteractable.GetComponent<InteractableObject>().isInteractable)
            {
                // Show task menu first
                TaskMenu tm = FindFirstObjectByType<TaskMenu>();
                UIManager ui = FindFirstObjectByType<UIManager>();
                tm.onTaskSelected = () =>
                {
                    DialogueManager dm = FindFirstObjectByType<DialogueManager>();
                    dm.interactableObject = latestInteractable;
                    dm.dialogueTask = tm.selectedTask;
                    dm.ShowDialogue();
                };
                ui.ShowTaskMenu();
            }
        }
    }
}
