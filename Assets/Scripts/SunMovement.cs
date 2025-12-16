using UnityEngine;

public class SunMovement : MonoBehaviour
{
    Vector3 rotate = Vector3.zero;
    float degrees = 0.6F; //10 minutes
    
    // Update is called once per frame
    void Update()
    {
        rotate.x = degrees * Time.deltaTime;
        transform.Rotate(rotate, Space.World);
    }
}
