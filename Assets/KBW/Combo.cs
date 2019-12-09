using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Combo : MonoBehaviour
{
    private static Combo _instance;
    public static Combo instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Combo>();
            }
            return _instance;
        }
    }


    public TextMeshProUGUI comboText;
    int combo = 0;

    float comboStartTime;
    float comboLifeTime = 2f;

    Color originColor;
    float alpha;
    
    void Start()
    {
        originColor = comboText.color;        
        alpha = 0f;
    }
    
    void Update()
    {
        if (Time.time - comboStartTime > 1f)
        {
            comboText.color = new Color(
                originColor.r,
                originColor.g,
                originColor.b,
                alpha
                );
            alpha -= Time.deltaTime;

            if(Time.time - comboStartTime > comboLifeTime)
            {
                combo = 0;
            }
        }        
    }

    public void SetCombo()
    {
        comboText.color = originColor;
        alpha = 1f;

        // 콤보 생성 시간 측정
        comboStartTime = Time.time;
            
        // 콤보 텍스트 갱신
        if(combo < 10)
        {
            comboText.SetText("COMBO\n" + "0" + combo.ToString());
        }
        else
        {
            comboText.SetText("COMBO\n" + combo.ToString());
        }

        // Shake
        StartCoroutine(Shake(0.3f, 2.5f));

        
    }

    public void ComboPlus()
    {
        combo++;
    }
    

    private IEnumerator Shake(float duration, float magnitude)
    {        
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float z = Random.Range(-1f, 1f) * magnitude;

            transform.eulerAngles = new Vector3(0f, 0f, z);
            
            elapsed += Time.deltaTime;

            yield return null;
        }
        transform.eulerAngles = new Vector3(0f, 0f, 0f);
    }
}
