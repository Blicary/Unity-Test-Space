using UnityEngine;
using System.Collections;

public class MoveInfo : MonoBehaviour 
{
    public Spinner spinner;
    public RollerBladeMovement3 roller_movement;

	// Use this for initialization
	public void Start () 
    {
	
	}
	
	// Update is called once per frame
	public void Update () 
    {
	
	}

    // Calculators
    public float GetAbsAngle(Vector2 p1, Vector2 p2)
    { 
        // Calculates the angle from p1 to p2
        float theta = Mathf.Rad2Deg*Mathf.Atan2(Mathf.Abs(p2.y - p1.y), Mathf.Abs(p2.x - p1.x) );
        //Debug.Log("Theta:" + "(" + (p2.y - p1.y) + ") / (" + (p2.x - p1.x) + ")");
        if (p2.y > p1.y)
        {
            if (p2.x > p1.x)
            {
                return theta;
            }
            else
            {
                return 180 - theta;
            }
        }
        else
        {
            if (p2.x > p1.x)
            {
                return 360 - theta;
            }
            else
            {
                return 180 + theta;
            }
        }
    }
}
