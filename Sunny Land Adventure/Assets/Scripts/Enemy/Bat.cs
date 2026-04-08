using UnityEngine;

public class Bat : MonoBehaviour
{
    [Header("Enemy Movement")]
    [SerializeField] private float speed = 1f;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb.linearVelocity = new Vector2(speed, 0f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        speed = -speed;
        FlipFacingEnemy();
    }

    void FlipFacingEnemy()
    {
        transform.localScale = new Vector2(-Mathf.Sign(rb.linearVelocity.x), 1f);
    }
}