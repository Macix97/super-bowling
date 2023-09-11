using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-10)]
public class Loader : MonoBehaviour
{
    [SerializeField] private float _startDelay = 2.0f;
    [SerializeField] private float _fadingTime = 0.5f;
    [SerializeField] private string _colorProperty;
    [SerializeField] private Color _fadingColor = Color.white;
    [SerializeField] private Scene _loadScene;
    [SerializeField] private Renderer _fader;

    private WaitForSeconds _waitForSeconds;
    private MaterialPropertyBlock _materialPropertyBlock;

    public static event Action OnFullFade;

    private void Awake()
    {
        _fader.enabled = false;
        _materialPropertyBlock = new MaterialPropertyBlock();
        _fader.GetPropertyBlock(_materialPropertyBlock);
        _waitForSeconds = new WaitForSeconds(_startDelay);
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        XRRig.OnAwaked += SetFaderParent;
    }

    private void OnDisable()
    {
        XRRig.OnAwaked -= SetFaderParent;
    }

    private IEnumerator Start()
    {
        yield return _waitForSeconds;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(_loadScene.Value);
        asyncOperation.allowSceneActivation = false;
        while (asyncOperation.progress < 0.9f) yield return null;
        _fader.enabled = true;
        yield return OnTransition(true);
        asyncOperation.allowSceneActivation = true;
        while (!asyncOperation.isDone) yield return null;
        yield return null;
        OnFullFade?.Invoke();
        yield return OnTransition(false);
        _fader.enabled = false;
    }

    private IEnumerator OnTransition(bool fade)
    {
        float sign = fade ? 1.0f : -1.0f;
        float alpha = fade ? 0.0f : 1.0f;
        float endValue = 1.0f - alpha;
        while (!Mathf.Approximately(alpha, endValue))
        {
            alpha += sign * Time.unscaledDeltaTime / _fadingTime;
            alpha = Mathf.Clamp01(alpha);
            _fadingColor.a = alpha;
            _materialPropertyBlock.SetColor(_colorProperty, _fadingColor);
            _fader.SetPropertyBlock(_materialPropertyBlock);
            yield return null;
        }
    }

    private void SetFaderParent(XRRig xrRig)
    {
        _fader.transform.SetParent(xrRig.Camera.transform, false);
    }
}
