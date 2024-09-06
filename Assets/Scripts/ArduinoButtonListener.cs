using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ArduinoButtonListener : MonoBehaviour
{
	public string serverUrl = "http://192.168.1.66:5005/update";  // Middleware server URL

	void Start()
	{
		StartCoroutine(CheckButtonStatus());
	}

	IEnumerator CheckButtonStatus()
	{
		while (true)
		{
			// Prepare POST request with empty body or the required data
			UnityWebRequest request = new UnityWebRequest(serverUrl, "POST");
			request.SetRequestHeader("Content-Type", "application/json");
            
			// Optionally, send a body with the request
			string jsonData = "{\"buttonPressed\": false}"; // Example JSON data
			byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
			request.uploadHandler = new UploadHandlerRaw(bodyRaw);
			request.downloadHandler = new DownloadHandlerBuffer();
            
			yield return request.SendWebRequest();

			if (request.result == UnityWebRequest.Result.Success)
			{
				string response = request.downloadHandler.text;
				Debug.Log("Server response: " + response);
				
				if (response.Contains("true"))
				{
					// Update game variable or trigger action
					Debug.Log("Button was pressed!");
				}
				else if (response.Contains("false"))
				{
					// Update game variable or trigger action
					Debug.Log("Button was released!");
				}
			}
			else
			{
				Debug.LogError("Error checking button status: " + request.error);
			}

			yield return new WaitForSeconds(1); // Check every second
		}
	}
}