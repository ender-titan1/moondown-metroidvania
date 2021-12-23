using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Moondown.Graphics.Animation
{
    public class PostProcessingAnimationController : MonoBehaviour
    {
        private void Awake()
        {
            Animator animator = GameObject.FindGameObjectWithTag("LowHealthVignette").GetComponent<Animator>();
            
            
        }
    }
}
