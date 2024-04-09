using System;
using Oculus.Platform.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WebcamInput : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] string _deviceName = "";
    [SerializeField] RawImage rawImage;

    [SerializeField] private TMPro.TMP_InputField cameraName;
    [SerializeField] private Button startCam;

    #endregion

    #region Internal objects

    WebCamTexture _webcam;
    RenderTexture _buffer;

    #endregion

    #region Public properties

    public Texture Texture => _buffer;

    #endregion

    #region MonoBehaviour implementation

    void Start()
    {
        cameraName.onValueChanged.AddListener(OnCameraNameSubmit);
        startCam.onClick.AddListener(OnStartCam);
    }

    private void OnStartCam()
    {
        _webcam = null;
        _buffer = null;

        try
        {
            _webcam = new WebCamTexture(_deviceName);
            _buffer = new RenderTexture(1920, 1080, 0);
            _webcam.Play();
        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex.Message);
        }
    }
    private void OnCameraNameSubmit(string cameraName)
    {
        _deviceName = cameraName;
    }

    void OnDestroy()
    {
        Destroy(_webcam);
        Destroy(_buffer);
    }

    void Update()
    {
        if (_webcam == null) return;
        if (!_webcam.didUpdateThisFrame) return;
        var vflip = _webcam.videoVerticallyMirrored;
        var scale = new Vector2(1, vflip ? -1 : 1);
        var offset = new Vector2(0, vflip ? 1 : 0);
        Graphics.Blit(_webcam, _buffer, scale, offset);

        rawImage.texture = Texture;
    }



    #endregion
}
