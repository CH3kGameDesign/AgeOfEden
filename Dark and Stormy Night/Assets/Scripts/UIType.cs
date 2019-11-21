using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIType : MonoBehaviour {

    public bool m_bAdditive = true;
    public bool m_bDeleteAfter = false;
    public bool m_bDeleteFromBack = false;
    public string m_sMessage;
    public float m_fTypeSpeed = 0.1f;
    public TextMeshProUGUI m_tmpTextMeshProUI;
    public GameObject m_goActivateOnFinish;

    private int m_iLetter = 0;
    private float m_fTypeTimer = 0;

	// Use this for initialization
	void Start () {
        if (GetComponent<TextMeshProUGUI>() != null)
            m_tmpTextMeshProUI = GetComponent<TextMeshProUGUI>();
        if (m_bAdditive == false)
            m_tmpTextMeshProUI.text = "";

    }
	
	// Update is called once per frame
	void Update () {
        if (m_fTypeTimer >= m_fTypeSpeed)
        {
            m_tmpTextMeshProUI.text += m_sMessage.Substring(m_iLetter, 1);
            m_iLetter++;
            m_fTypeTimer = 0;
        }
        if (m_iLetter >= m_sMessage.Length)
        {
            if (m_goActivateOnFinish != null)
                m_goActivateOnFinish.SetActive(true);
            GetComponent<UIType>().enabled = false;
        }
        m_fTypeTimer += Time.deltaTime;
	}
}
