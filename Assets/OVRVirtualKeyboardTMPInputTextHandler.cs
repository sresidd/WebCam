using System;
using UnityEngine;

public class OVRVirtualKeyboardTMPInputTextHandler : OVRVirtualKeyboard.AbstractTextHandler
{

    [SerializeField]
    private TMPro.TMP_InputField inputField;

    private bool _isSelected;

    public TMPro.TMP_InputField InputField
    {
        get => inputField;
        set
        {
            if (value == inputField)
            {
                return;
            }
            if (inputField)
            {
                inputField.onValueChanged.RemoveListener(ProxyOnValueChanged);
            }
            inputField = value;
            if (inputField)
            {
                inputField.onValueChanged.AddListener(ProxyOnValueChanged);
            }
            OnTextChanged?.Invoke(Text);
        }
    }

    public override Action<string> OnTextChanged { get; set; }

    public override string Text => inputField ? inputField.text : string.Empty;

    public override bool SubmitOnEnter => inputField && inputField.lineType != TMPro.TMP_InputField.LineType.MultiLineNewline;

    public override bool IsFocused => inputField && inputField.isFocused;

    public override void Submit()
    {
        if (!inputField)
        {
            return;
        }
        inputField.onEndEdit.Invoke(inputField.text);
    }

    public override void AppendText(string s)
    {
        if (!inputField)
        {
            return;
        }
        inputField.text += s;
    }

    public override void ApplyBackspace()
    {
        if (!inputField || string.IsNullOrEmpty(inputField.text))
        {
            return;
        }
        inputField.text = Text.Substring(0, Text.Length - 1);
    }

    public override void MoveTextEnd()
    {
        if (!inputField)
        {
            return;
        }
        inputField.MoveTextEnd(false);
    }

    protected void Start()
    {
        if (inputField)
        {
            inputField.onValueChanged.AddListener(ProxyOnValueChanged);
        }
    }

    protected void ProxyOnValueChanged(string arg0)
    {
        OnTextChanged?.Invoke(arg0);
    }
}
