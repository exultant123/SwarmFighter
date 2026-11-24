using UnityEngine;

[RequireComponent(typeof(Health))]
public class DropOnDeath : MonoBehaviour
{
    [SerializeField] MaterialPickup pickup;
    [SerializeField, Range(0f, 1f)] float chance = 1f;

    void Awake() => GetComponent<Health>().Died += Drop;

    void Drop(Health _)
    {
        if (pickup != null && Random.value <= chance)
            Instantiate(pickup, transform.position, Quaternion.identity);
    }
}