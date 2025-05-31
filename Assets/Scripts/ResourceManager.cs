using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro; 

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;
    public GameObject resourcePrefab;
    public float spawnInterval = 3f;
    private List<Resource> resources = new();
    private int redCount = 0, blueCount = 0;

    public TextMeshProUGUI redText, blueText;

    void Awake() => Instance = this;

    void Start() => InvokeRepeating(nameof(Spawn), 1f, spawnInterval);

    public void Spawn()
    {
        Vector3 pos = new Vector3(Random.Range(-4f, 4f), Random.Range(-4f, 4f), 0);
        var res = Instantiate(resourcePrefab, pos, Quaternion.identity).GetComponent<Resource>();
        resources.Add(res);
    }

    public Resource GetNearestAvailable(Vector3 pos)
    {
        return resources
            .Where(r => r != null && !r.IsClaimed)
            .OrderBy(r => Vector3.Distance(pos, r.transform.position))
            .FirstOrDefault();
    }

    public void AddResource(DroneController.Faction f)
    {
        if (f == DroneController.Faction.Red)
            redText.text = (++redCount).ToString();
        else
            blueText.text = (++blueCount).ToString();
    }
}