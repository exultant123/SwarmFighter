using System;
using UnityEngine;

public class PlayerMaterials : MonoBehaviour
{
    public int Materials { get; private set; }
    public int Xp { get; private set; }

    public event Action Changed;

    public void Add(int materials, int xp)
    {
        Materials += Mathf.Max(materials, 0);
        Xp += Mathf.Max(xp, 0);
        Changed?.Invoke();
    }

    public bool Spend(int amount)
    {
        if (amount < 0 || amount > Materials) return false;

        Materials -= amount;
        Changed?.Invoke();
        return true;
    }
}