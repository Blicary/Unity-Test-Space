using UnityEngine;
using System.Collections;

public class RollerBladeMovement : MonoBehaviour 
{
    private Spinner spinner;

    public float turn_speed = 5f;
    private float max_speed = 20.0f;
    private float drag = 0.04f;

    private Vector2 direction_point = new Vector2();
    private Vector2 target_direction = new Vector2();
    private Quaternion target_rotation;


    public void Start()
    {
        spinner = GetComponent<Spinner>();
        target_rotation = transform.rotation;
    }

    public void FixedUpdate()
    {
        if (spinner != null && spinner.OnTrack()) return;

        Debug.DrawLine(transform.position, (Vector2)transform.position + direction_point * 3f);

        // input
        Vector2 input = new Vector2();
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");


        // update direction (turn to target direction)
        if (input != Vector2.zero)
        {
            if (input.normalized != target_direction)
            {
                target_direction = input.normalized;
                SetTargetRotation();
            }
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, target_rotation, Time.fixedDeltaTime * turn_speed);
        direction_point.x = Mathf.Cos((transform.rotation.eulerAngles.z + 90) * Mathf.Deg2Rad);
        direction_point.y = Mathf.Sin((transform.rotation.eulerAngles.z + 90) * Mathf.Deg2Rad);



        if (input != Vector2.zero)
        {
            // physics
            rigidbody2D.velocity = direction_point * max_speed;
        }
        else
        {
            // drag
            rigidbody2D.velocity /= 1 + drag;
        }
    }
    public void OnSpinnerDetach()
    {
        Reset();
    }

    private void SetTargetRotation()
    {
        target_rotation = Quaternion.Euler(new Vector3(0, 0, -90 + Mathf.Atan2(target_direction.y, target_direction.x) * Mathf.Rad2Deg));
    }
    private void Reset()
    {
        direction_point = rigidbody2D.velocity.normalized;
        target_direction = rigidbody2D.velocity.normalized;
        SetTargetRotation();
    }

    public Vector2 DirectionPointing()
    {
        return direction_point;
    }
}
