using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour 
{
    public ParticleSystem particles;

    private float max_speed = 30.0f;
    private float drag = 0.01f;

    private bool thrusting = false;
    private int jumps_max = 3;
    private float jumps_recover_rate = 0.6f;
    private float jumps;
    private float jump_dist = 8;


    private Vector2 direction_point = new Vector2();
    private Vector2 target_direction = new Vector2();
    private Quaternion target_rotation;


    public void Start()
    {
        target_rotation = transform.rotation;
        jumps = jumps_max;
    }

    public void Update()
    {
        // input
        Vector2 input = new Vector2();
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
        bool input_thrust = Input.GetButton("Thrust");
        bool input_thrust_end = Input.GetButtonUp("Thrust");


        // update direction (turn to target direction)
        if (input != Vector2.zero)
        {
            if (input.normalized != target_direction)
            {
                target_direction = input.normalized;
                target_rotation = Quaternion.Euler(new Vector3(0, 0, -90 + Mathf.Atan2(target_direction.y, target_direction.x) * Mathf.Rad2Deg));
            }
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, target_rotation, Time.deltaTime * 10.0f);
        direction_point.x = Mathf.Cos((transform.rotation.eulerAngles.z + 90) * Mathf.Deg2Rad);
        direction_point.y = Mathf.Sin((transform.rotation.eulerAngles.z + 90) * Mathf.Deg2Rad);


        // thrust and drag
        if (!thrusting && jumps >= 1 && input_thrust) Jump();
        else if (input_thrust_end) EndThrust();

        if (thrusting)
        {
            // physics
            rigidbody2D.velocity = direction_point * max_speed;
        }
        else
        {
            // drag
            rigidbody2D.velocity /= 1 + drag;
        }

        // recover thrust charges
        jumps += jumps_recover_rate * Time.deltaTime;
        jumps = Mathf.Min(jumps, jumps_max);
        //Debug.Log(charges);
        

        
    }

    private void Jump()
    {
        // visual
        particles.Clear();
        particles.Play();

        // jump movement
        transform.Translate(Vector2.up * jump_dist);

        jumps -= 1;
        thrusting = true;
    }
    private void EndThrust()
    {
        thrusting = false;
        particles.Stop();
    }


    public Vector2 DirectionPointing()
    {
        return direction_point;
    }
}
