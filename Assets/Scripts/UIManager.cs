using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Slider droneSlider, speedSlider;
    public Toggle showPathToggle;
    public DroneSpawner redSpawner, blueSpawner;
    public TMP_InputField spawnIntervalInputField;

    private bool currentShowPathState = false;

    private void Start()
    {
        showPathToggle.isOn = false; // выключено по умолчанию
        ApplySettings();
        ToggleShowPaths(showPathToggle.isOn);
    }

    public void ApplySettings()
    {
        int droneCount = (int)droneSlider.value;
        float droneSpeed = speedSlider.value;

        // передаём текущее состояние showPath
        redSpawner.SpawnDrones(droneCount, droneSpeed, currentShowPathState);
        blueSpawner.SpawnDrones(droneCount, droneSpeed, currentShowPathState);

        if (float.TryParse(spawnIntervalInputField.text, out float resourcesPerMinute))
        {
            resourcesPerMinute = Mathf.Clamp(resourcesPerMinute, 1f, 600f);
            float interval = 60f / resourcesPerMinute;

            ResourceManager.Instance.spawnInterval = interval;

            ResourceManager.Instance.CancelInvoke(nameof(ResourceManager.Instance.Spawn));
            ResourceManager.Instance.InvokeRepeating(nameof(ResourceManager.Instance.Spawn), 1f, interval);
        }
    }

    public void ToggleShowPaths(bool show)
    {
        currentShowPathState = show;

        foreach (var drone in redSpawner.GetSpawnedDrones())
        {
            var pathVisualizer = drone.GetComponent<DronePathVisualizer>();
            if (pathVisualizer != null)
                pathVisualizer.SetShowPath(show);
        }

        foreach (var drone in blueSpawner.GetSpawnedDrones())
        {
            var pathVisualizer = drone.GetComponent<DronePathVisualizer>();
            if (pathVisualizer != null)
                pathVisualizer.SetShowPath(show);
        }
    }
}