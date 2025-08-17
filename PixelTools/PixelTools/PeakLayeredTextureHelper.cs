using ScottyFoxArt.PixelTools.ShaderTextures;
using UnityEngine;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

#if UNITY_EDITOR
using UnityEditor;
#endif
[Serializable]
[RequireComponent(typeof(Renderer))]
public class PeakLayeredTextureHelper : MonoBehaviour
{
    public Vector2Int TextureSize = new Vector2Int(512, 512);
    public List<Texture2D> LayerBundleTextures;
    public List<Texture2D> LayerBundleMasks;
    public string LayerBundleData;
    public LayerBundle layerbundle = new LayerBundle();
    public void Start()
    {
        Deserialize();
    }
    public void Deserialize()
    {
        if (string.IsNullOrWhiteSpace(LayerBundleData))
            return;
        try
        {
            layerbundle = JsonConvert.DeserializeObject<LayerBundle>(LayerBundleData);
            layerbundle.ReapplyTextures(LayerBundleTextures, LayerBundleMasks);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
    public Texture2D RenderTextureBundle()
    {
        return ShaderTextureBuilder.RenderBundle(layerbundle, TextureSize.x, TextureSize.y);
    }
}