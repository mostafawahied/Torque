using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class path : MonoBehaviour
{

    public Color linecolor;
    private List<Transform> nodes = new List<Transform>();

    void OnDrawGizmos()
    {
        Gizmos.color = linecolor;
        Transform[] PathTarnsform = GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();
        for (int i = 0; i < PathTarnsform.Length; i++)
        {
            if (PathTarnsform[i] != transform)
            {
                nodes.Add(PathTarnsform[i]);
            }

        }
        for (int i = 0; i < nodes.Count; i++)
        {
            Vector3 currentynode = nodes[i].position;
            Vector3 previusnode = Vector3.zero;
            if (i > 0)
            {
                previusnode = nodes[i - 1].position;
            }
            else if (i == 0 && nodes.Count > 1)
            {
                previusnode = nodes[nodes.Count - 1].position;
            }
            Gizmos.DrawLine(previusnode, currentynode);
        }

    }
}
