using UnityEngine;
using System.Collections;

public class EnergyBullet : MonoBehaviour 
{
    private float speed = 80;
    public LineRenderer line;

    public void Initialize(Transform shooter, Vector2 direction)
    {
        rigidbody2D.velocity = direction * speed;
        Physics2D.IgnoreCollision(rigidbody2D.collider2D, shooter.rigidbody2D.collider2D);

        UpdateLine();
    }

    public void Update()
    {
        UpdateLine();
    }

    public void UpdateLine()
    {
        Vector2 direction = rigidbody2D.velocity.normalized;

        line.SetPosition(0, transform.position);
        line.SetPosition(1, (Vector2)transform.position + direction * 5);
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag == "wall")
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y * -1);
        }
    }
	
}
