using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 8f;
    [SerializeField] float paddingLeft = 0.5f;
    [SerializeField] float paddingRight = 0.5f;
    [SerializeField] float paddingTop = 0.5f;
    [SerializeField] float paddingBottom = 0.5f;
    [SerializeField] GameObject playerShield;
    [SerializeField] GameObject[] playerWeapons;
    [SerializeField] Sprite[] playerSprites;

    int currentPowerUp = 0;

    Vector2 rawInput;
    Vector2 minBounds;
    Vector2 maxBounds;

    SpriteRenderer sr;
    Shooter shooter;
    CollectibleManager collectibleManager;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        shooter = GetComponent<Shooter>();
        collectibleManager = FindAnyObjectByType<CollectibleManager>();
    }

    void Start()
    {
        InitBounds();
        playerShield.SetActive(false);
        HandlePlayerWeapons();
        HandlePlayerSprites();
    }

    void HandlePlayerSprites()
    {
        sr.sprite = playerSprites[currentPowerUp];
    }

    void HandlePlayerWeapons()
    {
        List<GameObject> activeWeapons = new();

        for (int i = 0; i < playerWeapons.Length; i++)
        {
            if (playerWeapons[i].activeSelf)
            {
                activeWeapons.Add(playerWeapons[i]);
                shooter.SetPlayerWeapons(activeWeapons.ToArray());
            }
        }
    }

    void InitBounds()
    {
        Camera mainCamera = Camera.main;
        minBounds = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        maxBounds = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        Vector2 delta = rawInput * moveSpeed * Time.deltaTime;
        Vector2 newPosition = new Vector2();
        newPosition.x = Mathf.Clamp(transform.position.x + delta.x, minBounds.x + paddingLeft, maxBounds.x - paddingRight);
        newPosition.y = Mathf.Clamp(transform.position.y + delta.y, minBounds.y + paddingBottom, maxBounds.y - paddingTop);
        transform.position = newPosition;
    }

    void OnMove(InputValue value)
    {
        rawInput = value.Get<Vector2>();
    }

    void OnFire(InputValue value)
    {
        if (shooter != null)
        {
            shooter.isFiring = value.isPressed;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Collectible collectible = collision.GetComponent<Collectible>();

        if (collectible != null)
        {
            collectibleManager.Collect(collectible);
        }

    }

    public void ActivateShield()
    {
        playerShield.SetActive(true);
    }

    public void PowerUp()
    {
        if (currentPowerUp == 2) return;

        currentPowerUp++;

        if (currentPowerUp == 0)
        {
            playerWeapons[0].SetActive(true);
        }
        else if (currentPowerUp == 1)
        {
            playerWeapons[0].SetActive(false);
            playerWeapons[1].SetActive(true);
            playerWeapons[2].SetActive(true);
        }
        else if (currentPowerUp == 2)
        {
            foreach(var weapon in playerWeapons)
            {
                weapon.SetActive(true);
            }
        }

        HandlePlayerWeapons();
        HandlePlayerSprites();
    }
}
