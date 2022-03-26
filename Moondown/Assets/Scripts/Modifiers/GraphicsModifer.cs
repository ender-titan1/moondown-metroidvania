using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Moondown.Player.Modifiers 
{
    public abstract class GraphicsModifer : AbstractModifier
    {
        public override void setup()
        {
            base.setup();
            Player.Instance.OnApplyLowHealth += () => OnApplyLowHealth();
            Player.Instance.OnClearVignette += () => OnClearEffects();
        }
        public abstract void OnApplyLowHealth();
        public abstract void OnClearEffects();
    } 
}
