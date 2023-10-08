using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] int healthPoints = 10;
    [SerializeField] AudioClip healthClip;
    [SerializeField] GameObject healthPrefab;

    [Header("Shield")]
    [SerializeField] AudioClip shieldClip;
    [SerializeField] GameObject shieldPrefab;

    [Header("Power Up")]
    [SerializeField] AudioClip powerUpClip;
    [SerializeField] GameObject powerUpPrefab;

    [Header("Bomb")]
    [SerializeField] AudioClip bombClip;
    [SerializeField] GameObject bombPrefab;

    float collectibleClipVolume = 0.8f;

    Player player;
    AudioPlayer audioPlayer;

    void Awake()
    {
        player = FindAnyObjectByType<Player>();
        audioPlayer = FindAnyObjectByType<AudioPlayer>();
    }

    public void Collect(Collectible collectible)
    {
        CollectibleType collectibleType = collectible.GetCollectibleType();

        switch (collectibleType)
        {
            case CollectibleType.Health:
                HandleHealth();
                break;
            case CollectibleType.Shield:
                HandleShield();
                break;
            case CollectibleType.PowerUp:
                HandlePowerUp();
                break;
            case CollectibleType.Bomb:
                HandleBomb();
                break;
            default:
                break;
        }

        Destroy(collectible.gameObject);
    }

    void HandleHealth()
    {
        player.GetComponent<Health>().SetHealth(healthPoints);
        audioPlayer.PlayClip(healthClip, collectibleClipVolume);
    }

    void HandleShield()
    {
        player.ActivateShield();
        audioPlayer.PlayClip(shieldClip, collectibleClipVolume);
    }
    void HandlePowerUp()
    {
        player.PowerUp();
        audioPlayer.PlayClip(powerUpClip, collectibleClipVolume);
    }

    void HandleBomb()
    {
        audioPlayer.PlayClip(bombClip, collectibleClipVolume);
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(var enemy in enemies)
        {
            Health enemyHealth = enemy.GetComponent<Health>();
            
            if (enemyHealth != null)
            {
                enemyHealth.Die();
            }
        }
    }

    public void DropCollectible(Transform transform)
    {
        int rand = UnityEngine.Random.Range(0, 100);

        if (rand < 50)
        {
            rand = UnityEngine.Random.Range(0, 100);

            if (rand > 95)
            {
                DropBomb(transform);
            }
            else if (rand > 90)
            {
                DropShield(transform);
            }
            else if (rand > 85)
            {
                DropPowerUp(transform);
            }
            else if (rand > 75)
            {
                DropHealth(transform);
            }
        }
    }

    void DropBomb(Transform transform)
    {
        Instantiate(bombPrefab, transform.position, Quaternion.identity);
    }

    void DropShield(Transform transform)
    {
        Instantiate(shieldPrefab, transform.position, Quaternion.identity);
    }

    void DropPowerUp(Transform transform)
    {
        Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
    }

    void DropHealth(Transform transform)
    {
        Instantiate(healthPrefab, transform.position, Quaternion.identity);
    }
}
