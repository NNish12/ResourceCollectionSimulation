using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public bool IsClaimed { get; private set; } = false;

    public void Claim()
    {
        IsClaimed = true;
    }
}