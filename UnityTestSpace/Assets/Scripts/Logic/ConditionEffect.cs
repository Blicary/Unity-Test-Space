using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConditionEffect : MonoBehaviour 
{
    public List<Condition> conditions;
    public List<Effect> effects;


    public void Check()
    {


        // check if conditions are complete
        foreach (Condition c in conditions)
        {
            if (!c)
            {
                Debug.Log("no conditions specified");
                return;
            }
            if (!c.Met()) return;
        }

        // all conditions met... do actions
        foreach (Effect e in effects)
        {
            if (!e)
            {
                Debug.Log("no effects specified");
                return;
            }
            e.Do();
        }
    }
}
