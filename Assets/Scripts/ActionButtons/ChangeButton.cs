using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

public class ChangeButton : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public void ChangeObject()
    {
        dialogueManager.interactableObject.CurrentState = PointsSystem.ObjectState.Changed;
        dialogueManager.interactableObject.currentVariant = FindVariant();
    }

    public GameObject FindVariant()
    {
        GameObject variant = dialogueManager.interactableObject.currentVariant;
        
        return variant;
    }
}
