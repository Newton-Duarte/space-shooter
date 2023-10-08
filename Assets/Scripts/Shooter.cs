using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("General")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileLifetime = 5f;
    [SerializeField] float baseFiringRate = 0.2f;

    [Header("AI")]
    [SerializeField] bool useAI;
    [SerializeField] float firingRateVariance = 0f;
    [SerializeField] float minimumFiringRate = 0.1f;

    [HideInInspector] public bool isFiring;

    GameObject[] playerWeapons;
    AudioPlayer audioPlayer;
    Coroutine firingRoutine;

    void Awake()
    {
        audioPlayer = FindAnyObjectByType<AudioPlayer>();
    }

    void Start()
    {
        if (useAI)
        {
            isFiring = true;
        }
    }

    void Update()
    {
        Fire();
    }

    public void SetPlayerWeapons(GameObject[] weapons)
    {
        playerWeapons = weapons;
    }

    void Fire()
    {
        if (isFiring && firingRoutine == null)
        {
            firingRoutine = StartCoroutine(FireContinuously());
        }
        else if (!isFiring && firingRoutine != null)
        {
            StopCoroutine(firingRoutine);
            firingRoutine = null;
        }
    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            if (useAI)
            {
                GameObject projectil = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                Rigidbody2D projectilRb = projectil.GetComponent<Rigidbody2D>();

                if (projectilRb != null)
                {
                    projectilRb.velocity = transform.up * projectileSpeed;
                }

                Destroy(projectil, projectileLifetime);
            }
            else
            {
                foreach (GameObject playerWeapon in playerWeapons)
                {
                    GameObject projectil = Instantiate(projectilePrefab, playerWeapon.transform.position, Quaternion.identity);
                    Rigidbody2D projectilRb = projectil.GetComponent<Rigidbody2D>();

                    if (projectilRb != null)
                    {
                        projectilRb.velocity = transform.up * projectileSpeed;
                    }

                    Destroy(projectil, projectileLifetime);
                }
            }

            audioPlayer.PlayShootingClip();
            float timeToNextProjectile = UnityEngine.Random.Range(baseFiringRate - firingRateVariance, baseFiringRate + firingRateVariance);
            yield return new WaitForSeconds(Mathf.Clamp(timeToNextProjectile, minimumFiringRate, float.MaxValue));
        }
    }
}
