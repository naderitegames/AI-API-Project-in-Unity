using System;
using System.Collections;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GeminiImageQuery : MonoBehaviour
{
    enum ImageSource
    {
        LocalImage,
        URLImage
    }

    enum GeminiVisionModel
    {
        //Gemini10ProVision,
        Gemini15Pro,
        Gemini20Flash
    }

    [SerializeField] private string apiKey; // Your Google Gemini API key
    [SerializeField] private Texture2D imageToAnalyze; // The image to send
    [SerializeField] private TMP_InputField responseText; // UI Text to display the response
    [SerializeField] private GeminiVisionModel selectedModel = GeminiVisionModel.Gemini20Flash;
    [SerializeField] private ImageSource imageSource = ImageSource.LocalImage;
    [SerializeField] private TextMeshProUGUI imageURL;

    [TextArea] [SerializeField] private string defaultPrompt =
        "فکر کن یک ربات هستی که فقط ورودی میگیره و خیلی سرد جواب میده. هر عکسی که میفرستم فقط دوتا پرامپت دقیق بده تا بتونم عکسای بیشتری با اون پرامپت بسازم.";
    //"imagen you are a robot that answers to requests without anyjust give me 2 seperate prompts that resaults images like this in google gemeni or even other image generation AIs. do not talk abot more details.";

    [SerializeField] private bool useCastomPrompt;

    private void OnEnable()
    {
        SendImageToGemini();
    }

    public void SendImageToGemini()
    {
        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError("Please enter your API key.");
            return;
        }

        switch (imageSource)
        {
            case ImageSource.LocalImage:
                if (imageToAnalyze == null)
                {
                    Debug.LogError("Please assign an image to analyze.");
                    return;
                }

                StartCoroutine(UploadImageAndGetGeminiResponse(imageToAnalyze));
                break;
            case ImageSource.URLImage:
                if (string.IsNullOrEmpty(imageURL.text))
                {
                    Debug.LogError("Please enter the image URL.");
                    return;
                }

                var imageFromURLAndSendToGemini = GetImageFromURLAndSendToGemini();
                break;
        }
    }

    IEnumerator GetImageFromURLAndSendToGemini()
    {
        using (UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(imageURL.text))
        {
            yield return imageRequest.SendWebRequest();

            if (imageRequest.result == UnityWebRequest.Result.Success)
            {
                Texture2D imageTexture = DownloadHandlerTexture.GetContent(imageRequest);
                StartCoroutine(UploadImageAndGetGeminiResponse(imageTexture));
            }
            else
            {
                Debug.LogError("Error downloading image: " + imageRequest.error);
                responseText.text = "Error downloading image: " + imageRequest.error;
            }
        }
    }

    public string GetModelName()
    {
        switch (selectedModel)
        {
            //case GeminiVisionModel.Gemini10ProVision:
            //return "gemini-1.0-pro-vision";
            case GeminiVisionModel.Gemini15Pro:
                return "gemini-1.5-pro";
            case GeminiVisionModel.Gemini20Flash:
                return "gemini-2.0-flash";
            default:
                return "gemini-1.0-pro-vision"; // Default if something goes wrong
        }
    }

    IEnumerator UploadImageAndGetGeminiResponse(Texture2D targetTexture)
    {
        byte[] imageBytes = targetTexture.EncodeToPNG(); // Or JPG, depending on your needs.
        string base64Image = Convert.ToBase64String(imageBytes);

        string apiUrl =
            $"https://generativelanguage.googleapis.com/v1beta/models/{GetModelName()}:generateContent?key={apiKey}";

        string requestBody = $@"
        {{
          ""contents"": [
            {{
              ""parts"": [
                {{
                  ""inlineData"": {{
                    ""mimeType"": ""image/png"",
                    ""data"": ""{base64Image}""
                  }}
                }},
                {{
                  ""text"": ""{defaultPrompt}""
                }}
              ]
            }}
          ]
        }}";

        using (UnityWebRequest webRequest = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(requestBody);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string responseJson = webRequest.downloadHandler.text;
                Debug.Log("Gemini Response: " + responseJson);

                // Parse the JSON response and extract the text.
                string responseTextContent = ParseGeminiResponse(responseJson);

                if (!string.IsNullOrEmpty(responseTextContent))
                {
                    responseText.text = responseTextContent;
                }
                else
                {
                    responseText.text = "Failed to parse Gemini response.";
                }
            }
            else
            {
                Debug.LogError("Error: " + webRequest.error);
                responseText.text = "Error: " + webRequest.error;
            }
        }
    }

    private string ParseGeminiResponse(string jsonResponse)
    {
        try
        {
            // Simple parsing (requires more robust JSON parsing for production)
            int startIndex = jsonResponse.IndexOf("\"text\": \"") + "\"text\": \"".Length;
            int endIndex = jsonResponse.IndexOf("\"", startIndex);
            string content = jsonResponse.Substring(startIndex, endIndex - startIndex);
            content = content.Replace("\\n", "\n"); // Handle newline characters.
            return content;
        }
        catch (Exception e)
        {
            Debug.LogError("Error parsing JSON: " + e.Message);
            return null;
        }
    }
}