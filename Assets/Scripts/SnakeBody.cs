using System;
using UnityEngine;

public class SnakeBody : MonoBehaviour
{
    private Vector2Int m_Coord;

    public Vector2Int m_CoordProperty
    {
        get
        {
            return m_Coord;
        }
        set
        {
            //Vector2Int oldCoord = m_Coord;
            m_Coord = value;
            OnCoordChange();
        }
    }

    private void Start()
    {
        // find its coordinate
        m_CoordProperty = MapLocator.instance.GuessCoordination(transform.position);
    }

    private void OnCoordChange()
    {
        m_Coord.x += MapLocator.instance.m_MapSize.x;
        m_Coord.y += MapLocator.instance.m_MapSize.y;

        m_Coord.x %= MapLocator.instance.m_MapSize.x;
        m_Coord.y %= MapLocator.instance.m_MapSize.y;
        transform.position = MapLocator.instance.GetCoordinatePosition(m_Coord.x, m_Coord.y);

        // update recorded coordinate
        //m_MapLocator.UpdateItem(oldCoord, m_Coord);
    }
}
