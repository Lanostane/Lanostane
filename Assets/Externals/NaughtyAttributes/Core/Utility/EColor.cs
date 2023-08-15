using UnityEngine;

namespace NaughtyAttributes
{
    public enum EColor
    {
        Clear,
        White,
        Black,
        Gray,
        Red,
        Pink,
        Orange,
        Yellow,
        Green,
        Blue,
        Indigo,
        Violet
    }

    public static class EColorExtensions
    {
        public static Color GetColor(this EColor color)
        {
            return color switch
            {
                EColor.Clear => (Color)new Color32(0, 0, 0, 0),
                EColor.White => (Color)new Color32(255, 255, 255, 255),
                EColor.Black => (Color)new Color32(0, 0, 0, 255),
                EColor.Gray => (Color)new Color32(128, 128, 128, 255),
                EColor.Red => (Color)new Color32(255, 0, 63, 255),
                EColor.Pink => (Color)new Color32(255, 152, 203, 255),
                EColor.Orange => (Color)new Color32(255, 128, 0, 255),
                EColor.Yellow => (Color)new Color32(255, 211, 0, 255),
                EColor.Green => (Color)new Color32(98, 200, 79, 255),
                EColor.Blue => (Color)new Color32(0, 135, 189, 255),
                EColor.Indigo => (Color)new Color32(75, 0, 130, 255),
                EColor.Violet => (Color)new Color32(128, 0, 255, 255),
                _ => (Color)new Color32(0, 0, 0, 255),
            };
        }
    }
}
