using System.Collections.Generic;
using System.Collections;
using System.Linq; // For: Enumerable.First.
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.Initialization;
using UnityEngine.AddressableAssets.ResourceLocators; // For: IResourceLocator.
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations; // For: IResourceLocation.
using UnityEngine.ResourceManagement.ResourceProviders; // For: IAssetBundleResource.
using UnityEditor.AddressableAssets.Settings; // For: AddressableAssetSettingsDefaultObject.
using UnityEngine;

public class EcilosSpawnObjectAddressables : MonoBehaviour
{
  bool isLoaded = false;
  const string defaultIpfsHost = "http://localhost:8080/ipfs";
  const string defaultCatalogFilename = "catalog_1.0.json"; // @todo: To remove in favor of dynamic detection.
  [SerializeField] private AssetLabelReference assetLabelReference;
  [SerializeField] public string cidRemoteFolderLoadPath;
  private string remoteIpfsHost = defaultIpfsHost;

  public string GetDefaultCatalogName()
  {
    string versionedFileName = defaultCatalogFilename;
    /* @todo
    AddressableAssetSettings aaSettings = AddressableAssetSettingsDefaultObject.Settings;
    if (aaSettings != null)
    {
      versionedFileName = aaSettings.profileSettings.EvaluateString(aaSettings.activeProfileId, "/catalog_" + builderInput.PlayerVersion);
      Debug.Log("Versioned catalog file name: " + versionedFileName);
    }
    else
    {
      Debug.LogError("AddressableAssetSettings not found.");
    }
    */
    return versionedFileName;
  }

  // Start is called before the first frame update
  public IEnumerator Start()
  {
    if (cidRemoteFolderLoadPath.Length == 0)
    {
      yield return null;
    }
    // Load a catalog and automatically release the operation handle.
    IEnumerable<IResourceLocator> locators = Addressables.ResourceLocators;
    if (locators.Any())
    {
      IResourceLocator locator = locators.FirstOrDefault();
      ResourceLocatorInfo locatorInfo = Addressables.GetLocatorInfo(locator);
      if (locatorInfo.CatalogLocation != null)
      {
        remoteIpfsHost = locatorInfo.CatalogLocation.ToString();
      }
      else
      {
        //remoteIpfsHost = "http://localhost:8080/ipfs";
      }
    }
    else
    {
      Debug.Log("No resource locators found in Addressables settings.");
      //remoteIpfsHost = "http://localhost:8080/ipfs";
    }
    /*
    string defaultCatalogName = AddressablesRuntimeProperties.EvaluateString("{CatalogName}");
    var versionedFileName = aaSettings.profileSettings.EvaluateString(aaSettings.activeProfileId, "/catalog_" + builderInput.PlayerVersion);
    Debug.Log("Default catalog name: " + defaultCatalogName);
    */
    string catalogFilename = GetDefaultCatalogName();
    Debug.Log($"Loading catalog at: {remoteIpfsHost}/{cidRemoteFolderLoadPath}/{catalogFilename}");
    AsyncOperationHandle<IResourceLocator> handle = Addressables.LoadContentCatalogAsync(remoteIpfsHost + "/" + cidRemoteFolderLoadPath + "/" + defaultCatalogFilename, true);
    yield return handle;
  }

  // Update is called once per frame
  void Update()
  {
    if (!isLoaded)
    {
      /*
      AsyncOperationHandle<GameObject> asyncOpHandle = Addressables.LoadAssetAsync<GameObject>(assetLabelReference);
      asyncOpHandle.Completed += AsyncOpObjectLoaded;
      isLoaded = true;
      */
    }
  }

  private void AsyncOpObjectLoaded(AsyncOperationHandle<GameObject> asyncOpHandle)
  {
    if (asyncOpHandle.Status == AsyncOperationStatus.Succeeded)
    {
      Instantiate(asyncOpHandle.Result);
    }
    else
    {
      Debug.Log("Failed to load object!");
    }
  }

  // Implement a method to transform the internal ids of locations.
  static string CustomIpfsTransform(IResourceLocation location)
  {
    if (location.ResourceType == typeof(IAssetBundleResource))
    {
      if (location.InternalId.StartsWith("http"))
      {
      }
      else if (location.InternalId.EndsWith("bundle"))
      {
        //location.InternalId = remoteIpfsHost + '/' + location.InternalId;
        //Debug.Log($"Loading asset: {remoteIpfsHost}/{location.InternalId}");
        //return remoteIpfsHost + '/' + location.InternalId;
        var objInstance = FindObjectOfType<EcilosSpawnObjectAddressables>();
        if (objInstance != null)
        {
          return defaultIpfsHost + "/" + objInstance.cidRemoteFolderLoadPath + "/" + location.InternalId;
        }
      }
      Debug.Log(location);
      Debug.Log(location.InternalId);
      Debug.Log(location.ProviderId);
      // @todo: We can here change the ipfs endpoint.
      //return location.InternalId + "?customQueryTag=customQueryValue";
    }
    else
    {
      Debug.Log(location.ResourceType);
      Debug.Log(location);
      Debug.Log(location.InternalId);
      Debug.Log(location.ProviderId);
    }
    return location.InternalId;
  }

  // Override the Addressables transform method with your custom method.
  // This can be set to null to revert to default behavior.
  [RuntimeInitializeOnLoadMethod]
  static void SetInternalIdTransform()
  {
    Addressables.InternalIdTransformFunc = CustomIpfsTransform;
  }

  // @todo: Needs testing.
  public void AddIpfsLocatorToAddressables()
  {
    // This example code creates ResourceLocationBase and adds it to the locator for each file.
    // https://docs.unity3d.com/Packages/com.unity.addressables@1.21/api/UnityEngine.AddressableAssets.Addressables.AddResourceLocator.html
    ResourceLocationMap locator = new ResourceLocationMap(cidRemoteFolderLoadPath, 12);
    string providerId = typeof(TextDataProvider).ToString();
    locator.Add(cidRemoteFolderLoadPath, new ResourceLocationBase(cidRemoteFolderLoadPath, cidRemoteFolderLoadPath, providerId, typeof(string)));
    Addressables.AddResourceLocator(locator);
  }

  // @todo: Needs testing.
  private string m_DataFileName = "settings";
  public IEnumerator LoadDataUsingAddedLocator()
  {
    // Using Addressables API to load "dataFiles/settings.json" after adding the locator.
    // https://docs.unity3d.com/Packages/com.unity.addressables@1.21/api/UnityEngine.AddressableAssets.Addressables.AddResourceLocator.html
    var loadingHandle = Addressables.LoadAssetAsync<string>(m_DataFileName);
    yield return loadingHandle;
    Debug.Log("Load completed " + loadingHandle.Status + (loadingHandle.Status == AsyncOperationStatus.Succeeded ? ", with result " + loadingHandle.Result : ""));
  }
}
