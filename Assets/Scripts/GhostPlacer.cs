using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPlacer : MonoBehaviour
{
    public GameObject ghostPrefab;

    public List<Vector3> ghostPositions;

    void Start()
    {
        print(ghostPositions.Count);
        
        for (int i = 0; i < ghostPositions.Count; i++)
        {
            GameObject ghost = Instantiate(ghostPrefab, ghostPositions[i], Quaternion.identity);
        }
    }

    void Update()
    {
        
    }
}
