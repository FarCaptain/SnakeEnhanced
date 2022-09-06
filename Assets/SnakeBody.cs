using System;
using UnityEngine;

public class SnakeBody : MonoBehaviour
{
    private Vector2Int m_Coord;
    private MapLocator m_MapLocator;

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

    private void Awake()
    {
        m_MapLocator = MapLocator.instance;

        // find its coordinate
        m_CoordProperty = m_MapLocator.GuessCoordination(transform.position);
    }

    private void OnCoordChange()
    {
        m_Coord.x += m_MapLocator.m_MapSize.x;
        m_Coord.y += m_MapLocator.m_MapSize.y;

        m_Coord.x %= m_MapLocator.m_MapSize.x;
        m_Coord.y %= m_MapLocator.m_MapSize.y;
        transform.position = m_MapLocator.GetCoordinatePosition(m_Coord.x, m_Coord.y);

        // update recorded coordinate
        //m_MapLocator.UpdateItem(oldCoord, m_Coord);
    }
}
