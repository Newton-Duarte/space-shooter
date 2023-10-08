using UnityEngine;

public enum CollectibleType
{
    Health,
    Shield,
    PowerUp,
    Bomb
}

public class Collectible : MonoBehaviour
{
    [SerializeField] CollectibleType type;

    public CollectibleType GetCollectibleType()
    {
        return type;
    }
}
