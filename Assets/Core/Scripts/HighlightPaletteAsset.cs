using System.Collections.Generic;
using UnityEngine;

namespace TilemapGridNavigation
{
    [CreateAssetMenu(fileName = "HighlightPalette", menuName = "TilemapGridNav/HighlightPalette")]
    public class HighlightPaletteAsset : ScriptableObject
    {
        public List<HighlightPreset> presets = new();
        
        public Color GetColour(string name)
        {
            HighlightPreset preset = presets.Find(preset => preset.name == name);
            return preset.colour;
        }
    }

    [System.Serializable]
    public struct HighlightPreset
    {
        public string name;
        public Color colour;
    }
}
