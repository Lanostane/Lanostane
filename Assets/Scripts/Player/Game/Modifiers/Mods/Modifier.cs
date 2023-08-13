using LST.Player.Modifiers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LST.Player.Game.Modifiers
{
    public abstract class Modifier
    {
        public abstract GameModes Mode { get; }
    }
}
