using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private PolygonCollider2D mapCollider;
    private void Start()
    {
        mapCollider = GetComponent<PolygonCollider2D>();
    }
    public PolygonCollider2D GetMapCollider()
    {
        return mapCollider;
    }
}
