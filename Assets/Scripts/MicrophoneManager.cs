using Academy.HoloToolkit.Unity;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class MicrophoneManager : MonoBehaviour
{
    [Tooltip("A text area for the recognizer to display the recognized strings.")]
    public Text DictationDisplay;

    private DictationRecognizer dictationRecognizer;

    // Use this string to cache the text currently displayed in the text box.
    private StringBuilder textSoFar;

    // Using an empty string specifies the default microphone. 
    private static string deviceName = string.Empty;
    private int samplingRate;
    private const int messageLength = 10;

    // Use this to reset the UI once the Microphone is done recording after it was started.
    private bool hasRecordingStarted;

    public GameObject option1;
    public GameObject option2;
    public GameObject option3;

    void Start()
    {
        option1.SetActive(false);
        option2.SetActive(false);
        option3.SetActive(false);
    }

    void Awake()
    {
        // Create a new DictationRecognizer and assign it to dictationRecognizer variable.
        dictationRecognizer = new DictationRecognizer();

        // Register for dictationRecognizer.DictationHypothesis and implement DictationHypothesis below
        // This event is fired while the user is talking. As the recognizer listens, it provides text of what it's heard so far.
        dictationRecognizer.DictationHypothesis += DictationRecognizer_DictationHypothesis;

        // Register for dictationRecognizer.DictationResult and implement DictationResult below
        // This event is fired after the user pauses, typically at the end of a sentence. The full recognized string is returned here.
        dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;

        // Register for dictationRecognizer.DictationComplete and implement DictationComplete below
        // This event is fired when the recognizer stops, whether from Stop() being called, a timeout occurring, or some other error.
        dictationRecognizer.DictationComplete += DictationRecognizer_DictationComplete;

        // Register for dictationRecognizer.DictationError and implement DictationError below
        // This event is fired when an error occurs.
        dictationRecognizer.DictationError += DictationRecognizer_DictationError;

        // Query the maximum frequency of the default microphone. Use 'unused' to ignore the minimum frequency.
        int unused;
        Microphone.GetDeviceCaps(deviceName, out unused, out samplingRate);

        // Use this string to cache the text currently displayed in the text box.
        textSoFar = new StringBuilder();

        // Use this to reset the UI once the Microphone is done recording after it was started.
        hasRecordingStarted = false;
        DictationDisplay.text = "Handsight\nSpeak command to change modes:\nOption 1\nOption 2\nOption 3\nInstructions";
        StartRecording();
    }

    void Update()
    {
        // Add condition to check if dictationRecognizer.Status is Running
        if (hasRecordingStarted && !Microphone.IsRecording(deviceName) && dictationRecognizer.Status == SpeechSystemStatus.Running)
        {
            // Reset the flag now that we're cleaning up the UI.
            hasRecordingStarted = false;

            // This acts like pressing the Stop button and sends the message to the Communicator.
            // If the microphone stops as a result of timing out, make sure to manually stop the dictation recognizer.
            // Look at the StopRecording function.
            //SendMessage("RecordStop");
            //StopRecording();
        }
    }

    void OnDestroy()
    {
        StopRecording();
    }

    /// <summary>
    /// Turns on the dictation recognizer and begins recording audio from the default microphone.
    /// </summary>
    /// <returns>The audio clip recorded from the microphone.</returns>
    public AudioClip StartRecording()
    {
        // Shutdown the PhraseRecognitionSystem. This controls the KeywordRecognizers
        PhraseRecognitionSystem.Shutdown();

        // Start dictationRecognizer
        dictationRecognizer.Start();

        // Set the flag that we've started recording.
        hasRecordingStarted = true;

        // Start recording from the microphone for 10 seconds.
        return Microphone.Start(deviceName, false, messageLength, samplingRate);
    }

    /// <summary>
    /// Ends the recording session.
    /// </summary>
    public void StopRecording()
    {
        // Check if dictationRecognizer.Status is Running and stop it if so
        if (dictationRecognizer.Status == SpeechSystemStatus.Running)
        {
            dictationRecognizer.Stop();
        }

        Microphone.End(deviceName);
    }

    /// <summary>
    /// This event is fired while the user is talking. As the recognizer listens, it provides text of what it's heard so far.
    /// </summary>
    /// <param name="text">The currently hypothesized recognition.</param>
    private void DictationRecognizer_DictationHypothesis(string text)
    {
        // Set DictationDisplay text to be textSoFar and new hypothesized text
        // We don't want to append to textSoFar yet, because the hypothesis may have changed on the next event
        // DictationDisplay.text = textSoFar.ToString() + " " + text + "...";
        string heardText = textSoFar.ToString().ToLower();
        if (heardText.Contains("option one") || heardText.Contains("option 1"))
        {
            DictationDisplay.text = textSoFar.ToString() + " " + text + "...";
        }
        else if (heardText.Contains("option two") || heardText.Contains("option 2"))
        {
            DictationDisplay.text = textSoFar.ToString() + " " + text + "...";
        }
        else if (heardText.Contains("option three") || heardText.Contains("option 3"))
        {
            DictationDisplay.text = textSoFar.ToString() + " " + text + "...";
        }
    }

    /// <summary>
    /// This event is fired after the user pauses, typically at the end of a sentence. The full recognized string is returned here.
    /// </summary>
    /// <param name="text">The text that was heard by the recognizer.</param>
    /// <param name="confidence">A representation of how confident (rejected, low, medium, high) the recognizer is of this recognition.</param>
    private void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence)
    {
        // Append textSoFar with latest text
        textSoFar.Append(text + ". ");

        // Set DictationDisplay text to be textSoFar
        string heardText = textSoFar.ToString().ToLower();

        if (heardText.Contains("instructions"))
        {
            DictationDisplay.text = textSoFar.ToString();
            option1.SetActive(false);
            option2.SetActive(false);
            option3.SetActive(false);
            textSoFar.Length = 0;
            DictationDisplay.text = "Handsight\nSpeak command to change modes:\nOption 1\nOption 2\nOption 3\nInstructions";
        }
        else
        {
            if (heardText.Contains("option one") || heardText.Contains("option 1"))
            {
                DictationDisplay.text = textSoFar.ToString();
                enableOption1();
            }
            else if (heardText.Contains("option two") || heardText.Contains("option 2"))
            {
                DictationDisplay.text = textSoFar.ToString();
                enableOption2();
            }
            else if (heardText.Contains("option three") || heardText.Contains("option 3"))
            {
                DictationDisplay.text = textSoFar.ToString();
                enableOption3();
            }
            textSoFar.Length = 0;
            DictationDisplay.text = "";
        }
    }

    /// <summary>
    /// This event is fired when the recognizer stops, whether from Stop() being called, a timeout occurring, or some other error.
    /// Typically, this will simply return "Complete". In this case, we check to see if the recognizer timed out.
    /// </summary>
    /// <param name="cause">An enumerated reason for the session completing.</param>
    private void DictationRecognizer_DictationComplete(DictationCompletionCause cause)
    {
        // If Timeout occurs, the user has been silent for too long.
        // With dictation, the default timeout after a recognition is 20 seconds.
        // The default timeout with initial silence is 5 seconds.
        if (cause == DictationCompletionCause.TimeoutExceeded)
        {
            Microphone.End(deviceName);

            DictationDisplay.text = "Dictation has timed out. Restarting dictation recognizer.";
            //SendMessage("ResetAfterTimeout");
            DictationDisplay.text = " ";
            StartRecording();
        }
    }

    /// <summary>
    /// This event is fired when an error occurs.
    /// </summary>
    /// <param name="error">The string representation of the error reason.</param>
    /// <param name="hresult">The int representation of the hresult.</param>
    private void DictationRecognizer_DictationError(string error, int hresult)
    {
        // 3.a: Set DictationDisplay text to be the error string
        DictationDisplay.text = error + "\nHRESULT: " + hresult;
    }

    public void enableOption1()
    {
        // camera display is fixed to the screen 
        if (option2.activeInHierarchy)
        {
            option2.SetActive(false);
        }
        if (option3.activeInHierarchy)
        {
            option3.SetActive(false);
        }
        option1.SetActive(true);
    }

    public void enableOption2()
    {
        // camera follow's users view, stays facing user, on tap gesture the view will stop following
        if (option1.activeInHierarchy)
        {
            option1.SetActive(false);
        }
        if (option3.activeInHierarchy)
        {
            option3.SetActive(false);
        }
        option2.SetActive(true);
    }

    public void enableOption3()
    {
        // same as Option2 but view faces upward so that it lays flat on a surface (ie, a table)
        if (option1.activeInHierarchy)
        {
            option1.SetActive(false);
        }
        if (option2.activeInHierarchy)
        {
            option2.SetActive(false);
        }
        option3.SetActive(true);
    }
}
