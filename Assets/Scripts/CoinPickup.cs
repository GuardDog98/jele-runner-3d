using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [Header("Coin settings")]
    public int coinValue = 1;

    private void Update()
    {
        transform.Rotate(0f, 200f * Time.deltaTime, 0f, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (GameManager.Instance != null)
            GameManager.Instance.AddCoins(coinValue);

        Destroy(gameObject);
    }
}
