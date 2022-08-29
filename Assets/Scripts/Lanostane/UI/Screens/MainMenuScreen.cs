using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lanostane.UI.Screens
{
    public sealed class MainMenuScreen : BaseScreen
    {
        public override ScreenType Type => ScreenType.MainMenu;
        protected override bool AutoActiveObject => true;
        protected override bool AutoDeactiveObject => true;

        protected override void OnScreenDisabled()
        {
            
        }

        protected override void OnScreenEnabled()
        {
            
        }
    }
}
