using System;
using UnityEngine;

public class XRRig : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    public Camera Camera => _camera;

    public static event Action<XRRig> OnAwaked;
    public static event Func<Transform> OnGetSpawnPoint;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        OnAwaked?.Invoke(this);
    }

    private void OnEnable()
    {
        Loader.OnFullFade += OnFullFade;
    }

    private void OnDisable()
    {
        Loader.OnFullFade -= OnFullFade;
    }

    private void OnFullFade()
    {
        Transform spawnPoint = OnGetSpawnPoint?.Invoke();
        transform.position = spawnPoint ? spawnPoint.position : Vector3.zero;
        transform.rotation = spawnPoint ? spawnPoint.rotation : Quaternion.identity;
    }
}
