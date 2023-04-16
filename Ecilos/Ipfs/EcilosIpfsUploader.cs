using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Ecilos.Ipfs {

  public static class EcilosIpfsUploader {
    [System.Serializable]
    private class IpfsResponse {
      public string Name;
      public string Hash;
      public int Size;
    }
    private const string ipfsApiUrl = "http://localhost:5001/api/v0/add";
    private const string IPFS_BASE_URL = "http://localhost:5001/api/v0";

    public static async Task<string> UploadFile(string filePath) {
      // Read the contents of the file into a byte array
      byte[] fileBytes = File.ReadAllBytes(filePath);

      // Create a new HTTP client
      using (var client = new HttpClient()) {
        // Create a new multipart form data content
        var content = new MultipartFormDataContent();

        // Add the file content to the multipart form data
        content.Add(new ByteArrayContent(fileBytes), "file", Path.GetFileName(filePath));

        // Send the POST request to IPFS to add the file to the network
        var response = await client.PostAsync(IPFS_BASE_URL + "/add", content);

        // Read the response content as a string
        var responseContent = await response.Content.ReadAsStringAsync();

        // Parse the response to get the hash of the uploaded file
        // var responseJson = JSON.Parse(responseContent);
        // var hash = responseJson["Hash"].Value;
        IpfsResponse responseJson = JsonUtility.FromJson<IpfsResponse>(responseContent);
        var hash = responseJson.Hash;

        // Return the hash
        return hash;
      }
    }

    public static void UploadFile2(string filePath, MonoBehaviour monoBehaviour) {
      monoBehaviour.StartCoroutine(UploadFileCoroutine(filePath));
    }

    private static IEnumerator UploadFileCoroutine(string filePath) {
      // Read the file bytes
      byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

      // Create a UnityWebRequest and set its method to POST
      UnityWebRequest request = UnityWebRequest.PostWwwForm(ipfsApiUrl, "POST");

      // Set the request data to the file bytes
      request.uploadHandler = new UploadHandlerRaw(fileBytes);

      // Set the request headers
      request.SetRequestHeader("Content-Type", "application/octet-stream");

      // Send the request and wait for the response
      yield return request.SendWebRequest();

      // Check for errors
      if (request.result == UnityWebRequest.Result.ConnectionError ||
          request.result == UnityWebRequest.Result.ProtocolError) {
        Debug.LogError(request.error);
      } else {
        // Get the IPFS hash from the response
        string responseText = request.downloadHandler.text;
        string ipfsHash = responseText.Split(' ')[1];

        Debug.Log("File uploaded to IPFS. Hash: " + ipfsHash);
      }
    }
  }

}
