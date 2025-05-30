using UnityEngine;

public class TimeController : MonoBehaviour
{
    public void Pause()
    {
        Time.timeScale = 0f;
    }

    public void NormalSpeed()
    {
        Time.timeScale = 1f;
    }

    public void DoubleSpeed()
    {
        Time.timeScale = 4f;
    }
}