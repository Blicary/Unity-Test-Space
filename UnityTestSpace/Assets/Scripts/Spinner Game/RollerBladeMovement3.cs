using UnityEngine;
using System.Collections;

public class RollerBladeMovement3 : MonoBehaviour 
{
    private Spinner spinner;
    public Transform dir_arrow;

    private float speed = 20f;

    private float rotation = 0;
    private float rotate_speed = 5f;

    private float drag = 0.5f;
    private float break_drag = 6.5f;
    private bool break_turned = false;
    private float break_turn_speed_threshold = 8f;



    public void Start()
    {
        spinner = GetComponent<Spinner>();
    }

    public void Update()
    {
        if (spinner != null && spinner.OnTrack()) return;

        // input
        float input_steer = Input.GetAxis("Horizontal");
        bool input_fwrd = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        bool input_back = Input.GetButton("Back");
        bool input_back_up = Input.GetButtonUp("Back");
        bool input_back_press = Input.GetButtonDown("Back");

        // rotation
        rotation -= input_steer * rotate_speed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, 0, (rotation * Mathf.Rad2Deg) - 90);
        Vector2 direction = new Vector2(Mathf.Cos(rotation), Mathf.Sin(rotation));



        if (input_back)
        {
            rigidbody2D.velocity /= 1 + break_drag * Time.deltaTime;

            if (input_fwrd && !break_turned && rigidbody2D.velocity.magnitude <= break_turn_speed_threshold)
            {
                rotation += Mathf.PI;
                transform.rotation = Quaternion.Euler(0, 0, (rotation * Mathf.Rad2Deg) - 90);

                break_turned = true;
            }
        }
        else
        {
            if (input_back_up)
                break_turned = false;


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


        


        //Debug.Log(rigidbody2D.velocity.magnitude);
    }

    public void OnSpinnerDetach()
    {
        Reset();
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
