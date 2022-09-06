using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLocator : MonoBehaviour
{
    //mapsize 48 * 27
    public Vector2Int m_MapSize = new Vector2Int(48, 27);
    [SerializeField] Transform m_TopRightAnchor;
    [SerializeField] Transform m_BottomLeftAnchor;

    private Dictionary<Vector2Int, Item> m_Map = new Dictionary<Vector2Int, Item>();

    private float width;
    private float height;
    private float xUnit;
    private float yUnit;

    #region Singleton
    public static MapLocator instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Map found!");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            //DontDestroyOnLoad(this);
        }

        width = m_TopRightAnchor.position.x - m_BottomLeftAnchor.position.x;
        height = m_TopRightAnchor.position.y - m_BottomLeftAnchor.position.y;

        xUnit = width / (m_MapSize.x - 1);
        yUnit = height / (m_MapSize.y - 1);
    }
    #endregion

    /// <summary>
    ///  origin set in bottom left and coordination starts from 0
    /// </summary>
    public Vector3 GetCoordinatePosition(int x, int y)
    {
        return new Vector3(m_BottomLeftAnchor.position.x + 1.0f * x * xUnit,
            m_BottomLeftAnchor.position.y + 1.0f * y * yUnit, 0.0f);
    }

    public Vector2Int GetRandomAvailiblePosition()
    {
        while (true)
        {
            // ToDo. avoid lock
            int x = Random.Range(0, m_MapSize.x);
            int y = Random.Range(0, m_MapSize.y);
            var pos = new Vector2Int(x, y);
            if (!m_Map.ContainsKey(pos))
            {
                return pos;
            }
        }
    }

    public Vector2Int GuessCoordination(Vector3 position)
    {
        float x = position.x - m_BottomLeftAnchor.position.x;
        int xCoord = (int)Mathf.Round(x / xUnit);

        float y = position.y - m_BottomLeftAnchor.position.y;
        int yCoord = (int)Mathf.Round(y / yUnit);

        return new Vector2Int(xCoord, yCoord);
    }

    public void AddItem(Vector2Int coord, Item item)
    {
        m_Map.Add(coord, item);
    }

    public void UpdateItem(Vector2Int oldCoord, Vector2Int newCoord)
    {
        if (m_Map.ContainsKey(oldCoord))
        {
            Item item = m_Map[oldCoord];
            m_Map.Remove(oldCoord);

            m_Map.Add(newCoord, item);
        }
    }

    public void UpdateItem(Vector2Int oldCoord, Vector2Int newCoord, Item item)
    {
        if (m_Map.ContainsKey(oldCoord))
        {
            m_Map.Remove(oldCoord);
        }

        m_Map.Add(newCoord, item);
    }

    public void RemoveItem(Vector2Int coord)
    {
        if (m_Map.ContainsKey(coord))
        {
            m_Map[coord].gameObject.SetActive(false);
            m_Map.Remove(coord);
        }
    }

    public Item FindItem(Vector2Int coord)
    {
        if(m_Map.ContainsKey(coord))
        {
            return m_Map[coord];
        }
        return null;
    }
}
