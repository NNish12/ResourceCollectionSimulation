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

            var pathRenderer = drone.GetComponent<PathRenderer>();
            if (pathRenderer != null)
                pathRenderer.ShowPath = showPath;

            spawnedDrones.Add(drone);
        }

        for (int i = 0; i < Mathf.Min(desiredCount, spawnedDrones.Count); i++)
        {
            var pr = spawnedDrones[i].GetComponent<PathRenderer>();
            if (pr != null)
                pr.ShowPath = showPath;
        }
    }
}