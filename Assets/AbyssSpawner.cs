using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbyssSpawner : MonoBehaviour
{
    public Vector2Int m_ButtomLeft;
    public Vector2Int m_TopRight;
    [SerializeField] Item m_AbyssPrefab;
    private MapLocator m_MapLocator;


    const float m_UnitScale = 0.34f;

    private void Start()
    {
        m_MapLocator = MapLocator.instance;
    }

    private void Init()
    {
        Vector3 btnleft = m_MapLocator.GetCoordinatePosition(m_ButtomLeft.x, m_ButtomLeft.y);
        Vector3 topright = m_MapLocator.GetCoordinatePosition(m_TopRight.x, m_TopRight.y);

        Vector3 midpoint = (btnleft + topright) / 2.0f;
        transform.position = midpoint;

        int width = m_TopRight.x - m_ButtomLeft.x + 1;
        int height = m_TopRight.y - m_ButtomLeft.y + 1;

        transform.localScale = new Vector3(width, height, 0f) * m_UnitScale;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Init();
        }
    }
}
