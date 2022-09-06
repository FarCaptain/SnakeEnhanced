using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbyssFightingSequence : MonoBehaviour
{
    [SerializeField] private ItemSpawner m_ItemSpawner;
    [SerializeField] private GameObject m_WarningFrame;

    [SerializeField] private GameObject signalLeft;
    [SerializeField] private GameObject signalRight;
    [SerializeField] private GameObject signalUp;
    [SerializeField] private GameObject signalBottom;

    [SerializeField] private int fightCount = 6;

    private int speed;
    private List<IEnumerator> funcs = new List<IEnumerator>();

    private void Start()
    {
        funcs.Add(SpawnLeftAbyss());
        funcs.Add(SpawnRightAbyss());
        funcs.Add(SpawnTopAbyss());
        funcs.Add(SpawnBottomAbyss());

        Invoke("StartSequence", 2f);
    }

    // Hard Code 48 x 27
    IEnumerator SpawnLeftAbyss()
    {
        Debug.Log("Abyss!");

        m_WarningFrame.SetActive(true);
        signalLeft.SetActive(true);
        AudioManager.instance.Play("Alarm");

        yield return new WaitForSeconds(5.0f);
        signalLeft.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        m_ItemSpawner.SpawnAbyss(new Vector2Int(0, 0), new Vector2Int(0, 26), new Vector2Int(speed, 0));

        yield return StartCoroutine(StopAbyss());
    }

    IEnumerator SpawnRightAbyss()
    {
        Debug.Log("Abyss!");

        m_WarningFrame.SetActive(true);
        signalRight.SetActive(true);
        AudioManager.instance.Play("Alarm");


        yield return new WaitForSeconds(5.0f);
        signalRight.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        m_ItemSpawner.SpawnAbyss(new Vector2Int(47, 0), new Vector2Int(47, 26), new Vector2Int(-speed, 0));

        yield return StartCoroutine(StopAbyss());
    }

    IEnumerator SpawnTopAbyss()
    {
        Debug.Log("Abyss!");

        signalUp.SetActive(true);
        m_WarningFrame.SetActive(true);
        AudioManager.instance.Play("Alarm");

        yield return new WaitForSeconds(5.0f);
        signalUp.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        m_ItemSpawner.SpawnAbyss(new Vector2Int(0, 26), new Vector2Int(47, 26), new Vector2Int(0, -speed));

        yield return StartCoroutine(StopAbyss());
    }

    IEnumerator SpawnBottomAbyss()
    {
        Debug.Log("Abyss!");
        m_WarningFrame.SetActive(true);
        signalBottom.SetActive(true);
        AudioManager.instance.Play("Alarm");

        yield return new WaitForSeconds(5.0f);
        signalBottom.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        m_ItemSpawner.SpawnAbyss(new Vector2Int(0, 0), new Vector2Int(47, 0), new Vector2Int(0, speed));

        yield return StartCoroutine(StopAbyss());
    }

    IEnumerator StopAbyss()
    {
        yield return new WaitForSeconds(10f);
        Debug.Log("StopAbyss!");

        m_WarningFrame.SetActive(false);
        signalUp.SetActive(false);
        signalBottom.SetActive(false);
        signalLeft.SetActive(false);
        signalRight.SetActive(false);

        m_ItemSpawner.RemoveAbyss();

        int index = Random.Range(0, funcs.Count);
        float waitTime = Random.Range(5.0f, 15.0f);
        yield return new WaitForSeconds(waitTime);
        yield return StartCoroutine(funcs[index]);
    }

    private void StartSequence()
    {
        StartCoroutine(AbyssFight());
    }

    IEnumerator AbyssFight()
    {
        speed = 1;

        int index = Random.Range(0, funcs.Count);
        float waitTime = Random.Range(20.0f, 25.0f);

        yield return StartCoroutine(funcs[index]);
    }


}
