using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[Serializable]
public class BotFieldOfView
{
    [SerializeField] private float fovWideInDegree = 45f;
    [SerializeField] private float sightLength = 8f;
    [SerializeField] private int raySample = 5;
    [SerializeField] private Transform fovPoint = null;
    [SerializeField] private LayerMask sightHitLayer = ~0;

    private List<Transform> m_targetCaught = new List<Transform>();
    private float m_rayStartDegree;
    private float m_degreePerRayCount;

    public List<Transform> CaughtOnSight => m_targetCaught;
    public float FarSight => sightLength;
    
    public void HandleSight(Vector3 globalEulerAngle)
    {
        m_targetCaught.Clear();
        m_degreePerRayCount = fovWideInDegree / raySample;
        m_rayStartDegree = 180f + (fovWideInDegree / 2f) + (m_degreePerRayCount / 2f) - globalEulerAngle.y;
        for (int i = 0; i < raySample; i++)
        {
            Vector3 dir = DegreeToPointXZ(m_rayStartDegree - (i * m_degreePerRayCount));
            dir.y -= DegreeToPointYZ(globalEulerAngle.x).y;
            Ray sightRay = new Ray(fovPoint.position, dir);
            RaycastHit hit;
            if (Physics.Raycast(sightRay, out hit, sightLength, sightHitLayer.value))
            {
                if (!m_targetCaught.Contains(hit.transform))
                    m_targetCaught.Add(hit.transform);
                Debug.DrawRay(fovPoint.position, dir * Vector3.Distance(hit.point, fovPoint.position));
            }
            else
            {
                Debug.DrawRay(fovPoint.position, dir * sightLength);
            }
        }
    }

    private static Vector3 DegreeToPointXZ(float degree)
    {
        return new Vector3(Mathf.Sin(Mathf.Deg2Rad * degree), 0f, -Mathf.Cos(Mathf.Deg2Rad * degree));
    }

    private static Vector3 DegreeToPointYZ(float degree)
    {
        return new Vector3(0f, Mathf.Sin(Mathf.Deg2Rad * degree), -Mathf.Cos(Mathf.Deg2Rad * degree));
    }
}
