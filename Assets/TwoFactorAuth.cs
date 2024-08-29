using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Net;
using System.Net.Mail;
using TMPro;
using System;

public class TwoFactorAuth : MonoBehaviour
{
    public TMP_InputField otpInput;
    public Button verifyButton;
    public Button getOTP;
    public TMP_Text message;

    private string generatedOTP;
    [SerializeField] private string userEmail = "user@example.com"; // Replace with recipient's email for testing
    [SerializeField] private string senderEmail = "your-email@gmail.com"; // Replace with sender's email
    [SerializeField] private string senderPassword = "your-email-password"; // Replace with sender's email password

    void Start()
    {

        CheckRemainingTime();


        verifyButton.onClick.AddListener(OnVerifyOTP);
        getOTP.onClick.AddListener(GetOTP);
    }

    private void CheckRemainingTime()
    {
        // Load the remaining time from PlayerPrefs if it exists
        if (PlayerPrefs.HasKey(Timer.TimerKey))
        {
            Timer.timer = PlayerPrefs.GetFloat(Timer.TimerKey);
        }
        // If time is up, reset to login scene
        if (Timer.timer > 0)
        {
            SceneManager.LoadScene("Main");
        }
    }

    [ContextMenu("Get OTP")]
    public void GetOTP()
    {
        generatedOTP = GenerateOTP();
        SendOTPEmail(userEmail, generatedOTP);
    }

    [ContextMenu("Verify OTP")]
    public void OnVerifyOTP()
    {
        string enteredOTP = otpInput.text;
        if (enteredOTP == generatedOTP)
        {
            message.text = "Login Successful";
            Debug.Log("Login successful");
            SceneManager.LoadScene("Main");
        }
        else
        {
            message.text = "Invalid OTP";
            Debug.Log("Invalid OTP");
        }
    }

    string GenerateOTP()
    {
        System.Random random = new System.Random();
        return random.Next(100000, 999999).ToString();
    }

    void SendOTPEmail(string toEmail, string otp)
    {
        MailMessage mail = new()
        {
            From = new MailAddress(senderEmail)
        };
        mail.To.Add(toEmail);
        mail.Subject = "Your OTP Code";
        mail.Body = "Your OTP code is: " + otp;

        SmtpClient smtpServer = new("smtp.office365.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(senderEmail, senderPassword),
            EnableSsl = true
        };

        ServicePointManager.ServerCertificateValidationCallback = 
            delegate { return true; };

        try
        {
            smtpServer.Send(mail);
            Debug.Log("OTP sent to email");
        }
        catch (System.Exception e)
        {
            Debug.Log("Failed to send OTP email: " + e.Message);
        }
    }
}
