using System;
using System.Collections.Generic;
using UnityEngine;
namespace UCustomPrefabsAPI.Peak
{
    public enum PeakAccessoryType
    {
        Accessory,
        Eyes,
        Outfit,
        Hat,
        Mouth,
        Shorts,
        Skirt,
        NoPants, // Unique to Cowboy/Astronaut
        ScoutRank //TODO implement Scout Rank
    }
    static class Peak_InfoStub
    {
        public static readonly string[] Default_Desc = new[] { "[?] Custom" };
        public static readonly string Default_ID = string.Empty;
        public static readonly Dictionary<PeakAccessoryType, string[]> Descriptions =
            new()
            {
                [PeakAccessoryType.Accessory] = new[]
                {
            "[0] None",
            "[1] Charlie",
            "[2] Jagged Hair",
            "[3] Parted Hair",
            "[4] Microbangs",
            "[5] Raised Brow",
            "[6] Brows Dot",
            "[7] Brows Thick",
            "[8] Brows Angry",
            "[9] Glasses Round",
            "[10] Aviator",
            "[11] Cheekbones",
            "[12] Face Paint",
            "[13] Scar",
            "[14] Bow",
            "[15] Eyepatch",
            "[16] Glasses Broken",
            "[17] Groucho",
            "[18] Tiny Sunglasses",
            "[19] Scar 2",
            "[?] Custom"
                },
                [PeakAccessoryType.Eyes] = new[]
                {
            "[0] Basic",
            "[1] Lashes",
            "[2] Half",
            "[3] Shine",
            "[4] Squint",
            "[5] Angry",
            "[6] Eyeliner",
            "[7] Sad",
            "[8] Cry",
            "[9] Tired",
            "[10] Almond",
            "[11] Surprised",
            "[12] Small",
            "[13] Concern",
            "[14] Anime",
            "[15] Real",
            "[16] Aggro",
            "[17] Inverted",
            "[?] Custom"
                },
                [PeakAccessoryType.Outfit] = new[]
                {
            "[0] Seagull (Shorts)",
            "[1] Seagull (Skirt)",
            "[2] Turtle (Shorts)",
            "[3] Turtle (Skirt)",
            "[4] Sailor (Shorts)",
            "[5] Sailor (Skirt)",
            "[6] Castaway (Shorts)",
            "[7] Castaway (Skirt)",
            "[8] Tropical (Shorts)",
            "[9] Tropical (Skirt)",
            "[10] Cookie (Shorts)",
            "[11] Cookie (Skirt)",
            "[12] Balloon (Shorts)",
            "[13] Balloon (Skirt)",
            "[14] Scoutmaster (Shorts)",
            "[15] Scoutmaster (Skirt)",
            "[16] Cowboy",
            "[17] Astronaut",
            "[?] Custom"
                },
                [PeakAccessoryType.Hat] = new[]
                {
            "[0] Cap",
            "[1] Beret",
            "[2] Pith (Fedora)",
            "[3] Propeller",
            "[4] Straw",
            "[5] Aviator",
            "[6] Sailor",
            "[7] Medic",
            "[8] Midsummer",
            "[9] Mushroom",
            "[10] Crab",
            "[11] Courier",
            "[12] Scoutmaster",
            "[13] Bandana",
            "[14] Ninja Headband",
            "[15] Chef Hat",
            "[16] Wizard Hat",
            "[17] Wolf Ears",
            "[18] Crown",
            "[19] Goat",
            "[20] DesertHat",
            "[21] Visor",
            "[22] SunHat",
            "[23] Cowboy",
            "[24] BingBong",
            "[25] RacingHelmet",
            "[26] Astronaut",
            "[?] Custom"
                },
                [PeakAccessoryType.Mouth] = new[]
                {
            "[0] Smile",
            "[1] CalArts",
            "[2] Cat",
            "[3] Cheek",
            "[4] Drool",
            "[5] Sad",
            "[6] Frown",
            "[7] Kissy",
            "[8] Nonplussed",
            "[9] O",
            "[10] Squiggle",
            "[11] Triangle",
            "[12] Smirk",
            "[13] Cringe",
            "[14] Teeth",
            "[15] Vamp",
            "[16] Aggro",
            "[17] Wacky",
            "[18] Labu",
            "[?] Custom"
                },
                [PeakAccessoryType.ScoutRank] = new[]
                {
            "[0] Default",
            "[1] Ascent 1",
            "[2] Ascent 2",
            "[3] Ascent 3",
            "[4] Ascent 4",
            "[5] Ascent 5",
            "[6] Ascent 6",
            "[7] Ascent 7",
            "[8] Ascent 8",
            "[?] Custom"
                }
            };

        public static readonly Dictionary<PeakAccessoryType, string[]> IDs =
            new()
            {
                [PeakAccessoryType.Accessory] = new[]
                {
            "Accessory_None",
            "Accessory_Charlie",
            "Accessory_JaggedHair",
            "Accessory_PartedHair",
            "Accessory_Microbangs",
            "Accessory_RaisedBrow",
            "Accessory_Brows_Dot",
            "Accessory_Brows_Thick",
            "Accessory_Brows_Angry",
            "Accessory_Glasses_Round",
            "Accessory_Aviator",
            "Accessory_Cheekbones",
            "Accessory_Face_Paint",
            "Accessory_Scar",
            "Accessory_Bow",
            "Accessory_Eyepatch",
            "Accessory_Glasses_Broken",
            "Accessory_Groucho",
            "Accessory_Tiny_Sunglasses",
            "Accessory_Scar2",
            "[?] Custom"
                },
                [PeakAccessoryType.Eyes] = new[]
                {
            "Eye_Basic",
            "Eye_Lashes",
            "Eye_Half",
            "Eye_Shine",
            "Eye_Squint",
            "Eye_Angry",
            "Eye_Eyeliner",
            "Eye_Sad",
            "Eye_Cry",
            "Eye_Tired",
            "Eye_Almond",
            "Eye_Surprised",
            "Eye_Small",
            "Eye_Concern",
            "Eye_Anime",
            "Eye_Real",
            "Eye_Aggro",
            "Eye_Inverted",
            "[?] Custom"
                },
                [PeakAccessoryType.Outfit] = new[]
                {
            "Fit_Seagull_Shorts",
            "Fit_Seagull_Skirt",
            "Fit_Turtle_Shorts",
            "Fit_Turtle_Skirt",
            "Fit_Sailor_Shorts",
            "Fit_Sailor_Skirt",
            "Fit_Castaway_Shorts",
            "Fit_Castaway_Skirt",
            "Fit_Tropical_Shorts",
            "Fit_Tropical_Skirt",
            "Fit_Cookie_Shorts",
            "Fit_Cookie_Skirt",
            "Fit_Balloon_Shorts",
            "Fit_Balloon_Skirt",
            "Fit_Scoutmaster_Shorts",
            "Fit_Scoutmaster_Skirt",
            "Fit_Cowboy",
            "Fit_Astronaut",
            "[?] Custom"
                },
                [PeakAccessoryType.Hat] = new[]
                {
            "Hat_Cap",
            "Hat_Beret",
            "Hat_Pith",
            "Hat_Propeller",
            "Hat_Straw",
            "Hat_Aviator",
            "Hat_Sailor",
            "Hat_Medic",
            "Hat_Midsummer",
            "Hat_Mushroom",
            "Hat_Crab",
            "Hat_Courier",
            "Hat_Scoutmaster",
            "Hat_Bandana",
            "Hat_NinjaHeadband",
            "Hat_ChefHat",
            "Hat_WizardHat",
            "Hat_WolfEars",
            "Hat_Crown",
            "Hat_Goat",
            "Hat_DesertHat",
            "Hat_Visor",
            "Hat_SunHat",
            "Hat_Cowboy",
            "Hat_BingBong",
            "Hat_RacingHelmet",
            "Hat_Astronaut",
            "[?] Custom"
                },
                [PeakAccessoryType.Mouth] = new[]
                {
            "Mouth_Smile",
            "Mouth_CalArts",
            "Mouth_Cat",
            "Mouth_Cheek",
            "Mouth_Drool",
            "Mouth_Sad",
            "Mouth_Frown",
            "Mouth_Kissy",
            "Mouth_Nonplussed",
            "Mouth_O",
            "Mouth_Squiggle",
            "Mouth_Triangle",
            "Mouth_Smirk",
            "Mouth_Cringe",
            "Mouth_Teeth",
            "Mouth_Vamp",
            "Mouth_Aggro",
            "Mouth_Wacky",
            "Mouth_Labu",
            "[?] Custom"
                },
                [PeakAccessoryType.ScoutRank] = new[]
                {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "?"
                }
            };
        public static string[] Fetch_Desc(PeakAccessoryType type)
        {
            if (Descriptions.TryGetValue(type, out var desc))
                return desc;
            return Default_Desc;
        }
        public static string Fetch_ID(PeakAccessoryType type, int index)
        {
            if (!IDs.TryGetValue(type, out var id))
                return Default_ID;
            return id[Mathf.Clamp(index, 0, id.Length - 1)];
        }
    }
    [Serializable]
    public struct PeakAccessoryTarget
    {
        public PeakAccessoryType type;
        public string hint;
        //RUNTIME HANDLE//
        public PeakAccessoryTarget(string type, string hint)
        {
            Enum.TryParse(type.TrimStart().TrimEnd(), true, out this.type);
            this.hint = hint;
        }
    }
}
