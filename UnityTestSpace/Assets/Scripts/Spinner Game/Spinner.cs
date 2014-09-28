using UnityEngine;
using System.Collections;

public class Spinner : MonoBehaviour 
{
    // references
    private SpriteRenderer sprite_renderer;
    public LayerMask tracks_layer;

    // appearance
    public Material mat_on_track, mat_normal;

    // movement / physics
    public float radius = 1f;
    public float force_detach_knockback = 10f;
    public float track_speed = 20f;
    public float drag = 0.4f;
    private Vector2 last_velocity;
    
    // track
    private bool on_track = false;
    private Collider2D track = null;
    private Vector2 track_direction;

    private int direction = 1;



    public void Start()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();
        if (sprite_renderer == null) Debug.LogError("SpriteRenderer missing");


        sprite_renderer.material = mat_normal;
        rigidbody2D.velocity = new Vector2(0, 8);
        last_velocity = rigidbody2D.velocity;
    }

    public void Update()
    {
        if (on_track)
        {
            UpdateTrackAttatchment();
            transform.Translate(track_direction * track_speed * Time.deltaTime);
        }

        // TESTING
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ForcedetachFromTrack();
        }

        if (!on_track)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                direction = -1;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                direction = 1;
            }
        }

    }
    public void FixedUpdate()
    {
        if (!on_track)
        {
            last_velocity = rigidbody2D.velocity;
            rigidbody2D.velocity *= 1f - (drag * Time.fixedDeltaTime);
        }
    }

    /*
    public void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "track" && collider == track) // only detach if not on a different track now
        {
            Debug.Log("leave track");
            detachFromTrack();
        }

        Debug.Log("leave old track (do nothing)");
    }
     * */
    
    public void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            // attach to a track if not on a track already
            if (!on_track && contact.collider.tag == "track")
            {
                AttachToTrack(contact);
            }
        }
    }
    
    
    private void UpdateTrackAttatchment()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Perpendicular(track_direction, direction), radius + 0.5f, tracks_layer);

        // detach
        if (hit.collider == null)
        {
            DetachFromTrack();
            return;
        }
        
        // attach to new track (segment)
        if (hit.collider != track) // new track
            AttachToTrack(hit.collider);

        // update spinner position (keep next to track)
        transform.position = hit.point + hit.normal * radius;

        // update direction
        track_direction = -Perpendicular(hit.normal, direction);
    }
    private void AttachToTrack(Collider2D collider)
    {
        on_track = true;
        track = collider;
        sprite_renderer.material = mat_on_track;

        rigidbody2D.velocity = Vector2.zero;
    }
    private void AttachToTrack(ContactPoint2D contact)
    {
        // find track_direction first time
        Vector2 normal = (contact.point - (Vector2)transform.position).normalized;
        Vector2 perp = Perpendicular(normal, 1);
        float dot = Vector2.Dot(last_velocity, perp);
        direction = dot == 0 ? direction : dot > 0 ? 1 : -1;

        track_direction = Perpendicular(normal, direction);


        // attach
        AttachToTrack(contact.collider);
    }
    
    private void ForcedetachFromTrack()
    {
        if (!on_track) return;
        DetachFromTrack();
        rigidbody2D.AddForce(Perpendicular(track_direction, direction) * force_detach_knockback, ForceMode2D.Impulse);
    }
    private void DetachFromTrack()
    {
        if (!on_track) return;

        on_track = false;
        track = null;
        sprite_renderer.material = mat_normal;

        rigidbody2D.velocity = track_direction * track_speed;
    }


    private Vector2 Perpendicular(Vector2 v, int direction)
    {
        return new Vector2(v.y, -v.x) * direction;
    }

}
