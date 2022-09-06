using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abyss : Item
{
    public Vector2Int m_MoveSpeed;

    private void OnEnable()
    {
        if(m_MoveSpeed != Vector2Int.zero)
            InvokeRepeating("Move", 0.1f, 0.3f);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Move()
    {
        var oldCoord = m_CoordProperty;
        m_CoordProperty += m_MoveSpeed;
        MapLocator.instance.RemoveItem(m_CoordProperty);
        MapLocator.instance.UpdateItem(oldCoord, m_CoordProperty, this);
    }
}
