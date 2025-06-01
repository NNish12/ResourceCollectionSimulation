using System.Collections.Generic;
using UnityEngine;

public class DroneSpawner : MonoBehaviour
{
    public GameObject dronePrefab;
    public Transform[] slots;
    public DroneController.Faction faction;

    private List<GameObject> spawnedDrones = new List<GameObject>();

    public void SpawnDrones(int desiredCount, float speed, bool showPath)
    {
        int currentCount = spawnedDrones.Count;

        if (desiredCount < currentCount)
        {
            for (int i = currentCount - 1; i >= desiredCount; i--)
            {
                Destroy(spawnedDrones[i]);
                spawnedDrones.RemoveAt(i);
            }
        }

        for (int i = currentCount; i < desiredCount && i < slots.Length; i++)
        {
            var slot = slots[i];
            var drone = Instantiate(dronePrefab, slot.position, Quaternion.identity);
            var droneController = drone.GetComponent<DroneController>();
            droneController.faction = faction;
            droneController.speed = speed;
            droneController.homeSlot = slot;

            var pathVisualizer = drone.GetComponent<DronePathVisualizer>();
            if (pathVisualizer != null)
                pathVisualizer.SetShowPath(showPath);

            spawnedDrones.Add(drone);
        }
    }

    public List<GameObject> GetSpawnedDrones()
    {
        return spawnedDrones;
    }
}