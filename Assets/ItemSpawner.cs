using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public Vector2Int m_ButtomLeft;
    public Vector2Int m_TopRight;

    [SerializeField] private List<Item> m_FruitPrefabs;
    [SerializeField] private Item m_AbyssPrefab;
    [SerializeField] private int m_MaxFruitAmount = 6;
    private MapLocator m_MapLocator;

    private List<Item> m_ObjectPool = new List<Item>();


    private void Start()
    {
        m_MapLocator = MapLocator.instance;

        InvokeRepeating("SpawnFruit", 5.0f, 4.5f);
    }

    private Item GetItem(Item itemPrefab)
    {
        foreach (var item in m_ObjectPool)
        {
            if (item.m_Type != itemPrefab.m_Type)
                continue;
            if (item.gameObject.activeSelf == false)
            {
                item.gameObject.SetActive(true);
                return item;
            }
        }

        Item newItem = Instantiate(itemPrefab);
        m_ObjectPool.Add(newItem);

        return newItem;
    }

    private int GetActiveFruitCount()
    {
        int cnt = 0;
        foreach (var item in m_ObjectPool)
        {
            if (item.isActiveAndEnabled && item.m_Type != CollectableType.ABYSS)
                cnt++;
        }
        return cnt;
    }

    public void SpawnFruit()
    {
        int cnt = GetActiveFruitCount();
        if (cnt >= m_MaxFruitAmount)
            return;

        int index = Random.Range(0, m_FruitPrefabs.Count);
        Item item = GetItem(m_FruitPrefabs[index]);

        // get position
        item.m_CoordProperty = m_MapLocator.GetRandomAvailiblePosition();

        m_MapLocator.AddItem(item.m_CoordProperty, item);
    }

    public void SpawnAbyss(Vector2Int bottomLeft, Vector2Int topRight, Vector2Int speed = default)
    {
        for (int i = bottomLeft.x; i < topRight.x; i++)
        {
            for (int j = bottomLeft.y; j < topRight.y; j++)
            {
                Vector2Int pos = new Vector2Int(i, j);
                Item old = m_MapLocator.FindItem(pos);
                if(old != null)
                {
                    old.gameObject.SetActive(false);
                    m_MapLocator.RemoveItem(pos);
                }

                Abyss abyss = (Abyss)GetItem(m_AbyssPrefab);
                abyss.m_MoveSpeed = speed;
                abyss.m_CoordProperty = pos;
                m_MapLocator.AddItem(pos, abyss);
            }
        }
    }

    // Only one Abbyss at a time
    public void RemoveAbyss()
    {
        foreach (var item in m_ObjectPool)
        {
            if(item.m_Type == CollectableType.ABYSS)
            {
                m_MapLocator.RemoveItem(item.m_CoordProperty);
                item.gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnAbyss(m_ButtomLeft, m_TopRight, new Vector2Int(1, 1));
        }
    }

}