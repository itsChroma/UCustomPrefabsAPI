using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScottyFoxArt.PixelTools.ShaderTextures
{
    public class LayerConverter : JsonConverter<Layer>
    {
        public override void WriteJson(JsonWriter writer, Layer value, JsonSerializer serializer)
        {
            JObject obj = new JObject
                {
            { "name", value.name },
            { "enabled", value.enabled },
            { "mode", JToken.FromObject(value.mode) },
            { "color", ColorUtility.ToHtmlStringRGBA(value.color)}
                };
            obj.WriteTo(writer);
        }
        public override Layer ReadJson(JsonReader reader, Type objectType, Layer existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);
            //Color
            var colorToken = obj["color"]?.ToString();
            Debug.LogWarning(colorToken);
            if (!ColorUtility.TryParseHtmlString("#" + colorToken, out Color color))
                color = Color.white;
            //
            return new Layer
            {
                name = obj["name"]?.ToString(),
                enabled = obj["enabled"]?.ToObject<bool>() ?? false,
                mode = obj["mode"]?.ToObject<BlendMode>() ?? BlendMode.None,
                color = color
            };
        }
    }
    [JsonConverter(typeof(LayerConverter))]
    [Serializable]
    public struct Layer
    {
        public string name;
        public bool enabled;
        public BlendMode mode;
        public Color color;
        public Texture2D texture;
        public Texture2D mask;
        public Layer(string name = "Layer", bool enabled = true, BlendMode mode = BlendMode.Overwrite, Color color = default, Texture2D texture = null, Texture2D mask = null)
        {
            this.name = name;
            this.enabled = enabled;
            this.mode = mode;
            this.color = color;
            this.texture = texture;
            this.mask = mask;
        }
    }
    public class LayerBundleConverter : JsonConverter<LayerBundle>
    {
        public override void WriteJson(JsonWriter writer, LayerBundle value, JsonSerializer serializer)
        {
            JObject obj = new JObject
                {
            { "backgroundColor", ColorUtility.ToHtmlStringRGBA(value.backgroundColor)},
            { "layers", JArray.FromObject(value.layers, serializer) }
                };
            obj.WriteTo(writer);
        }
        public override LayerBundle ReadJson(JsonReader reader, Type objectType, LayerBundle existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);
            //Color
            var colorToken = obj["color"]?.ToString();
            if (!ColorUtility.TryParseHtmlString(colorToken, out Color color))
                color = Color.white;
            //
            JArray layersArray = (JArray)obj["layers"];
            return new LayerBundle
            {
                backgroundColor = color,
                layers = layersArray?.ToObject<List<Layer>>(serializer) ?? new List<Layer>()
            };
        }
    }
    [JsonConverter(typeof(LayerBundleConverter))]
    [Serializable]
    public partial class LayerBundle
    {
        public Color backgroundColor = Color.white;
        public List<Layer> layers = new List<Layer>();
        public void ReapplyTextures(List<Texture2D> textures, List<Texture2D> masks)
        {
            if (textures.Count != layers.Count || masks.Count != layers.Count)
            {
                Debug.LogWarning("Reapply Textures has invalid textures/or/masks count. may throw error.");
            }
            for (int i = 0; i < layers.Count; i++)
            {
                try
                {
                    var layer = layers[i];
                    layer.texture = textures[i];
                    layer.mask = masks[i];
                    layers[i] = layer;
                }
                catch
                {
                    Debug.LogError($"Error Applying Texture To Layer {i}");
                }
            }
        }
        public int AddLayer(string name, Texture2D texture, Texture2D mask, Color color, BlendMode mode = BlendMode.Overwrite, bool enabled = true, int index = int.MaxValue)
        {
            Layer newLayer = new Layer(name, enabled, mode, color, texture, mask);
            AddLayer(newLayer, index);
            return index;
        }
        public int AddLayer(Layer layer, int index = int.MaxValue)
        {
            index = GetNearestValidIndex(index);
            if (index == layers.Count)
                layers.Add(layer);
            else
                layers.Insert(index, layer);
            return index;
        }
        public void AddLayers(IEnumerable<Layer> layers)
        {
            foreach (var layer in layers)
            {
                AddLayer(layer);
            }
        }
        public bool RemoveLayer(int index)
        {
            if (!IsValidIndex(index))
                return false;
            layers.RemoveAt(index);
            return true;
        }
        public bool RemoveLayer(string name)
        {
            return RemoveLayer(FindName(name));
        }
        public bool SetLayer(int index, Layer layer)
        {
            if (!IsValidIndex(index))
                return false;
            layers[index] = layer;
            return true;
        }
        public bool SetLayer(string name, Layer layer)
        {
            return SetLayer(FindName(name), layer);
        }
        public int FindName(string name)
        {
            int index = 0;
            foreach (var layer in layers)
            {
                if (layer.name == name)
                    break;
                index++;
            }
            return (index == layers.Count) ? -1 : index;
        }
        public bool TryGetLayer(int index, out Layer layer)
        {
            layer = default(Layer);
            if (!IsValidIndex(index))
                return false;
            layer = layers[index];
            return true;
        }
        public bool TryGetLayer(string name, out Layer layer)
        {
            return TryGetLayer(FindName(name), out layer);
        }
        public bool IsValidIndex(int index)
        {
            return index >= 0 && index < layers.Count;
        }
        public int GetNearestValidIndex(int index)
        {
            if (index > layers.Count)
                index = layers.Count;
            else if (index < 0)
                index = 0;
            return index;
        }
    }
    public partial class LayerBundle
    {
        //
        public void SetLayerEnabled(int index, bool enabled)
        {
            if (!TryGetLayer(index, out var layer))
                return;
            layer.enabled = enabled;
            SetLayer(index, layer);
        }
        public void SetLayerEnabled(string name, bool enabled)
        {
            SetLayerEnabled(FindName(name), enabled);
        }
        //
        public void SetLayerMode(int index, BlendMode mode)
        {
            if (!TryGetLayer(index, out var layer))
                return;
            layer.mode = mode;
            SetLayer(index, layer);
        }
        public void SetLayerMode(string name, BlendMode mode)
        {
            SetLayerMode(FindName(name), mode);
        }
        //
        public void SetLayerTexture(int index, Texture2D texture)
        {
            if (!TryGetLayer(index, out var layer))
                return;
            layer.texture = texture;
            SetLayer(index, layer);
        }
        public void SetLayerTexture(string name, Texture2D texture)
        {
            SetLayerTexture(FindName(name), texture);
        }
        //
        public void SetLayerMask(int index, Texture2D mask)
        {
            if (!TryGetLayer(index, out var layer))
                return;
            layer.texture = mask;
            SetLayer(index, layer);
        }
        public void SetLayerMask(string name, Texture2D mask)
        {
            SetLayerMask(FindName(name), mask);
        }
        //
        public void SetLayerColor(int index, Color color)
        {
            if (!TryGetLayer(index, out var layer))
                return;
            layer.color = color;
            SetLayer(index, layer);
        }
        public void SetLayerColor(string name, Color color)
        {
            SetLayerColor(FindName(name), color);
        }
        //
    }

}
