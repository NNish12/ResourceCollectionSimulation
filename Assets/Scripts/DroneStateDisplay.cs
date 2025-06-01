using TMPro;
using UnityEngine;

public class DroneStateDisplay : MonoBehaviour
{
    public TextMeshProUGUI stateText;
    private DroneController currentDrone;

    private void Update()
    {
        if (currentDrone != null)
        {
            stateText.text = $"{currentDrone.State}";
        }
        else
        {
            stateText.text = "Choose a drone";
        }
    }

    public void SetDrone(DroneController drone)
    {
        currentDrone = drone;
    }

    public void ClearDrone()
    {
        currentDrone = null;
    }
}