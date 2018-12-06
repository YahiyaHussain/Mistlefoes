using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HolidaySpiritBar : MonoBehaviour {
    
    public int amountHolidaySupplies;
    private Slider suppliesBar;
    private Button[] buttons;

    private Button BombButton;
	// Use this for initialization
	void Start () {
        suppliesBar = transform.GetComponentInChildren<Slider>();
        buttons = transform.GetComponentsInChildren<Button>();
        foreach (Button b in buttons)
        {
            if (b.tag == "BombButton")
            {
                BombButton = b;
            }
        }
	}

    public void BombButtonPress()
    {
        amountHolidaySupplies -= 400;
    }
    private void Update()
    {
        suppliesBar.value = amountHolidaySupplies / 1000.0f;
        if (amountHolidaySupplies < 250 && BombButton.interactable )
        {
            BombButton.interactable = false;
        }
        else if(amountHolidaySupplies > 250 && !BombButton.interactable)
        {
            BombButton.interactable = true;
        }
    }
    public void addSupplies(int shipmentSize)
    {
        if (suppliesBar.value < 1)
        {
            amountHolidaySupplies += shipmentSize;
        }
    }
    public int howMuchSupplies()
    {
        return amountHolidaySupplies;
    }
    public void removeSupplies(int shipmentSize)
    {
        if (suppliesBar.value != 0)
        {
            amountHolidaySupplies -= shipmentSize;
        }
    }
    private enum buttonType
    {
        Bomb
    }

}
