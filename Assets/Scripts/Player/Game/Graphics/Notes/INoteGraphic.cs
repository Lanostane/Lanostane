﻿using Lanostane.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LST.Player.Graphics
{
    public interface INoteGraphic
    {
        LST_NoteSpecialFlags Flags { get; }
        void Show();
        void Hide();
        void DestroyInstance();
    }
}
