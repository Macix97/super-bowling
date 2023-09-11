using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [Header(nameof(Gizmos))]
    [SerializeField] private float _radius = 0.5f;
    [SerializeField] private Color _color = Color.white;
    [SerializeField][Range(0.0f, 1.0f)] private float _alpha = 0.2f;

    private void OnEnable()
    {
        XRRig.OnGetSpawnPoint += OnGetSpawnPoint;
    }

    private void OnDisable()
    {
        XRRig.OnGetSpawnPoint -= OnGetSpawnPoint;
    }

    private void OnDrawGizmos()
    {
        Color color = Gizmos.color;
        Gizmos.color = _color;
        Gizmos.DrawWireSphere(transform.position, _radius);
        Gizmos.DrawWireCube(transform.position + transform.forward * _radius, Vector3.one * _radius * 0.5f);
        Gizmos.color = new Color(_color.r, _color.g, _color.b, _alpha);
        Gizmos.DrawSphere(transform.position, _radius);
        Gizmos.color = color;
    }

    private Transform OnGetSpawnPoint() => transform;
}
