using UnityEngine;
using System.Collections;

public class PlayerCam : MonoBehaviour 
{
    public Transform player;

    public void Update()
    {
        Vector3 target_pos = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, target_pos, Time.deltaTime * 4f);
    }
	
}
