using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lst.Loading
{
    public enum LoadingStyles
    {
        BlackShutter
    }

    public struct LoadingStyle
    {
        public LoadingStyles Style;
        public bool HideScreenOnFinished;

        public static readonly LoadingStyle Default = new()
        {
            Style = LoadingStyles.BlackShutter,
            HideScreenOnFinished = true
        };
    }
}
