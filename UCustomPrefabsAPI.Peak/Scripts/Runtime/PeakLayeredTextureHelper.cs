using UnityEngine;
using System;
using System.Collections.Generic;
[Serializable]
[RequireComponent(typeof(Renderer))]
public class PeakLayeredTextureHelper : MonoBehaviour
{
    //WIP CLASS IGNORE PLZ//
    //Data For Runtime
    private new Renderer renderer;
    private Material material;
    public Vector2Int TextureSize = new Vector2Int(512, 512);
    [HideInInspector]
    public List<Texture2D> LayerBundleTextures;
    [HideInInspector]
    public List<Texture2D> LayerBundleMasks;
    [HideInInspector]
    public string LayerBundleData;
}