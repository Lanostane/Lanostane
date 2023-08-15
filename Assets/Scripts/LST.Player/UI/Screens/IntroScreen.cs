using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LST.Player.UI
{
    public sealed class IntroScreen : BaseScreen
    {
        public override ScreenType Type => ScreenType.Intro;
        protected override bool AutoActiveObject => true;
        protected override bool AutoDeactiveObject => true;

        protected override void OnScreenEnabled()
        {

        }

        protected override void OnScreenDisabled()
        {
            
        }
    }
}
