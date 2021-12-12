using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnLocation : MonoBehaviour
{
    public enum RespawnMode
    {
        HIT,
        DEATH
    }

    public Vector2 position;
    public RespawnMode mode;
    public int cost;

    private void OnEnable()
    {
        position = gameObject.transform.position;
    }
}
