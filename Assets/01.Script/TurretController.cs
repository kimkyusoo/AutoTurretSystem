using System.Collections;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    public Transform target;
    public Transform pitchPivot;
    public Transform shootPosition;
    public GameObject projectilePrefab;
    public MeshRenderer turretRenderer;
    public Transform barrel;
    public Transform camTransform;

    public float yawSpeed = 90f;
    public float pitchSpeed = 90f;

    public float shootAngle = 5f;
    public float shootInterval = 1.5f;

    public float minPitch = -45f;
    public float maxPitch = 20f;
    public float recoilDistance = 0.2f;
    public float recoilReturnSpeed = 5f;
    public float shakeDuration = 0.1f;
    public float shakeAmount = 0.1f;

    private float shootCool = -999f;
    private Color originalColor;


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
            StopAllCoroutines();

            StartCoroutine(ChangeColorCoroutine());

            StartCoroutine(RecoilCoroutine());

            StartCoroutine(ShakeCameraCoroutine());
        }
    }

    private IEnumerator ChangeColorCoroutine()
    {
        if (turretRenderer == null) yield break;

        float passedTime = 0f;

        turretRenderer.material.color = Color.red;

        while(passedTime < shootInterval)
        {
            passedTime += Time.deltaTime;

            float progress = passedTime / shootInterval;

            turretRenderer.material.color = Color.Lerp(Color.red, originalColor, progress);

            yield return null;
        }
        turretRenderer.material.color = originalColor;
    }

    private IEnumerator RecoilCoroutine()
    {
        if (barrel == null) yield break;

        Vector3 originPosition = barrel.localPosition;
                                                      
        Vector3 recoilPosition = originPosition - (Vector3.forward * recoilDistance);

        barrel.localPosition = recoilPosition;

        float progress = 0f;
        while (progress < 1f)
        {
            progress += Time.deltaTime * recoilReturnSpeed;
            barrel.localPosition = Vector3.Lerp(recoilPosition, originPosition, progress);
            yield return null;
        }

        barrel.localPosition = originPosition;
    }
    private IEnumerator ShakeCameraCoroutine()
    {
        if (camTransform == null) yield break;

        Vector3 originalCamPos = camTransform.localPosition;
        float passedTime = 0f;

        while (passedTime < shakeDuration)
        {
            
            Vector3 randomOffset = Random.insideUnitSphere * shakeAmount;
            camTransform.localPosition = originalCamPos + randomOffset;

            passedTime += Time.deltaTime;
            yield return null;
        }
        camTransform.localPosition = originalCamPos;
    }
}
