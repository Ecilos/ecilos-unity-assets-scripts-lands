using System.Collections.Generic;
using System.Collections;
using System.Linq; // For: Enumerable.First.
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations; // For: IResourceLocation.
using UnityEngine.ResourceManagement.ResourceProviders; // For: IAssetBundleResource.
using UnityEngine.AddressableAssets.ResourceLocators; // For: IResourceLocator.
using UnityEngine;

public class EcilosSpawnObjectAddressables : MonoBehaviour
{
  bool isLoaded = false;
  [SerializeField] private AssetLabelReference assetLabelReference;
  [SerializeField] public string remoteCatalogLoadPath;

  // Start is called before the first frame update
  public IEnumerator Start()
  {
    // Load a catalog and automatically release the operation handle.
    IEnumerable<IResourceLocator> locators = Addressables.ResourceLocators;
    if (locators.Any())
    {
      IResourceLocator locator = locators.FirstOrDefault();
      ResourceLocatorInfo locatorInfo = Addressables.GetLocatorInfo(locator);
      string catalogUrl = locatorInfo.CatalogLocation + "/test-catalog.json";
      Debug.Log(catalogUrl);
      AsyncOperationHandle<IResourceLocator> handle = Addressables.LoadContentCatalogAsync(catalogUrl, true);
      yield return handle;
    }
    else
    {
      Debug.Log("No resource locators found in Addressables settings.");
    }
  }

  // Update is called once per frame
  void Update()
  {
    if (!isLoaded)
    {
      AsyncOperationHandle<GameObject> asyncOpHandle = Addressables.LoadAssetAsync<GameObject>(assetLabelReference);
      asyncOpHandle.Completed += AsyncOpObjectLoaded;
      isLoaded = true;
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
    if (location.ResourceType == typeof(IAssetBundleResource) && location.InternalId.StartsWith("http"))
    {
      // @todo: We can here change the ipfs endpoint.
      //return location.InternalId + "?customQueryTag=customQueryValue";
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

  /*
  private string m_SourceFolder = "dataFiles";
  public void AddFileLocatorToAddressables()
  {
    // This example code creates ResourceLocationBase and adds it to the locator for each file.
    // https://docs.unity3d.com/Packages/com.unity.addressables@1.21/api/UnityEngine.AddressableAssets.Addressables.AddResourceLocator.html
    if (!Directory.Exists(m_SourceFolder)) {
      return;
    }

    ResourceLocationMap locator = new ResourceLocationMap(m_SourceFolder + "_FilesLocator", 12);
    string providerId = typeof(TextDataProvider).ToString();

    string[] files = Directory.GetFiles(m_SourceFolder);
    foreach (string filePath in files)
    {
      if (!filePath.EndsWith(".json"))
        continue;
      string keyForLoading = Path.GetFileNameWithoutExtension(filePath);
      locator.Add(keyForLoading, new ResourceLocationBase(keyForLoading, filePath, providerId, typeof(string)));
    }
    Addressables.AddResourceLocator(locator);
  }
  */

  /*
  private string m_DataFileName = "settings";
  public IEnumerator LoadDataUsingAddedLocator()
  {
    // Using Addressables API to load "dataFiles/settings.json" after adding the locator.
    // https://docs.unity3d.com/Packages/com.unity.addressables@1.21/api/UnityEngine.AddressableAssets.Addressables.AddResourceLocator.html
    var loadingHandle = Addressables.LoadAssetAsync<string>(m_DataFileName);
    yield return loadingHandle;
    Debug.Log("Load completed " + loadingHandle.Status + (loadingHandle.Status == AsyncOperationStatus.Succeeded ? ", with result " + loadingHandle.Result : ""));
  }
  */
}
