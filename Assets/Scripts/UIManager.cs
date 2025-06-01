using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UIManager : MonoBehaviour
{
    public Slider droneSlider, speedSlider;
    public Toggle showPathToggle;
    public DroneSpawner redSpawner, blueSpawner;
    public TMP_InputField spawnIntervalInputField;

    private bool currentShowPathState = false;

    private void Start()
    {
        currentShowPathState = showPathToggle.isOn;
        ApplySettings();
    }

    public void ApplySettings()
    {
        int droneCount = (int)droneSlider.value;
        float droneSpeed = speedSlider.value;


        redSpawner.SpawnDrones(droneCount, droneSpeed, currentShowPathState);
        blueSpawner.SpawnDrones(droneCount, droneSpeed, currentShowPathState);

        UpdateDroneSpeeds(droneSpeed);

        if (float.TryParse(spawnIntervalInputField.text, out float resourcesPerMinute))
        {
            resourcesPerMinute = Mathf.Clamp(resourcesPerMinute, 1f, 600f);
            float interval = 60f / resourcesPerMinute;

            ResourceManager.Instance.spawnInterval = interval;

            ResourceManager.Instance.CancelInvoke(nameof(ResourceManager.Instance.Spawn));
            ResourceManager.Instance.InvokeRepeating(nameof(ResourceManager.Instance.Spawn), 1f, interval);
        }
    }
    private void UpdateDroneSpeeds(float newSpeed)
    {
        foreach (var drone in redSpawner.GetSpawnedDrones())
        {
            var controller = drone.GetComponent<DroneController>();
            if (controller != null)
            {
                controller.SetSpeed(newSpeed);
            }
        }

        foreach (var drone in blueSpawner.GetSpawnedDrones())
        {
            var controller = drone.GetComponent<DroneController>();
            if (controller != null)
            {
                controller.SetSpeed(newSpeed);
            }
        }
    }

    public void ToggleShowPaths(bool show)
    {
        var allDrones = redSpawner.GetSpawnedDrones().Concat(blueSpawner.GetSpawnedDrones());

        foreach (var drone in allDrones)
        {
            var pathVisualizer = drone.GetComponent<DronePathVisualizer>();
            if (pathVisualizer != null)
                pathVisualizer.SetShowPath(show);
        }
    }
}