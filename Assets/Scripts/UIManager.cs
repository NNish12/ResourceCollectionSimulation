using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Slider droneSlider, speedSlider;
    public Toggle showPathToggle;
    public DroneSpawner redSpawner, blueSpawner;
    public TMP_InputField spawnIntervalInputField;

    void Start()
    {
        ApplySettings();
    }

    public void ApplySettings()
    {
        int droneCount = (int)droneSlider.value;
        float droneSpeed = speedSlider.value;
        bool showPath = showPathToggle.isOn;

        redSpawner.SpawnDrones(droneCount, droneSpeed, showPath);
        blueSpawner.SpawnDrones(droneCount, droneSpeed, showPath);

        if (float.TryParse(spawnIntervalInputField.text, out float resourcesPerMinute))
        {
            resourcesPerMinute = Mathf.Clamp(resourcesPerMinute, 1f, 600f);
            float interval = 60f / resourcesPerMinute;

            ResourceManager.Instance.spawnInterval = interval;

            ResourceManager.Instance.CancelInvoke(nameof(ResourceManager.Instance.Spawn));
            ResourceManager.Instance.InvokeRepeating(nameof(ResourceManager.Instance.Spawn), 1f, interval);
        }
    }
}