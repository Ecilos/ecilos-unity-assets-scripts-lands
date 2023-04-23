using System.Collections.Generic;
using System.IO;
using System.Linq;
using System; // For: Exception.
using UnityEditor.AddressableAssets.Build.BuildPipelineTasks;
using UnityEditor.AddressableAssets.Settings.GroupSchemas; // For: BundledAssetGroupSchema.
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Build.Pipeline.Utilities; // For: HashingMethods.
using UnityEngine.AddressableAssets.ResourceLocators; // For: ResourceLocationData.
using UnityEngine.Build.Pipeline; // For: BundleDetails.
using UnityEngine.ResourceManagement.ResourceProviders; // For: ProviderLoadRequestOptions.
using UnityEngine;

namespace UnityEditor.AddressableAssets.Build.DataBuilders
{
  using Debug = UnityEngine.Debug;
  public class EcilosBuildScriptIpfsMode : BuildScriptPackedMode
  {

    /// <inheritdoc />
    public override string Name
    {
      get { return "Ecilos Build Script (IPFS)"; }
    }

    /// Creates a name for an asset bundle using the provided information.
    /// <inheritdoc />
    protected override string ConstructAssetBundleName(AddressableAssetGroup assetGroup, BundledAssetGroupSchema schema, BundleDetails info, string assetBundleName)
    {
      if (assetGroup != null)
      {
        string groupName = assetGroup.Name.Replace(" ", "").Replace('\\', '/').Replace("//", "/").ToLower();
        assetBundleName = groupName + "_" + assetBundleName;
      }

      string bundleNameWithHashing = BuildUtility.GetNameWithHashNaming(schema.BundleNaming, info.Hash.ToString(), assetBundleName);
      //For no hash, we need the hash temporarily for content update purposes.  This will be stripped later on.
      if (schema.BundleNaming == BundledAssetGroupSchema.BundleNamingStyle.NoHash)
      {
        bundleNameWithHashing = bundleNameWithHashing.Replace(".bundle", "_" + info.Hash.ToString() + ".bundle");
      }

      bundleNameWithHashing = bundleNameWithHashing.Replace(".bundle", ".ipfs.bundle");
      Debug.Log("Bundle name: " + bundleNameWithHashing);
      return bundleNameWithHashing;

    }

    /// The method that does the actual building after all the groups have been processed.
    /// <inheritdoc />
    protected override TResult DoBuild<TResult>(AddressablesDataBuilderInput builderInput, AddressableAssetsBuildContext aaContext)
    {
      var genericResult = base.DoBuild<TResult>(builderInput, aaContext);
      /*
      foreach (var location in aaContext.locations)
      {
        string dataEntryId = location.InternalId;
        string dataEntryFilename = Path.GetFileName(dataEntryId);
        Debug.Log(dataEntryId + "; " + dataEntryFilename);
      }
      */
      return genericResult;
    }

    /// The processing of the bundled asset schema.
    /// <inheritdoc />
    protected override string ProcessBundledAssetSchema(
        BundledAssetGroupSchema schema,
        AddressableAssetGroup assetGroup,
        AddressableAssetsBuildContext aaContext)
    {
      string res = string.Empty;
      res = base.ProcessBundledAssetSchema(schema, assetGroup, aaContext);
      Debug.Log("Res: " + res);
      return res;
    }

    /// Called per group per schema to evaluate that schema.
    /// <inheritdoc />
    protected override string ProcessGroupSchema(AddressableAssetGroupSchema schema, AddressableAssetGroup assetGroup, AddressableAssetsBuildContext aaContext)
    {
      return base.ProcessGroupSchema(schema, assetGroup, aaContext);
    }

  }

}
