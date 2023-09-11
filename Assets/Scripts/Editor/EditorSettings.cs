using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = nameof(EditorSettings))]
public class EditorSettings : ScriptableObject
{
    private static EditorSettings _instance;

    [SerializeField] private Scene _startScene;
    [SerializeField] private GameObject _xrRigPrefab;
    [SerializeField] private GameObject _ambientPrefab;

    private XRRig _xrRigInstance;
    private Ambient _ambientInstance;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void BeforeSceneLoaded()
    {
        _instance = AssetDatabase.LoadAssetAtPath<EditorSettings>(Path.Combine("Assets", $"{nameof(Editor)}{nameof(Resources)}", $"{nameof(EditorSettings)}.asset"));
        if (SceneManager.GetActiveScene().name != _instance._startScene.Value)
        {
            _instance._xrRigInstance = Instantiate(_instance._xrRigPrefab).GetComponent<XRRig>();
            _instance._ambientInstance = Instantiate(_instance._ambientPrefab).GetComponent<Ambient>();
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void AfterSceneLoaded()
    {
        InitializeXRRig();
        InitializeAmbient();
    }

    private static void InitializeXRRig()
    {
        if (!_instance._xrRigInstance) return;
        Transform spawnPoint = GameObject.FindWithTag(nameof(SpawnPoint)).transform;
        _instance._xrRigInstance.transform.position = spawnPoint.position;
        _instance._xrRigInstance.transform.rotation = spawnPoint.rotation;
    }

    private static void InitializeAmbient()
    {
        if (!_instance._ambientInstance) return;
        _instance._ambientInstance.StartTransition();
    }
}
