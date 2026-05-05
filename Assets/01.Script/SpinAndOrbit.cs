using UnityEngine;

public class SpinAndOrbit : MonoBehaviour
{
    public Transform orbitPivot;
    public Transform body;

    public float orbitSpeed = 30f;
    public float spinSpeed = 50f;

    private void Update()
    {
        if( orbitPivot != null)
        {
            orbitPivot.Rotate(Vector3.up, orbitSpeed * Time.deltaTime, Space.Self);
        }

        if (body != null)
        {
            body.Rotate(Vector3.up, orbitSpeed * Time.deltaTime, Space.Self);
        }
    }
}
