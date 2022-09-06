using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Snake : MonoBehaviour
{
    [SerializeField] private List<SnakeBody> m_Bodies;
    [SerializeField] private CameraShake m_CamShake;
    [SerializeField] private GameObject m_EndScreen;
    [SerializeField] private Text m_EndText; 

    private Vector2 m_NextStep;
    private MapLocator m_MapLocator;

    private bool m_StepUpdated = false;
    private bool m_isMoving = true;

    private void Start()
    {
        m_NextStep = Vector2.up;
        InvokeRepeating("MoveForward", 1.2f, 0.25f);
        //m_Bodies[0].m_CoordProperty = new Vector2Int(0, 0);

        m_MapLocator = MapLocator.instance;
    }

    public void PauseMoving()
    {
        m_isMoving = false;
    }

    public void ResumeMoving()
    {
        m_isMoving = true;
    }

    public void MoveForward()
    {
        if (!m_isMoving)
            return;

        Vector2Int headNext= m_Bodies[0].m_CoordProperty + Vector2Int.RoundToInt(m_NextStep);
        if (CheckAbyss(headNext))
            return;
        CheckItems(headNext);

        for (int i = m_Bodies.Count - 1; i > 0; i--)
        {
            m_Bodies[i].m_CoordProperty = m_Bodies[i - 1].m_CoordProperty;

            bool bumpIntoBody = m_Bodies[i].m_CoordProperty == headNext;
            if(bumpIntoBody)
            {
                Debug.Log("GameOver! Bump Into Body!");
                Die("GameOver! Bump Into Body!");
            }

            CheckAbyss(m_Bodies[i].m_CoordProperty);
        }

        // headMoveForward
        m_Bodies[0].m_CoordProperty = headNext;

        m_StepUpdated = false;
    }

    private bool CheckAbyss(Vector2Int coord)
    {
        Item item = m_MapLocator.FindItem(coord);
        if (item == null)
            return false;

        if(item.m_Type == CollectableType.ABYSS)
        {
            Debug.Log("GameOver! Bumped into Abyss!");
            Die("GameOver! Bumped into Abyss!");
            return true;
        }
        return false;
    }

    private void CheckItems(Vector2Int headCoord)
    {
        Item item = m_MapLocator.FindItem(headCoord);
        if (item == null)
            return;

        GameObject head = m_Bodies[0].gameObject;

        switch (item.m_Type)
        {
            case CollectableType.EXPAND:
                GameObject tail = Instantiate(head, head.transform.parent);
                m_Bodies.Add(tail.GetComponent<SnakeBody>());
                AudioManager.instance.Play("FruitCollect");
                break;
            case CollectableType.SHRINK:
                AudioManager.instance.Play("BadFruitCollect");
                bool onlyOneBodyLeft = m_Bodies.Count < 2;
                if (onlyOneBodyLeft)
                {
                    Debug.Log("GameOver! Nothing Left!");
                    Die("GameOver! Nothing Left!");
                }
                else
                {
                    SnakeBody tailBody = m_Bodies[m_Bodies.Count - 1];
                    m_Bodies.RemoveAt(m_Bodies.Count - 1);
                    Destroy(tailBody.gameObject);
                }
                break;
        }
        m_MapLocator.RemoveItem(item.m_CoordProperty);
        item.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (m_StepUpdated)
            return;

        //only repond left - right 
        float m_Horizontal= Input.GetAxis("Horizontal");
        float m_Vertical = Input.GetAxis("Vertical");

        if ((m_NextStep == Vector2.up || m_NextStep == Vector2.down) && m_Horizontal != 0.0f)
        {
            m_NextStep = (m_Horizontal > 0f) ? Vector2.right : Vector2.left;
            m_StepUpdated = true;
        }
        else if ((m_NextStep == Vector2.left || m_NextStep == Vector2.right) && m_Vertical != 0.0f)
        {
            m_NextStep = (m_Vertical > 0f) ? Vector2.up : Vector2.down;
            m_StepUpdated = true;
        }
    }

    public void Die(string alarm)
    {
        AudioManager.instance.Play("Die");
        PauseMoving();
        StartCoroutine(m_CamShake.Shake(.15f, .2f));

        m_EndText.text = alarm;
        m_EndScreen.SetActive(true);
    }
}
