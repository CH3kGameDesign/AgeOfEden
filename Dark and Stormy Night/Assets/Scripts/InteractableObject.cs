using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [Header ("Colours")]
    public Color defaultColor;
    public Color hoverOverColor;
    public Color clickColor;

    [Header ("Variables")]
    public int interactionAnimation;

    [Header("GameObjects")]
    public GameObject grabObject;

    public bool inUse = false;

    private Transform m_tTransCache;
    private Transform m_tPlayerTrans;
    private Transform m_tSecondChild;

    private SpriteRenderer m_srSpriteRenderer;
    private Movement m_mPlayerMovement;

    private void Start()
    {
        m_tTransCache = transform;
        m_tPlayerTrans = Movement.s_goPlayerObject.transform;
        m_tSecondChild = transform.parent.GetChild(1);
        m_srSpriteRenderer = GetComponent<SpriteRenderer>();
        m_mPlayerMovement = Movement.s_goPlayerObject.GetComponent<Movement>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Vector3.Distance(m_tTransCache.position, m_tPlayerTrans.position) < 2)
        {
            if (!inUse)
            {
                m_srSpriteRenderer.color = Color.Lerp(
                    m_srSpriteRenderer.color, hoverOverColor, Time.deltaTime);
                m_tTransCache.localScale = Vector3.Lerp(
                    m_tTransCache.localScale, new Vector3(0.1f, 0.1f, 0.1f), Time.deltaTime);

                if (Input.GetMouseButtonDown(0))
                {
                    inUse = true;
                    GetComponent<SpriteRenderer>().color = clickColor;
                    m_mPlayerMovement.m_aModelAnimator.SetInteger("Interaction", 1);
                }
            }
        }
        else
        {
            m_srSpriteRenderer.color = Color.Lerp(
                m_srSpriteRenderer.color, defaultColor, Time.deltaTime);
            m_tTransCache.localScale = Vector3.Lerp(
                m_tTransCache.localScale, Vector3.zero, Time.deltaTime);
        }
        
        if (inUse)
        {
            m_tTransCache.localScale = Vector3.Lerp(
                m_tTransCache.localScale, Vector3.zero, Time.deltaTime * 2);

            Movement.s_bCanMove = false;
            m_tPlayerTrans.position = Vector3.Lerp(
                m_tPlayerTrans.position, m_tSecondChild.position,
                Time.deltaTime);

            CameraMovement.s_CanMove = false;
            CameraMovement.s_CameraObject.transform.LookAt(grabObject.transform);
        }
    }
}