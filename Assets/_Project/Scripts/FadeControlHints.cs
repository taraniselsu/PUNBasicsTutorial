using UnityEngine;
using UnityEngine.InputSystem;

public class FadeControlHints : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float initialWait = 5f;
    [SerializeField] private float fadeTime = 1f;

    private float waitTimeRemaining;
    private float fadeTimeRemaining;

    private void OnValidate()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        waitTimeRemaining = initialWait;
        fadeTimeRemaining = fadeTime;
    }

    private void Update()
    {
        if (waitTimeRemaining > 0)
        {
            waitTimeRemaining -= Time.deltaTime;

            if (Keyboard.current.wKey.isPressed
                || Keyboard.current.aKey.isPressed
                || Keyboard.current.sKey.isPressed
                || Keyboard.current.dKey.isPressed
                || Mouse.current.leftButton.isPressed)
            {
                waitTimeRemaining = 0;
            }
        }
        else if (fadeTimeRemaining > 0)
        {
            fadeTimeRemaining -= Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(fadeTimeRemaining / fadeTime);

            if (fadeTimeRemaining <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
