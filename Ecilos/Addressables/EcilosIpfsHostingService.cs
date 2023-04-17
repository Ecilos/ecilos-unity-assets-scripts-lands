using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System;
using UnityEditor.AddressableAssets;                  // For: AddressableAssetSettingsDefaultObject.
using UnityEditor.AddressableAssets.HostingServices;  // For: BaseHostingService.
using UnityEditor.AddressableAssets.Settings;         // For: KeyDataStore.
using UnityEditor;                                    // For: EditorGUILayout.
using UnityEngine;

public class EcilosIpfsHostingService : BaseHostingService {
  /* Properties */
  private class IPFSAddResponse {
    public string Name;
    public string Hash;
    public long Size;
  }
  // internal List<FileUploadOperation> m_ActiveUploads = new List<FileUploadOperation>();
  internal const string KIpfsHostnameKey = "IpfsHostname";
  private const string IPFS_BASE_HOST = "localhost";
  private const string IPFS_BASE_URL = "http://" + IPFS_BASE_HOST + ":5001/";
  private const string IPFS_GATEWAY_URL = IPFS_BASE_URL + "/ipfs/";
  readonly List<string> m_ContentRoots;
  readonly Dictionary<string, string> m_ProfileVariables;

  /// <inheritdoc/>
  public override List<string> HostingServiceContentRoots {
    get { return m_ContentRoots; }
  }

  /* GUI fields */
  string m_IpfsHostname;
  const string k_IpfsHostname = "IpfsHostname";
  GUIContent m_IpfsHostnameGUI = new GUIContent(k_IpfsHostname, "Ipfs hostname");

  /// <inheritdoc/>
  public override Dictionary<string, string> ProfileVariables {
    get {
      m_ProfileVariables[k_IpfsHostname] = IpfsHostname;
      return m_ProfileVariables;
    }
  }

  /// <summary>
  /// IPFS Hostname.
  /// </summary>
  public string IpfsHostname {
    get { return m_IpfsHostname; }
  protected
    set {
      if (value != "") {
        m_IpfsHostname = value;
      }
    }
  }

  /* Class methods */

  // Implements class constructor.
  public EcilosIpfsHostingService() {
    m_ProfileVariables = new Dictionary<string, string>();
    m_ContentRoots = new List<string>();
  }

  private string GetIpfsHashForDirectory(string path) {
    var processStartInfo = new System.Diagnostics.ProcessStartInfo(
        "ipfs", $"add -r -Q {path}") { CreateNoWindow = true, UseShellExecute = false, RedirectStandardOutput = true };
    var process = System.Diagnostics.Process.Start(processStartInfo);
    process.WaitForExit();
    var hash = process.StandardOutput.ReadToEnd().Trim();
    return hash;
  }

  /// <inheritdoc/>
  public override bool IsHostingServiceRunning {
    // @todo: Check if the service is up-and-running.
    get {
      return true;
    }
  }

  /// <inheritdoc/>
  public override void OnAfterDeserialize(KeyDataStore dataStore) {
    IpfsHostname = dataStore.GetData(k_IpfsHostname, "");
    // HostingServicePort = dataStore.GetData(k_HostingServicePortKey, 0);
    // UploadSpeed = dataStore.GetData(k_UploadSpeedKey, 0);
    base.OnAfterDeserialize(dataStore);
  }

  /// <inheritdoc/>
  public override void OnBeforeSerialize(KeyDataStore dataStore) {
    dataStore.SetData(k_IpfsHostname, IpfsHostname);
    // dataStore.SetData(k_UploadSpeedKey, m_UploadSpeed);
    base.OnBeforeSerialize(dataStore);
  }

  /// <inheritdoc/>
  public override void OnGUI() {
    EditorGUILayout.BeginHorizontal();
    {
      IpfsHostname = EditorGUILayout.DelayedTextField(m_IpfsHostnameGUI, IpfsHostname);
      var settings = AddressableAssetSettingsDefaultObject.Settings;
      if (settings != null) {
        settings.SetDirty(AddressableAssetSettings.ModificationEvent.HostingServicesManagerModified, this, false, true);
      }
    }
    EditorGUILayout.EndHorizontal();
  }

  public override void StartHostingService() {
    // m_ContentRoot = $"ipfs://{GetIpfsHashForDirectory(contentRoot)}";
    // m_IsRunning = true;

    if (HostingServiceContentRoots.Count == 0) {
      throw new Exception(
          "ContentRoot is not configured; cannot start service. This can usually be fixed by modifying the BuildPath for any new groups and/or building content.");
    }
  }

  public override void StopHostingService() {
    if (!IsHostingServiceRunning)
      return;
    Log("Stopping");
    // m_ActiveUploads.Clear();
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
