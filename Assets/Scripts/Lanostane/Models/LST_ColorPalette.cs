using System;
using System.Drawing;
using ZeroFormatter;
using ID = ZeroFormatter.IndexAttribute;
using PreserveAttribute = UnityEngine.Scripting.PreserveAttribute;

namespace Lanostane.Models
{
    public enum LST_ColorPaletteIndex : byte
    {
        None,
        A, B, C, D, E, F, G, H
    }

    [Preserve]
    [ZeroFormattable]
    public struct LST_ColorPalette
    {
        [ID(0)] public Color A;
        [ID(1)] public Color B;
        [ID(2)] public Color C;
        [ID(3)] public Color D;
        [ID(4)] public Color E;
        [ID(5)] public Color F;
        [ID(6)] public Color G;
        [ID(7)] public Color H;

        public LST_ColorPalette(Color a, Color b, Color c, Color d, Color e, Color f, Color g, Color h)
        {
            A = a;
            B = b;
            C = c;
            D = d;
            E = e;
            F = f;
            G = g;
            H = h;
        }

        public Color GetColor(LST_ColorPaletteIndex index)
        {
            return index switch
            {
                LST_ColorPaletteIndex.A => A,
                LST_ColorPaletteIndex.B => B,
                LST_ColorPaletteIndex.C => C,
                LST_ColorPaletteIndex.D => D,
                LST_ColorPaletteIndex.E => E,
                LST_ColorPaletteIndex.F => F,
                LST_ColorPaletteIndex.G => G,
                LST_ColorPaletteIndex.H => H,
                _ => throw new NotImplementedException(),
            };
        }
    }
}
