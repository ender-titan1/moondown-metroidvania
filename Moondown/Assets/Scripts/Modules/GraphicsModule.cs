using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Moondown.Player.Modules {
    public abstract class GraphicsModule : AbstractModule
    {
        public override void setup()
        {
            base.setup();
            Player.Instance.OnApplyLowHealth += () => OnApplyLowHealth();
            Player.Instance.OnClearVigette += () => OnClearVigette();
        }

        public abstract void OnApplyLowHealth();
        public abstract void OnClearVigette();
    } 
}
