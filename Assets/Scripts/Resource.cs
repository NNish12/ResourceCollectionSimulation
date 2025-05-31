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
        // Сообщаем менеджеру перед удалением
        ResourceManager.Instance.RemoveResource(this);
        Destroy(gameObject);
    }
}