using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform m_cameraContainer;
    [SerializeField] private Camera m_camera;

    private void Start()
    {
        if (m_camera == null)
        {
            m_camera = Camera.main;
        }
    }

    public void Shake(float duration = 0.3f, float strength = 0.4f)
    {
        m_cameraContainer.DOShakePosition(duration, strength).OnComplete(() => m_cameraContainer.DOMove(Vector2.zero, 0.05f));
    }

    public void Zoom(float size, Vector2 position, float duration = 0.5f)
    {
        m_camera.DOOrthoSize(size, duration);
        m_camera.transform.DOMove(new Vector3(position.x, position.y, -10), duration);
    }
}
