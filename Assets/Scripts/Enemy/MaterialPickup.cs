using UnityEngine;

public class MaterialPickup : MonoBehaviour
{
    [SerializeField] int value = 1;
    [SerializeField] int xp = 1;

    bool collected;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (collected || !other.TryGetComponent(out PlayerMaterials player)) return;

        collected = true;
        player.Add(value, xp);
        Destroy(gameObject);
    }
}