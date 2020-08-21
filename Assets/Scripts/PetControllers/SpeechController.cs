using UnityEngine;
using System.Collections;
using GameData;
using System;
using UnityEngine.Networking;
using System.Net.Http;
using System.Text;
using System.IO;
using System.Threading.Tasks;

public class SpeechController : MonoBehaviour {

    private const string SUBSCRIPTION_KEY = "576eae4bd71b4212843ca73772fb72ea";
    private const string ACCESS_TOKEN_URL = "https://eastus.api.cognitive.microsoft.com/sts/v1.0/issueToken";
    private const string VOICE_URL = "https://eastus.voice.speech.microsoft.com/cognitiveservices/v1?deploymentId=c1ec50b0-bb55-4426-9bd7-ee8a135068cd";

    private static string _accessKey;

    public bool IsSpeaking { get { return _audioSource.isPlaying; } }

    [SerializeField] private DialogueData _dialogueData;
    [SerializeField] private AudioSource _audioSource;

    private Skin _skin;
    private Stream _audioStream;
    private Task _downloadTask;

    private void Start() {
        _skin = GetComponent<Skin>();
        MessengerServer.singleton.SetHandler(NetMsgInds.SpeechMessage, OnSpeechMessage);

        if(string.IsNullOrEmpty(_accessKey))
        {
            StartCoroutine(GetAccessToken());
        }
    }

    private IEnumerator GetAccessToken()
    {
        UnityWebRequest www = UnityWebRequest.Post(ACCESS_TOKEN_URL, "");
        www.SetRequestHeader("Ocp-Apim-Subscription-Key", SUBSCRIPTION_KEY);
        www.SetRequestHeader("Content-type", "application/x-www-form-urlencoded");

        using (www)
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log("Error getting speech access key: " + www.error);
            }
            else
            {
                Debug.Log("Got speech access key");
                _accessKey = www.downloadHandler.text;

                Speak("Hello james. My name is sweetango. I love to eat apples!");

            }
        }
    }

    private void OnSpeechMessage(string msg)
    {
        Speak(msg);
    }

    public void Speak(string text)
    {
        PrepareSpeechClip(text);
        PlayPreparedClip();
    }

    public void PrepareSpeechClip(string text)
    {
        if (string.IsNullOrEmpty(_accessKey))
        {
            Debug.LogError("Cannot speak, access key null");
            return;
        }

        Debug.Log("Preparing clip for: " + text);
        _downloadTask = SpeechAsync(text);
    }

    public void PlayPreparedClip()
    {
        if(_downloadTask == null)
        {
            Debug.LogError("Must prepare speech clip first!");
            return;
        }

        StartCoroutine(SpeechRoutine());
    }

    private IEnumerator SpeechRoutine()
    {
        while (!_downloadTask.IsCompleted)
        {
            yield return null;
        }

        _skin.emoteController.StartSpeakEmote();

        PlayAudio(_audioStream);
        _downloadTask = null;

        while(IsSpeaking)
        {
            yield return null;
        }

        _skin.emoteController.StopSpeakEmote();
    }

    private async Task SpeechAsync(string text)
    {
        // A sample SSML
        string body = "<speak version=\"1.0\" xmlns=\"http://www.w3.org/2001/10/synthesis\" xmlns:mstts=\"http://www.w3.org/2001/mstts\" xml:lang=\"en-US\"><voice name=\"harry\">" + text + "</voice></speak>";

        // Get audio from endpoint
        using (HttpClient client = new HttpClient())
        {
            using (HttpRequestMessage request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(VOICE_URL);
                request.Content = new StringContent(body, Encoding.UTF8, "application/ssml+xml");
                request.Headers.Add("Authorization", "Bearer " + _accessKey);
                request.Headers.Add("Connection", "Keep-Alive");
                request.Headers.Add("X-Microsoft-OutputFormat", "riff-24khz-16bit-mono-pcm");
                
                using (HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();
                    _audioStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                }
            }
        }
    }

    private void PlayAudio(Stream audioStream)
    {
        // Play the audio using Unity AudioSource, allowing us to benefit from effects,
        // spatialization, mixing, etc.

        // Get the size of the original stream
        var size = audioStream.Length;

        // Don't playback if the stream is empty
        if (size > 0)
        {
            try
            {
                // Create buffer
                byte[] buffer = new byte[size];
                
                buffer = ReadToEnd(audioStream);

                // Convert raw WAV data into Unity audio data
                int sampleCount = 0;
                int frequency = 0;
                var unityData = AudioWithHeaderToUnityAudio(buffer, out sampleCount, out frequency);

                // Convert data to a Unity audio clip
                var clip = ToClip("Speech", unityData, sampleCount, frequency);

                // Set the source on the audio clip
                _audioSource.clip = clip;
                
                // Play audio
                _audioSource.Play();
            }
            catch (Exception ex)
            {
                Debug.Log("An error occurred during audio stream conversion and playback."
                           + Environment.NewLine + ex.Message);
            }
        }
    }

    /// <summary>
    /// Dynamically creates an <see cref="AudioClip"/> that represents raw Unity audio data.
    /// </summary>
    /// <param name="name"> The name of the dynamically generated clip.</param>
    /// <param name="audioData">Raw Unity audio data.</param>
    /// <param name="sampleCount">The number of samples in the audio data.</param>
    /// <param name="frequency">The frequency of the audio data.</param>
    /// <returns>The <see cref="AudioClip"/>.</returns>
    private static AudioClip ToClip(string name, float[] audioData, int sampleCount, int frequency)
    {
        var clip = AudioClip.Create(name, sampleCount, 1, frequency, false);
        clip.SetData(audioData, 0);
        return clip;
    }

    /// <summary>
    /// Converts raw WAV data into Unity formatted audio data.
    /// </summary>
    /// <param name="wavAudio">The raw WAV data.</param>
    /// <param name="sampleCount">The number of samples in the audio data.</param>
    /// <param name="frequency">The frequency of the audio data.</param>
    /// <returns>The Unity formatted audio data. </returns>
    private static float[] AudioWithHeaderToUnityAudio(byte[] wavAudio, out int sampleCount, out int frequency)
    {
        // Determine if mono or stereo
        int channelCount = wavAudio[22];  // Speech audio data is always mono but read actual header value for processing

        // Get the frequency
        frequency = BytesToInt(wavAudio, 24);

        // Get past all the other sub chunks to get to the data subchunk:
        int pos = 12; // First subchunk ID from 12 to 16

        // Keep iterating until we find the data chunk (i.e. 64 61 74 61 ...... (i.e. 100 97 116 97 in decimal))
        while (!(wavAudio[pos] == 100 && wavAudio[pos + 1] == 97 && wavAudio[pos + 2] == 116 && wavAudio[pos + 3] == 97))
        {
            pos += 4;
            int chunkSize = wavAudio[pos] + wavAudio[pos + 1] * 256 + wavAudio[pos + 2] * 65536 + wavAudio[pos + 3] * 16777216;
            pos += 4 + chunkSize;
        }
        pos += 8;

        // Pos is now positioned to start of actual sound data.
        sampleCount = (wavAudio.Length - pos) / 2;  // 2 bytes per sample (16 bit sound mono)
        if (channelCount == 2) { sampleCount /= 2; }  // 4 bytes per sample (16 bit stereo)

        // Allocate memory (supporting left channel only)
        var unityData = new float[sampleCount];

        try
        {
            // Write to double array/s:
            int i = 0;
            while (pos < wavAudio.Length)
            {
                unityData[i] = BytesToFloat(wavAudio[pos], wavAudio[pos + 1]);
                pos += 2;
                if (channelCount == 2)
                {
                    pos += 2;
                }
                i++;
            }
        }
        catch (Exception ex)
        {
            Debug.Log($"Error occurred converting audio data to float array of size {wavAudio.Length} at position {pos}.");
        }

        return unityData;
    }

    /// <summary>
    /// Converts two bytes to one float in the range -1 to 1.
    /// </summary>
    /// <param name="firstByte">The first byte.</param>
    /// <param name="secondByte"> The second byte.</param>
    /// <returns>The converted float.</returns>
    private static float BytesToFloat(byte firstByte, byte secondByte)
    {
        // Convert two bytes to one short (little endian)
        short s = (short)((secondByte << 8) | firstByte);

        // Convert to range from -1 to (just below) 1
        return s / 32768.0F;
    }

    /// <summary>
    /// Converts an array of bytes to an integer.
    /// </summary>
    /// <param name="bytes"> The byte array.</param>
    /// <param name="offset"> An offset to read from.</param>
    /// <returns>The converted int.</returns>
    private static int BytesToInt(byte[] bytes, int offset = 0)
    {
        int value = 0;
        for (int i = 0; i < 4; i++)
        {
            value |= ((int)bytes[offset + i]) << (i * 8);
        }
        return value;
    }

    /// <summary>
    /// Reads a stream from beginning to end, returning an array of bytes
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    public static byte[] ReadToEnd(Stream stream)
    {
        long originalPosition = 0;

        if (stream.CanSeek)
        {
            originalPosition = stream.Position;
            stream.Position = 0;
        }

        try
        {
            byte[] readBuffer = new byte[4096];

            int totalBytesRead = 0;
            int bytesRead;

            while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
            {
                totalBytesRead += bytesRead;

                if (totalBytesRead == readBuffer.Length)
                {
                    int nextByte = stream.ReadByte();
                    if (nextByte != -1)
                    {
                        byte[] temp = new byte[readBuffer.Length * 2];
                        Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                        Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                        readBuffer = temp;
                        totalBytesRead++;
                    }
                }
            }

            byte[] buffer = readBuffer;
            if (readBuffer.Length != totalBytesRead)
            {
                buffer = new byte[totalBytesRead];
                Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
            }
            return buffer;
        }
        finally
        {
            if (stream.CanSeek)
            {
                stream.Position = originalPosition;
            }
        }
    }


    public void PrepareRandomDialogue()
    {
        int ind = UnityEngine.Random.Range(0, _dialogueData.IdleDialogues.Length);
        PrepareSpeechClip(_dialogueData.IdleDialogues[ind]);
    }
}
