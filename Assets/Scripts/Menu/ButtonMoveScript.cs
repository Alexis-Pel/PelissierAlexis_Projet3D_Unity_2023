using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class ButtonMove : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform m_transform;

    [SerializeField]
    private Vector3 move_pos;

    private Vector3 init_pos;

    void Start()
    {
        m_transform = GetComponent<RectTransform>();
        init_pos = m_transform.position;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        m_transform.DOKill();
        m_transform.DOMove(init_pos + move_pos, 0.5f);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_transform.DOKill();
        m_transform.DOMove(init_pos, 0.5f);
    }
}
