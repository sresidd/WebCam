using System.Collections;
using TMPro;
using UnityEngine;

public class KeyboardController : MonoBehaviour
{
    [SerializeField] private OVRVirtualKeyboard keyboard;
    [SerializeField] private Transform canvasPosition;
    [SerializeField] private Vector3 offset;

    private void Start()
    {

        // keyboard.UseSuggestedLocation(OVRVirtualKeyboard.KeyboardPosition.Custom);
    }
    [ContextMenu("Move Near")]
    public void MoveKeyboardNear()
    {
        if (!keyboard.gameObject.activeSelf) return;
        keyboard.UseSuggestedLocation(OVRVirtualKeyboard.KeyboardPosition.Near);
    }

    [ContextMenu("Move Far")]
    public void MoveKeyboardFar()
    {
        if (!keyboard.gameObject.activeSelf) return;
        keyboard.UseSuggestedLocation(OVRVirtualKeyboard.KeyboardPosition.Far);
    }

    [ContextMenu("Show Keyboard")]
    public void ShowKeyboard()
    {
        keyboard.gameObject.SetActive(true);
    }

    [ContextMenu("Hide Keyboard")]
        public void HideKeyboard()
    {
        keyboard.gameObject.SetActive(false);
    }


}
