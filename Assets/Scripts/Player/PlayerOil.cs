using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOil : MonoBehaviour
{
    public float duration = 4;
    public bool oilModeActive = false;
    public Slider oilSlider;

    private PlayerDig playerDig;
    public Animator animator;
    private void Start()
    {

        playerDig = GetComponent<PlayerDig>();
    }
    public void OpenOilMode()
    {
        playerDig.ChangeDigTime(playerDig.fastDigTime);
    }
    public void CloseOilMode()
    {
        playerDig.ChangeDigTime(playerDig.normalDigTime);
        animator.Play("Idle");
    }
    public IEnumerator OilAction()
    {
        OpenOilMode();
        oilModeActive = true;
        StartCoroutine(UpdateSlider());
        yield return new WaitForSeconds(duration);
        oilModeActive = false;
        CloseOilMode();
    }
    private IEnumerator UpdateSlider()
    {
        oilSlider.maxValue = duration;
        oilSlider.value = duration;
        while (oilModeActive)
        {
            yield return new WaitForSeconds(0.1f);
            oilSlider.value = oilSlider.value - 0.1f;
        }
    }
}
