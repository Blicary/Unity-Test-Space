using UnityEngine;
using System.Collections;

public class RollerBladeMovement2 : MonoBehaviour 
{
    private Spinner spinner;
    public Transform dir_arrow;

    private float last_input_x = 0;

    private float speed_min = 8f, speed_max = 30f;
    private float speed = 0f;
    private float speed_accel = 4f, speed_fall = 9f;

    private float rotation = 0;
    private float rotate_speed_min = 1f, rotate_speed_max = 4.5f;
    private float rotate_speed = 0f;
    private float rotate_speed_accel = 5f;

    private float drag = 1f;


    public void Start()
    {
        spinner = GetComponent<Spinner>();
    }

    public void FixedUpdate()
    {
        if (spinner != null && spinner.OnTrack()) return;

        // input
        Vector2 input = new Vector2();
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        rotation -= input.x * rotate_speed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, 0, (rotation * Mathf.Rad2Deg) - 90);
        Vector2 direction = new Vector2(Mathf.Cos(rotation), Mathf.Sin(rotation));
        

        if (input.x == 0)
        {
            // drag
            rigidbody2D.velocity /= 1 + drag * Time.deltaTime;
            rigidbody2D.velocity = direction * rigidbody2D.velocity.magnitude;

            speed -= speed_fall * 4f * Time.deltaTime;
            speed = Mathf.Max(speed_min, speed);
        }
        else
        {
            rigidbody2D.velocity = direction * speed;

            speed += speed_accel * Time.deltaTime;
            speed = Mathf.Min(speed, speed_max);

            rotate_speed += rotate_speed_accel * Time.deltaTime;
            rotate_speed = Mathf.Min(rotate_speed, rotate_speed_max);
        }

        // switch directions or stop turning
        if (input.x == 0 || Mathf.Sign(last_input_x) == -Mathf.Sign(input.x))
        {
            rotate_speed = rotate_speed_min;
        }

        last_input_x = input.x;
    }

    public void OnSpinnerDetach()
    {
        Reset();
        speed = speed_max;
    }
    public void OnSpinnerAttach()
    {
        transform.rotation = Quaternion.identity;
        dir_arrow.gameObject.SetActive(false);
    }

    private void Reset()
    {
        rotation = Mathf.Atan2(rigidbody2D.velocity.y, rigidbody2D.velocity.x);
        dir_arrow.gameObject.SetActive(true);
    }
}
