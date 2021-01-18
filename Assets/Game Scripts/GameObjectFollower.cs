using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class GameObjectFollower : MonoBehaviour
{
    [SerializeField] private Transform targetFollow = null;
    [SerializeField] private Vector3 offset = Vector3.zero;
    [SerializeField] private float stopDistance = 0f;
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] [Slider(0f, 10f)] private float delaySeconds = 0f;
    [SerializeField] private bool isConstantMove = false;

    [Space]
    [InfoBox("Turn this to true if want simulationesly follow the target path movement.", type: InfoBoxType.Normal)]
    [SerializeField] private bool isPathFollow = false;

    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private float m_distanceBetween;

    #region Unity BuiltIn Methods
    private void OnEnable()
    {
        transform.position = targetFollow.position;
    }

    private void Update()
    {
        if (isPathFollow)
            FollowPath();
        else
            FollowObject();
    }
    #endregion

    private void FollowPath()
    {

    }

    private void FollowObject()
    {
        Vector3 m_targetPosition = targetFollow.position + offset;
        m_distanceBetween = Vector3.Distance(transform.position, targetFollow.position + offset);
        if (m_distanceBetween > stopDistance)
        {
            Vector3 m_moveDir = (m_targetPosition - transform.position).normalized;
            Vector3 m_afterMove;
            if (isConstantMove)
                m_afterMove = transform.position + m_moveDir * followSpeed;
            else
                m_afterMove = transform.position + m_moveDir * m_distanceBetween * followSpeed * Time.deltaTime;

            float m_distanceAfterMove = Vector3.Distance(m_afterMove, m_targetPosition);
            if (m_distanceAfterMove >= m_distanceBetween)
                m_afterMove = m_targetPosition;

            transform.position = m_afterMove;
        }
    }
}
