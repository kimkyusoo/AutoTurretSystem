using UnityEngine;

public class TurretController : MonoBehaviour
{
    public Transform target;
    public Transform pitchPivot;
    public Transform shootPosition;
    public GameObject projectilePrefab;

    public float yawSpeed = 90f;
    public float pitchSpeed = 90f;

    public float shootAngle = 5f;
    public float shootInterval = 0.5f;

    public float minPitch = -45f;
    public float maxPitch = 20f;

    private float shootCool = -999f;


    private void Update()
    {
        if (target == null || pitchPivot == null) return;

        if (shootPosition == null) return;

        Vector3 directionToTarget = target.position - transform.position;
        Vector3 yawDirection = new Vector3(directionToTarget.x, 0f, directionToTarget.z);

        if (yawDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(yawDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, yawSpeed * Time.deltaTime);
        }

        float distance = new Vector2(directionToTarget.x, directionToTarget.z).magnitude;

        float barrelPosition = target.position.y - pitchPivot.position.y;
        float targetPitchAngle = -Mathf.Atan2(barrelPosition, distance) * Mathf.Rad2Deg;

        targetPitchAngle = Mathf.Clamp(targetPitchAngle, minPitch, maxPitch);

        Quaternion targetPitch = Quaternion.Euler(targetPitchAngle, 0, 0);
        pitchPivot.localRotation = Quaternion.RotateTowards(pitchPivot.localRotation, targetPitch, pitchSpeed * Time.deltaTime);

        Vector3 shootDirection = (target.position - shootPosition.position).normalized;
        float currentAngle = Vector3.Angle(shootPosition.forward, shootDirection);

        if (currentAngle < shootAngle)
        {
            if(Time.time >= shootCool + shootInterval)
            {
                Shoot();
                shootCool = Time.time;
            }
        }
    }

    private void Shoot()
    {
        if (projectilePrefab != null)
        {
            Instantiate(projectilePrefab, shootPosition.position, shootPosition.rotation);
        }
    }
}
