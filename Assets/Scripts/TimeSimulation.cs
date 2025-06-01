using UnityEngine;
using UnityEngine.UI;

public class TimeScaleController : MonoBehaviour
{
    [SerializeField] private Slider timeSlider;

    private void Start()
    {
        if (timeSlider != null)
        {
            timeSlider.minValue = 0f;
            timeSlider.maxValue = 10f;
            timeSlider.value = 1f;

            timeSlider.onValueChanged.AddListener(OnSliderValueChanged);
            Time.timeScale = timeSlider.value;
        }
    }

    private void OnSliderValueChanged(float value)
    {
        Time.timeScale = value;
    }

    private void OnDestroy()
    {
        if (timeSlider != null)
            timeSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }
}