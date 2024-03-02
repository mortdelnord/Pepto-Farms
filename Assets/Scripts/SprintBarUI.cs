using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SprintBarUI : MonoBehaviour
{
    public Image sprintBackground;
    public Image sprintBar;
    public float fadeTime;
    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (playerMovement.isSprinting)
        {
            sprintBar.color = new Color(sprintBar.color.r, sprintBar.color.g, sprintBar.color.b, 100f);
            sprintBackground.color = new Color(sprintBackground.color.r, sprintBackground.color.g, sprintBackground.color.b, 100f);
        }
        if(sprintBar.fillAmount >= 0.75f && !playerMovement.isSprinting)
        {
            float alphaValue = Mathf.Lerp(sprintBar.color.a, 0f, Time.deltaTime * fadeTime);
            sprintBar.color = new Color(sprintBar.color.r, sprintBar.color.g, sprintBar.color.b, alphaValue);
            sprintBackground.color = new Color(sprintBackground.color.r, sprintBackground.color.g, sprintBackground.color.b, alphaValue);
            
        }
    }

}
