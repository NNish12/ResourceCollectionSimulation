using UnityEngine;

public class Resource : MonoBehaviour
{
    public bool IsClaimed { get; private set; }

    public void Claim()
    {
        IsClaimed = true;
    }

    public void Release()
    {
        IsClaimed = false;
    }

    public void Collect()
    {
        ResourceManager.Instance.RemoveResource(this);
        Destroy(gameObject);
    }
}