using UnityEngine;

public class Attackable : MonoBehaviour
{
    [SerializeField] private float damage = 1f;

    public float Damage { get => damage; set => damage = value; }
}