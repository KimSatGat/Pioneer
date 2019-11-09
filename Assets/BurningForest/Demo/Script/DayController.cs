using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayController : MonoBehaviour {


   public enum SkyState
    {
        dawn,
        noon,
        evening,
        night

    }

    public SkyState CurrentSkyState = SkyState.dawn;

	// Use this for initialization
	void Start () {



       // SkyCount = SkySprite.Length;

        StartCoroutine(DayChange());
	}

    public SpriteRenderer[] CurrentSky;
    public SpriteRenderer[] NextSky;

    public Sprite[] SkySpriteGroup;

    //public int SkyCount;
    public int SkyCurrentCount=2;

    public float CurrentSkyAlpha = 1;
    //public float FadeOutTime = 2;

    public Slider m_DaySpeed_slider;

    IEnumerator DayChange()
    {
        while (true)
        {

            if (CurrentSkyAlpha > 0)
            {
                CurrentSkyAlpha -= Time.deltaTime* m_DaySpeed_slider.value;

                for (int i = 0; i < CurrentSky.Length; i++)
                {
                    CurrentSky[i].color = new Color(1, 1, 1, CurrentSkyAlpha);
                   
                }
            }
            else
            {
                CurrentSkyAlpha = 1;

                if (SkyCurrentCount > SkySpriteGroup.Length - 1)
                {
                    SkyCurrentCount = 1;
                }
                else
                {
                    SkyCurrentCount++;
                }


                for (int i = 0; i < CurrentSky.Length; i++)
                {
                    CurrentSky[i].sprite = NextSky[i].sprite;
                    CurrentSky[i].color = new Color(1, 1, 1, CurrentSkyAlpha);
                    NextSky[i].sprite = SkySpriteGroup[SkyCurrentCount-1];
                }



            }



       



            yield return new WaitForSeconds(0.01f);
        }

    }





}
