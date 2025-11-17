using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class JudgementManager : MonoBehaviour
{
    public PointsSystem pointsSystem;

    public TextMeshProUGUI partnerText;

    public float judgementDuration = 5f;

    private float timer = 0f;
    private bool isJudging = false;
    private InputAction escapeAction;
    private float lastEscapeTime = -1f;

    void Awake()
    {
        if (pointsSystem == null) pointsSystem = FindFirstObjectByType<PointsSystem>();
        escapeAction = InputSystem.actions.FindAction("Escape");
    }

    void Update()
    {
        if (isJudging)
        {
            timer += Time.deltaTime;
            if (timer >= judgementDuration)
            {
                isJudging = false;
                StartNextDay();
            }

            // Check for double escape to skip
            if (escapeAction.WasPerformedThisFrame())
            {
                if (Time.time - lastEscapeTime < 0.5f)
                {
                    isJudging = false;
                    StartNextDay();
                }
                lastEscapeTime = Time.time;
            }
        }
    }

    public void StartJudgement()
    {
        UIManager.instance.ShowJudgement();
        partnerText.text = pointsSystem.GetJudgementText();
        timer = 0f;
        isJudging = true;
    }

    public void StartNextDay()
    {
        UIManager.instance.HideAll();
        pointsSystem.StartNewDay();
    }
}
