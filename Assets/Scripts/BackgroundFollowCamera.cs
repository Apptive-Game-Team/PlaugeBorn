using UnityEngine;

public class BackgroundFollowCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform; // Cinemachine ī�޶��� Transform
    [SerializeField] private float parallaxFactor = 0.1f; // ���� ī�޶󺸴� ������ ������� ȿ�� (������)

    void Start()
    {
        // Cinemachine Virtual Camera�� Transform�� �ڵ����� ã��
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform; // �⺻ ���� ī�޶� ���
        }
    }

    void Update()
    {
        // ī�޶� ��ġ�� ���� ��� �̵�
        Vector3 newPosition = cameraTransform.position;
        newPosition.z = transform.position.z; // Z���� ���� (��� ���� ����)
        transform.position = newPosition * parallaxFactor + transform.position * (1 - parallaxFactor);
    }
}