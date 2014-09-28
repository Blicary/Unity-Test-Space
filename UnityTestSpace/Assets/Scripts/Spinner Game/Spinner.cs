using UnityEngine;
using System.Collections;

public class Spinner : MonoBehaviour 
{
    private bool on_track = false;
    Collider2D track = null;
    private Vector2 track_direction;

    public Material mat_on_track, mat_normal;
    private SpriteRenderer sprite_renderer;

    public LayerMask tracks_layer;
    public float radius = 1f;

    private float track_speed = 20f;



    public void Start()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();
        if (sprite_renderer == null) Debug.LogError("SpriteRenderer missing");


        sprite_renderer.material = mat_normal;
        rigidbody2D.velocity = Vector2.up * 15f;
    }

    public void Update()
    {
        if (on_track)
        {
            UpdateTrackAttatchment();
            transform.Translate(track_direction * track_speed * Time.deltaTime);
        }
    }
    public void FixedUpdate()
    {
        if (!on_track)
            rigidbody2D.velocity *= 1f - (0.7f * Time.fixedDeltaTime);
    }

    /*
    public void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "track" && collider == track) // only detatch if not on a different track now
        {
            Debug.Log("leave track");
            DetatchFromTrack();
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Perpendicular(track_direction), radius + 0.5f, tracks_layer);

        // detach
        if (hit.collider == null)
        {
            DetatchFromTrack();
            return;
        }
        
        // attach to new track (segment)
        if (hit.collider != track) // new track
            AttachToTrack(hit.collider);

        // update spinner position (keep next to track)
        transform.position = hit.point + hit.normal * radius;

        // update direction
        track_direction = -Perpendicular(hit.normal);
        //Debug.DrawLine(transform.position, hit.point, Color.blue);
        //Debug.DrawLine(transform.position, (Vector2)transform.position + track_direction * radius, Color.white);
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
        AttachToTrack(contact.collider);

        // find track_direction first time
        Vector2 normal = (contact.point - (Vector2)transform.position).normalized;
        track_direction = Perpendicular(normal);
    }
    
    private void DetatchFromTrack()
    {
        on_track = false;
        track = null;
        sprite_renderer.material = mat_normal;

        rigidbody2D.velocity = track_direction * track_speed;
    }


    private Vector2 Perpendicular(Vector2 v)
    {
        return new Vector2(v.y, -v.x);
    }

}
