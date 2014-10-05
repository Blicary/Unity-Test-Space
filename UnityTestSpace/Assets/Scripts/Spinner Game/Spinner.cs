using UnityEngine;
using System.Collections;

public class Spinner : MonoBehaviour 
{
    // references
    private SpriteRenderer sprite_renderer;
    public LayerMask tracks_layer;
    public CircleCollider2D tracks_collider;

    // appearance
    public Material mat_on_track, mat_normal;

    // movement / physics
    public float radius = 1f;
    public float track_speed = 35f;
    public float drag = 0.4f;
    private Vector2 last_velocity;
    
    // track
    private bool on_track = false;
    private Collider2D track = null;
    private Vector2 attached_direction;

    private int track_direction = 1;


    // PUBLIC MODIFIERS

    public void Start()
    {
        sprite_renderer = GetComponent<SpriteRenderer>();
        if (sprite_renderer == null) Debug.LogError("SpriteRenderer missing");

        sprite_renderer.material = mat_normal;
        last_velocity = rigidbody2D.velocity;

        rigidbody2D.velocity = new Vector2(0, 8);
    }

    public void Update()
    {
        if (on_track)
        {
            UpdateTrackAttatchment();
            transform.Translate(attached_direction * track_speed * Time.deltaTime);
        }

        // TESTING
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DetachFromTrack(true);
        }

    }
    
    public void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "track")
        {
            if (!on_track) tracks_collider.enabled = true; 
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.collider.tag == "track")
            {
                // attach to a track if not on a track already
                if (!on_track) AttachToTrack(contact);
            }
            else if (contact.collider.tag == "wall")
            {
                if (on_track)
                {
                    attached_direction *= -1;
                    track_direction *= -1;
                }
                //ForceDetachFromTrack();
            }
        }
    }


    // PRIVATE MODIFIERS
    
    private void UpdateTrackAttatchment()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Perpendicular(attached_direction, track_direction), radius + 0.5f, tracks_layer);

        // detach
        if (hit.collider == null)
        {
            DetachFromTrack(false);
            return;
        }
        
        // attach to new track (segment)
        if (hit.collider != track) // new track
            AttachToTrack(hit.collider);

        // update spinner position (keep next to track)
        UpdateAttachedPosition(hit.point, hit.normal);

        // update direction
        attached_direction = -Perpendicular(hit.normal, track_direction);
    }
    private void AttachToTrack(Collider2D collider)
    {
        on_track = true;
        track = collider;
        sprite_renderer.material = mat_on_track;

        rigidbody2D.velocity = Vector2.zero;
        transform.rotation = Quaternion.identity;

        SendMessage("OnSpinnerAttach");
    }
    private void AttachToTrack(ContactPoint2D contact)
    {
        // find direction first time
        Vector2 normal = (contact.point - (Vector2)transform.position).normalized;
        Vector2 perp = Perpendicular(normal, 1);

        float dot = Vector2.Dot(last_velocity, perp);
        track_direction = dot == 0 ? track_direction : dot > 0 ? 1 : -1;

        attached_direction = Perpendicular(normal, track_direction);


        // disable track collider
        tracks_collider.enabled = false;

        // update spinner position (first time) 
        UpdateAttachedPosition(contact.point, normal);

        // attach
        AttachToTrack(contact.collider);
    }
    
    private void DetachFromTrack(bool forceful)
    {
        if (!on_track) return;

        on_track = false;
        track = null;
        sprite_renderer.material = mat_normal;

        if (forceful)
        {
            // push the spinner away from the track
            Vector2 normal = Perpendicular(attached_direction, track_direction);
            Vector2 v = (attached_direction + normal).normalized;
            rigidbody2D.velocity = v * track_speed;

            transform.Translate(normal * tracks_collider.radius);
        }
        else
        {
            // spinner continues to travel at same velocity
            rigidbody2D.velocity = attached_direction * track_speed;
        }

        SendMessage("OnSpinnerDetach");
    }


    private void UpdateAttachedPosition(Vector2 point_on_track, Vector2 track_normal)
    {
        transform.position = point_on_track + track_normal * radius;
    }


    // PRIVATE HELPERS

    private Vector2 Perpendicular(Vector2 v, int track_direction)
    {
        return new Vector2(v.y, -v.x) * track_direction;
    }


    // PUBLIC ACCESSORS

    public bool OnTrack()
    {
        return on_track;
    }

}
