using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableObject : MonoBehaviour
{
    public bool isInteractable = false;
    private PointsSystem.ObjectState currentState = PointsSystem.ObjectState.Unchanged;
    public GameObject currentVariant;
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
            newAction.TaskName = "";
            pointsSystem.stateHistory[pointsSystem.stateHistory.Count].Day.Add(newAction);
            currentState = value;
        }
    }


    void Awake()
    {
        dialogueManager = FindFirstObjectByType<DialogueManager>();
    }
}