using UnityEngine;
using System.Collections;

public class RollerBladeMovement3 : MonoBehaviour 
{
    private Spinner spinner;
    public Transform graphics, graphics_arrow;

    private float speed = 20f;

    private float rotation = 0;
    public float rotate_speed = 5f;

    private float drag = 0.5f;
    private float break_drag = 6.5f;
    private bool break_turned = false;
    private float break_turn_speed_threshold = 8f;

    private float input_steer = 0;
    private bool input_fwrd = false;
    private bool input_back = false;
    private bool input_back_up = false;

    public Vector2 direction = new Vector2();


    public void Start()
    {
        spinner = GetComponent<Spinner>();
    }

    public void Update()
    {
        if (spinner != null && spinner.OnTrack()) return;

        // input
        input_steer = Input.GetAxis("Horizontal");
        input_fwrd = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        input_back = Input.GetButton("Back");
        input_back_up = Input.GetButtonUp("Back");

        // rotation
        rotation -= input_steer * rotate_speed * Time.deltaTime;
        graphics.localEulerAngles = new Vector3(0, 0, (rotation * Mathf.Rad2Deg) - 90);
        direction = new Vector2(Mathf.Cos(rotation), Mathf.Sin(rotation));

        // break turn reset
        if (input_back_up)
        {
            break_turned = false;
        }
    }
    public void FixedUpdate()
    {
        if (spinner != null && spinner.OnTrack()) return;


        // forward and break movement
        if (input_back)
        {
            rigidbody2D.velocity /= 1 + break_drag * Time.deltaTime;

            if (input_fwrd && !break_turned && rigidbody2D.velocity.magnitude <= break_turn_speed_threshold)
            {
                rotation += Mathf.PI;
                graphics.rotation = Quaternion.Euler(0, 0, (rotation * Mathf.Rad2Deg) - 90);

                break_turned = true;
            }
        }
        else
        {
            if (!input_fwrd || rigidbody2D.velocity.magnitude > speed)
            {
                // drag
                rigidbody2D.velocity /= 1 + drag * Time.deltaTime;
                rigidbody2D.velocity = direction * rigidbody2D.velocity.magnitude;
            }
            else if (input_fwrd)
            {
                rigidbody2D.velocity = direction * Mathf.Max(speed, rigidbody2D.velocity.magnitude);
            }
        }
    }


    public void OnSpinnerDetach()
    {
        Reset();
    }
    public void OnSpinnerAttach()
    {
        transform.rotation = Quaternion.identity;
        //graphics_arrow.gameObject.SetActive(false);
    }

    private void Reset()
    {
        rotation = Mathf.Atan2(rigidbody2D.velocity.y, rigidbody2D.velocity.x);
        //graphics_arrow.gameObject.SetActive(true);
    }
}
