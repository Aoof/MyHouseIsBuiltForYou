using Unity.VisualScripting;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public bool showInteract = false;
    public GameObject interactIndicator;

    void Update()
    {
        if (showInteract != interactIndicator.activeSelf)
            interactIndicator.SetActive(showInteract);
    }
}
