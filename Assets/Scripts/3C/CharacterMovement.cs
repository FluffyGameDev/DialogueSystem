using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    private float m_MovementSpeed = 1.0f;
    [SerializeField]
    private Transform m_CameraTransform;

    private CharacterController m_CharacterController;

    private void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Vector3 forward = (transform.position - m_CameraTransform.position).normalized;
        forward.y = 0;
        Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;

        float moveAxisX = m_MovementSpeed * Input.GetAxis("Horizontal");
        float moveAxisY = m_MovementSpeed * Input.GetAxis("Vertical");
        Vector3 movement = forward * moveAxisY + right * moveAxisX;

        m_CharacterController.Move(movement);
    }
}
