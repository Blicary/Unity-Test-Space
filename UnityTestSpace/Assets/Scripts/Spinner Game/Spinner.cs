﻿using UnityEngine;
using System.Collections;

public class Spinner : MonoBehaviour 
{
    private bool on_track = false;
    private Vector2 track_direction;

    public Material mat_on_track, mat_normal;
    private SpriteRenderer sprite_renderer;

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
            transform.Translate(track_direction * track_speed * Time.deltaTime);
        else
        {
            rigidbody2D.velocity *= 0.99f;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "track")
        {
            DetachFromTrack();
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.collider.tag == "track")
            {
                AttachToTrack(contact);
            }
        }
    }
    

    private void AttachToTrack(ContactPoint2D contact)
    {
        on_track = true;
        sprite_renderer.material = mat_on_track;

        rigidbody2D.velocity = Vector2.zero;

        Vector2 normal = (contact.point - (Vector2)transform.position).normalized;
        track_direction = new Vector2(normal.y, -normal.x);
    }
    private void DetachFromTrack()
    {
        on_track = false;
        sprite_renderer.material = mat_normal;

        rigidbody2D.velocity = track_direction * track_speed;
    }

}