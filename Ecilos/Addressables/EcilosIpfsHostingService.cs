using System.Collections.Generic;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEditor.AddressableAssets.HostingServices;
using UnityEngine;

public class EcilosIpfsHostingService : HttpHostingService {
  private class IPFSAddResponse {
    public string Name;
    public string Hash;
    public long Size;
  }
  private const string IPFS_BASE_HOST = "localhost";
  private const string IPFS_BASE_URL = "http://" + IPFS_BASE_HOST + ":5001/";
  private const string IPFS_GATEWAY_URL = IPFS_BASE_URL + "/ipfs/";

  // Implement class constructor.
  public EcilosIpfsHostingService() {
    DescriptiveName = "IPFS HTTP Hosting Service";
  }

  private string GetIpfsHashForDirectory(string path) {
    var processStartInfo = new System.Diagnostics.ProcessStartInfo(
        "ipfs", $"add -r -Q {path}") { CreateNoWindow = true, UseShellExecute = false, RedirectStandardOutput = true };
    var process = System.Diagnostics.Process.Start(processStartInfo);
    process.WaitForExit();
    var hash = process.StandardOutput.ReadToEnd().Trim();
    return hash;
  }

  public override void StartHostingService() {
    // m_ContentRoot = $"ipfs://{GetIpfsHashForDirectory(contentRoot)}";
    // m_IsRunning = true;
  }

  public override void StopHostingService() {
    // m_IsRunning = false;
  }

  /*
  public async Task<string> UploadAssetBundleAsync(string assetBundlePath) {
    try {
      // Read the asset bundle file data
      byte[] assetBundleData = File.ReadAllBytes(assetBundlePath);

      // Compute the IPFS hash of the asset bundle data
      //string ipfsHash = await AddDataToIPFSAsync(assetBundleData);

      // Return the IPFS URL of the asset bundle
      //return $"{IPFS_GATEWAY_URL}{ipfsHash}";
    } catch (Exception ex) {
      Debug.LogError($"Failed to upload asset bundle to IPFS: {ex}");
      return null;
    }
  }
  */
}
