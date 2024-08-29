 using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class WebCamInput : MonoBehaviour
{

    [SerializeField] RawImage rawImage;

    public TMP_Dropdown webcamDropdown; // Reference to the Dropdown UI element
    private WebCamTexture _webcam;
    private RenderTexture _buffer;

    private void Start()
    {
        PopulateDropdown();
        webcamDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    private void PopulateDropdown()
    {
        webcamDropdown.ClearOptions();

        WebCamDevice[] devices = WebCamTexture.devices;
        var options = devices.Select(device => device.name).ToList();

        // Add the "Select Webcam" option at the start
        options.Insert(0, "Select Webcam");

        webcamDropdown.AddOptions(options);
        webcamDropdown.value = 0;
        webcamDropdown.RefreshShownValue();
    }

    private void OnDropdownValueChanged(int index)
    {
        // Check if "Select Webcam" is selected
        if (index == 0)
        {
            // Optionally, you can stop any running webcam here
            if (_webcam != null && _webcam.isPlaying)
            {
                _webcam.Stop();
                _webcam = null;
                _buffer = null;
            }
            Debug.Log("Select Webcam option chosen. No webcam started.");
            return;
        }

        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length > 0 && index <= devices.Length)
        {
            string selectedDeviceName = devices[index - 1].name; // Adjust for the "Select Webcam" option
            StartWebcam(selectedDeviceName);
        }
    }

    private void StartWebcam(string deviceName)
    {
        if (_webcam != null && _webcam.isPlaying)
        {
            _webcam.Stop();
            _webcam = null;
            _buffer = null;
        }

        try
        {
            _webcam = new WebCamTexture(deviceName);
            _buffer = new RenderTexture(1920, 1080, 0);
            _webcam.Play();
            Debug.Log($"Started webcam: {deviceName}");
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning(ex.Message);
        }
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

        rawImage.texture = _buffer;
    }
}
