using UnityEngine;
using System.Collections;

public class RangeAimer : MonoBehaviour 
    {

    public float aim_direction = 0.0f;
    private Spinner spinner;
    public Transform graphics;
    public MoveInfo move_info;

	// Use this for initialization
	public void Start () 
    {
	        spinner = GetComponent<Spinner>();
	}
	
	// Update is called once per frame
	public void Update () 
    {
       if (spinner.OnTrack())
        {
            Vector2 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 current_pos = (Vector2) transform.position;
            //Debug.Log(mouse_pos);
            //aim_direction = Mathf.Atan2(mouse_pos,current_pos);
            aim_direction = move_info.GetAbsAngle(current_pos,mouse_pos);
            graphics.localEulerAngles = new Vector3(0, 0, aim_direction-90);
        }
	}
}
