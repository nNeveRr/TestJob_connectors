using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField]
    private float radius = 20;
    [SerializeField]
    private int objectsCount = 10;
    [Space]
    [SerializeField]
    private GameObject templateObject;
    [SerializeField]
    private Transform centerTranform;
    [SerializeField]
    private Transform parentTranform;
    [SerializeField]
    private CollissionTemplate checkCollissionTemplate;

    private List<GameObject> activeObjects = new List<GameObject>();

    public static event System.Action OnRecreateAll;

    private void Start()
    {
        RegenerateObjects();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            RegenerateObjects();
        }
    }
    private void RegenerateObjects()
    {
        clearLastObjects();
        int maxCycle = 1000;
        for ( int i = 0; i < objectsCount; i++)
        {
            int antyCycle = 0;
            while (antyCycle < maxCycle)
            {
                float randAngle = Random.Range(0f, 360f);
                Vector3 probePos = Quaternion.Euler(0f, randAngle, 0f) * new Vector3(Random.Range(0f, radius), 0f, 0f) + centerTranform.position;
                if (antiCollisionCheck(probePos))
                {
                    GameObject nO = Instantiate(templateObject, probePos, Quaternion.identity, parentTranform);
                    activeObjects.Add(nO);
                    break;
                }
                antyCycle++;
                if (antyCycle == maxCycle)
                {
                    Debug.LogError("Too low space for placing all objects!");
                }
            }
        }
        OnRecreateAll?.Invoke();
    }

    private bool antiCollisionCheck(Vector3 position)
    {
        int platformMask = 1 << 8;
        var hits = Physics.BoxCastAll(position - new Vector3(0f, -1f,0f), checkCollissionTemplate.transform.localScale,
            Vector3.up, Quaternion.identity, 3f, platformMask);
        return hits.Length == 0;
    }

    private void clearLastObjects()
    {
        activeObjects.ForEach(t => Destroy(t));
        activeObjects.Clear();
    }
}
