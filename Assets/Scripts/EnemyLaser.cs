using UnityEngine;

public class EnemyLaser : Laser
{
    // Update is called once per frame
    void Update()
    {
        CalculateMovement(Vector3.down);
    }
}