using UnityEngine;

public class PlayerLaser : Laser
{
    // Update is called once per frame
    void Update()
    {
        CalculateMovement(Vector3.up);
    }
}