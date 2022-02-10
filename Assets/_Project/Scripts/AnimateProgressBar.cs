using UnityEngine;
using UnityEngine.UI;

public class AnimateProgressBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private float speed = 0.1f;

    private bool increasing = true;

    private void OnValidate()
    {
        if (!slider)
        {
            slider = GetComponent<Slider>();
        }
    }

    private void Start()
    {
        slider.value = 0;
    }

    private void Update()
    {
        if (increasing)
        {
            float newValue = slider.value + speed * Time.deltaTime;
            if (newValue > 1f)
            {
                newValue = 2f - newValue;
                increasing = false;
            }

            slider.value = newValue;
        }
        else
        {
            float newValue = slider.value - speed * Time.deltaTime;
            if (newValue < 0)
            {
                newValue = -newValue;
                increasing = true;
            }

            slider.value = newValue;
        }
    }
}
