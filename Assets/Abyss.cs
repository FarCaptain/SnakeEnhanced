using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abyss : Item
{
    public Vector2Int m_MoveSpeed;

    private void Start()
    {
        if(m_MoveSpeed != Vector2Int.zero)
            InvokeRepeating("Move", 0.1f, 0.2f);
    }

    private void Move()
    {
        var oldCoord = m_CoordProperty;
        m_CoordProperty += m_MoveSpeed;
        MapLocator.instance.RemoveItem(m_CoordProperty);
        MapLocator.instance.UpdateItem(oldCoord, m_CoordProperty, this);
    }
}
