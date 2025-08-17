using System.Reflection;
using UnityEngine;
namespace ScottyFoxArt.PixelTools.ShaderTextures
{
    public enum BlendMode
    {
        None,
        Burn,
        Darken,
        Difference,
        Dodge,
        Divide,
        Exclusion,
        HardLight,
        HardMix,
        Lighten,
        LinearBurn,
        LinearDodge,
        LinearLight,
        LinearLightAddSub,
        Multiply,
        Negation,
        Overlay,
        PinLight,
        Screen,
        SoftLight,
        Subtract,
        VividLight,
        Overwrite,
        UVDraw
    }
    public static class BlendModeShaderLibrary
    {
        public static Shader Burn;
        public static Shader Darken;
        public static Shader Difference;
        public static Shader Dodge;
        public static Shader Divide;
        public static Shader Exclusion;
        public static Shader HardLight;
        public static Shader HardMix;
        public static Shader Lighten;
        public static Shader LinearBurn;
        public static Shader LinearDodge;
        public static Shader LinearLight;
        public static Shader LinearLightAddSub;
        public static Shader Multiply;
        public static Shader Negation;
        public static Shader Overlay;
        public static Shader PinLight;
        public static Shader Screen;
        public static Shader SoftLight;
        public static Shader Subtract;
        public static Shader VividLight;
        public static Shader Overwrite;
        public static Shader UVDraw;
        //RegistryStuff
        private const string PixelTools_ShaderBundle_File = "PixelTools/pixeltoolsshaders";
        private static bool _loaded = false;
        private static AssetBundle _shaderBundle;
        public static AssetBundle LoadEmbeddedAssetBundleFromCurrentAssembly(string resourcePath)
        {
            var assembly = typeof(BlendModeShaderLibrary).Assembly;
            string fullResourceName = resourcePath.Replace("\\", "/").Replace("/", ".");
            string embeddedpath = assembly.FullName.Split(',')[0] + "." + fullResourceName;
            try
            {
                using (var stream = assembly.GetManifestResourceStream(embeddedpath))
                {
                    if (stream == null)
                    {
                        Debug.LogError("Failed to open stream for embedded AssetBundle.");
                        return null;
                    }
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);
                    return AssetBundle.LoadFromMemory(buffer);
                }
            }
            catch (System.Exception e) {
                Debug.LogError(e);
            }
            return null;
        }
        private static Shader FetchShaderFromRegistry(string shaderName)
        {
            return _shaderBundle.LoadAsset<Shader>(shaderName);
        }
        public static Shader GetBlendMode(BlendMode blendmode)
        {
            if (!_loaded)
            {
                try
                {
                    _shaderBundle = LoadEmbeddedAssetBundleFromCurrentAssembly(PixelTools_ShaderBundle_File);
                    Burn = FetchShaderFromRegistry("LayerBurn");
                    Darken = FetchShaderFromRegistry("LayerDarken");
                    Difference = FetchShaderFromRegistry("LayerDifference");
                    Dodge = FetchShaderFromRegistry("LayerDodge");
                    Divide = FetchShaderFromRegistry("LayerDivide");
                    Exclusion = FetchShaderFromRegistry("LayerExclusion");
                    HardLight = FetchShaderFromRegistry("LayerHardLight");
                    HardMix = FetchShaderFromRegistry("LayerHardMix");
                    Lighten = FetchShaderFromRegistry("LayerLighten");
                    LinearBurn = FetchShaderFromRegistry("LayerLinearBurn");
                    LinearDodge = FetchShaderFromRegistry("LayerLinearDodge");
                    LinearLight = FetchShaderFromRegistry("LayerLinearLight");
                    LinearLightAddSub = FetchShaderFromRegistry("LayerLinearLightAddSub");
                    Multiply = FetchShaderFromRegistry("LayerMultiply");
                    Negation = FetchShaderFromRegistry("LayerNegation");
                    Overlay = FetchShaderFromRegistry("LayerOverlay");
                    PinLight = FetchShaderFromRegistry("LayerPinLight");
                    Screen = FetchShaderFromRegistry("LayerScreen");
                    SoftLight = FetchShaderFromRegistry("LayerSoftLight");
                    Subtract = FetchShaderFromRegistry("LayerSubtract");
                    VividLight = FetchShaderFromRegistry("LayerVividLight");
                    Overwrite = FetchShaderFromRegistry("LayerOverride");
                    UVDraw = FetchShaderFromRegistry("LayerUVDraw");
                    _loaded = true;
                }
                catch { Debug.LogWarning("Failed To Load PixelTool Shaders...!"); }
            }
            if (_loaded)
                switch (blendmode)
                {
                    case BlendMode.Burn:
                        return Burn;
                    case BlendMode.Darken:
                        return Darken;
                    case BlendMode.Difference:
                        return Difference;
                    case BlendMode.Dodge:
                        return Dodge;
                    case BlendMode.Divide:
                        return Divide;
                    case BlendMode.Exclusion:
                        return Exclusion;
                    case BlendMode.HardLight:
                        return HardLight;
                    case BlendMode.HardMix:
                        return HardMix;
                    case BlendMode.Lighten:
                        return Lighten;
                    case BlendMode.LinearBurn:
                        return LinearBurn;
                    case BlendMode.LinearDodge:
                        return LinearDodge;
                    case BlendMode.LinearLight:
                        return LinearLight;
                    case BlendMode.LinearLightAddSub:
                        return LinearLightAddSub;
                    case BlendMode.Multiply:
                        return Multiply;
                    case BlendMode.Negation:
                        return Negation;
                    case BlendMode.Overlay:
                        return Overlay;
                    case BlendMode.PinLight:
                        return PinLight;
                    case BlendMode.Screen:
                        return Screen;
                    case BlendMode.SoftLight:
                        return SoftLight;
                    case BlendMode.Subtract:
                        return Subtract;
                    case BlendMode.VividLight:
                        return VividLight;
                    case BlendMode.Overwrite:
                        return Overwrite;
                    case BlendMode.UVDraw:
                        return UVDraw;
                }
            return null;
        }
    }
}
