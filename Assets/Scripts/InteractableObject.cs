using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableObject : MonoBehaviour
{
    public bool isInteractable = false;
    private PointsSystem.ObjectState currentState = PointsSystem.ObjectState.Unchanged;
    public GameObject currentVariant;
    public PointsSystem pointsSystem;
    public PointsSystem.ObjectState CurrentState
    {
        set
        {
            if (value == PointsSystem.ObjectState.Unchanged) return;
            pointsSystem.stateHistory[gameObject.name].Add(value);
            currentState = value;
        }
    }

}