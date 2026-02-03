using UnityEngine; //hello

public class Movement : MonoBehaviour
{
    public float moveSpeed;
    public float raycastHeight = 10f;
    public float raycastDistance = 20f;
    public float terrainOffset = 0.5f; //keeps organism slightly above ground
    public float checkDistance = 1f; //checking for water or terrain
    public LayerMask groundLayer;
    public LayerMask waterLayer;

    float oldAngle = 180;

    private Vector3 moveDirection;
    public float speed;
    private Vector3 lastPosition;
    public bool isRunning = false;

    void Start()
    {
        moveSpeed = Random.Range(20f, 40f);
        PickNewDirection();
    }

    void Update()
    {
        if (!IsGroundAhead() || IsWaterAhead()) //if reached edge of world or water
        {
        PickNewDirection();
        }

        if (!isRunning)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime; //go forward
        }

        RaycastHit hit;
        Vector3 rayOrigin = new Vector3(transform.position.x, transform.position.y + raycastHeight, transform.position.z); //makes sure raycast doesn't hit collider

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, raycastDistance, groundLayer)) //if ground ahead
        {
            Vector3 pos = transform.position;
            pos.y = hit.point.y + terrainOffset; //organism is slightly above terrain
            transform.position = pos;
        }
    }

    void PickNewDirection()
    {
        float angle = Random.Range(0f, 360f);

        while (angle == oldAngle)
        {
            angle = Random.Range(0f, 360f);
        }

        float x = Mathf.Cos(angle);
        float z = Mathf.Sin(angle);


        oldAngle = angle;
        moveDirection = new Vector3(x, 0f, z).normalized; //normalized ensures object moves at constant speed
        Quaternion lookRot = Quaternion.LookRotation(moveDirection);
        transform.rotation = lookRot;
    }

    bool IsGroundAhead()
    {
        Vector3 checkPos = transform.position + moveDirection * checkDistance + Vector3.up * raycastHeight; //current position of organism
        return Physics.Raycast(checkPos, Vector3.down, raycastDistance, groundLayer); //returns true if groundlayer is found
    }

    bool IsWaterAhead()
    {       
        Vector3 checkPos = transform.position + moveDirection * checkDistance + Vector3.up * 0.5f; //current position of organism
        return Physics.Raycast(checkPos, Vector3.down, 1f, waterLayer); //returns true if waterlayer is found
    }

}