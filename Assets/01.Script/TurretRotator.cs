using UnityEngine;

public class TurretRotator : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0f, 40f, 0f);

    public Space rotationSpace = Space.Self;

    private void Update()
    {
        transform.Rotate(rotationSpeed *  Time.deltaTime, rotationSpace);
    }
}
