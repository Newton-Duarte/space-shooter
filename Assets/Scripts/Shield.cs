using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] AudioClip deactivateShieldClip;

    AudioPlayer audioPlayer;

    void Awake()
    {
        audioPlayer = FindAnyObjectByType<AudioPlayer>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.GetComponent<DamageDealer>();

        if (damageDealer != null)
        {
            audioPlayer.PlayClip(deactivateShieldClip, 0.8f);
            gameObject.SetActive(false);
            Destroy(collision.gameObject);
        }
    }
}
