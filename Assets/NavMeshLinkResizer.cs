using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

[ExecuteInEditMode]
public class NavMeshLinkResizer : MonoBehaviour
{

    private NavMeshLink[] m_Links;
    private Collider thisCollider;

    [ContextMenu("Generate Sizes For Links")]
    public void GenerateSize()
    {
        thisCollider = GetComponent<Collider>();
        m_Links = gameObject.GetComponents<NavMeshLink>();
        foreach(NavMeshLink link in m_Links)
        {
            link.width = thisCollider.bounds.size.x;
        }
        NavMeshLink xLink = m_Links[0];
        NavMeshLink zLink = m_Links[1];

        xLink.startPoint = new Vector3(thisCollider.GetComponent<BoxCollider>().center.x, 0f, -3f);
        xLink.endPoint = new Vector3(thisCollider.GetComponent<BoxCollider>().center.x, 0f, 3f);

        zLink.startPoint = new Vector3(3f, 0f, thisCollider.GetComponent<BoxCollider>().center.z);
        zLink.endPoint = new Vector3(-3f, 0f, thisCollider.GetComponent<BoxCollider>().center.z);

        xLink.width = thisCollider.bounds.size.x;
        //Debug.Log(xLink.width);
        zLink.width = thisCollider.bounds.size.z;

        float minWidth = Mathf.Min(zLink.width, xLink.width);
        if (minWidth == xLink.width)
        {
            xLink.enabled = false;
            zLink.enabled = true;
        }else
        {
            zLink.enabled = false;
            xLink.enabled = true;
        }
    }
}
