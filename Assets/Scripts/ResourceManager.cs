using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class ResourceManager : MonoBehaviour
{
    private int redCount = 0;
    private int blueCount = 0;
    public static ResourceManager Instance;
    public GameObject resourcePrefab;
    public float spawnInterval = 3f;
    private List<Resource> resources = new();

    public TextMeshProUGUI redText, blueText;


    public float minX = -3f;
    public float maxX = 3f;
    public float minY = -4f;
    public float maxY = 4f;

    void Awake() => Instance = this;

    void Start() => InvokeRepeating(nameof(Spawn), 1f, spawnInterval);

    public void Spawn()
    {
        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);
        Vector3 pos = new Vector3(x, y, 0);
        Quaternion rot = Quaternion.Euler(0, 0, Random.Range(0, 360f));
        var resource = Instantiate(resourcePrefab, pos, rot).GetComponent<Resource>();
        resources.Add(resource);
    }

    public Resource GetNearestAvailable(Vector3 pos)
    {
        return resources
            .Where(resourse => resourse != null && !resourse.IsClaimed)
            .OrderBy(r => Vector3.Distance(pos, r.transform.position))
            .FirstOrDefault();
    }

    public void RemoveResource(Resource resource)
    {
        resources.Remove(resource);
    }

    public void AddResource(DroneController.Faction currentFaction)
    {
        if (currentFaction == DroneController.Faction.Red)
            redText.text = (++redCount).ToString();
        else
            blueText.text = (++blueCount).ToString();
    }
}