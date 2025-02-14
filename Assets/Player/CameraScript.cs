using UnityEngine;

public class CameraScript : MonoBehaviour // 플레이어 따라오는 카메라
{
    [SerializeField] private float cameraSpeed = 5.0f;
    public GameObject player;

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 targetPosition = player.transform.position;
        targetPosition.z = transform.position.z; 

        transform.position = Vector3.Lerp(transform.position, targetPosition, cameraSpeed * Time.deltaTime);
    }
}
