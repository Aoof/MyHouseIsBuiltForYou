using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class InteractableObject : MonoBehaviour
{
    public bool isInteractable = false;
    public bool isSelected = false;
    [SerializeField] private GameObject highlightSphere;
    private PointsSystem.ObjectState currentState = PointsSystem.ObjectState.Unchanged;
    public GameObject currentVariant;
    [Header("Variants")]
    [SerializeField] private GameObject baseVariant;
    [SerializeField] private GameObject destroyedVariant;
    [SerializeField] private GameObject[] changedVariants;
    [Header("Dialogue Options")]
    [SerializeField] private DialogueOptions baseOptions;
    [SerializeField] private DialogueOptions[] changedOptions;
    [Header("References")]
    public PointsSystem pointsSystem;
    public DialogueManager dialogueManager;
    public PointsSystem.ObjectState CurrentState
    {
        set
        {
            if (value == PointsSystem.ObjectState.Unchanged) return;
            PointsSystem.ActionStruct newAction = new PointsSystem.ActionStruct();
            newAction.ObjectName = gameObject.name;
            newAction.ObjectState = value;
            newAction.TaskName = dialogueManager.dialogueTask;
            pointsSystem.stateHistory[pointsSystem.currentDay].Day.Add(newAction);
            currentState = value;
            
            if (value == PointsSystem.ObjectState.Changed)
            {

            }
            pointsSystem.ActionPerformed();
        }
        get { return currentState; }
    }

    public void ResetState()
    {
        currentState = PointsSystem.ObjectState.Unchanged;
    }

    void Awake()
    {
        dialogueManager = FindFirstObjectByType<DialogueManager>();
        pointsSystem = FindFirstObjectByType<PointsSystem>();
        currentVariant = baseVariant;
        isSelected = false;
        UpdateVariants();
    }

    void Update()
    {
        if (highlightSphere != null)
        {
            highlightSphere.SetActive(isSelected && isInteractable);
        }
    }

    void UpdateVariants()
    {
        // Deactivate all variants
        if (baseVariant != null) baseVariant.SetActive(false);
        if (destroyedVariant != null) destroyedVariant.SetActive(false);
        foreach (var v in changedVariants)
        {
            if (v != null) v.SetActive(false);
        }
        // Activate current
        if (currentVariant != null) currentVariant.SetActive(true);
        // Update interactable
        isInteractable = currentVariant != destroyedVariant;
    }

    public DialogueOptions GetCurrentOptions()
    {
        if (currentVariant == baseVariant) return baseOptions;
        int index = System.Array.IndexOf(changedVariants, currentVariant);
        if (index >= 0 && index < changedOptions.Length) return changedOptions[index];
        return null;
    }

    public void ChangeVariant()
    {
        int index = System.Array.IndexOf(changedVariants, currentVariant);
        if (index >= 0 && index < changedVariants.Length - 1)
        {
            currentVariant = changedVariants[index + 1];
            UpdateVariants();
        } else if (currentState == PointsSystem.ObjectState.Unchanged && currentVariant != destroyedVariant)
        {
            currentVariant = changedVariants[0];
            UpdateVariants();
        }
        // Else, cannot change further
    }

    public void DestroyVariant()
    {
        currentVariant = destroyedVariant;
        UpdateVariants();
    }
}