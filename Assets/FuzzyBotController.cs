using UnityEngine;
using System.Collections;
using System.Linq;

public class FuzzyBotController : BotController2 {

    public override void stateSwitcher()
    {
        float result = 0;
        result = FuzzyLogic.getValues(Health, (Ammo), Range);
        

        if (result <= -2) // Collect Health
        {
            activeState.enabled = false;
            activeState = csCollectHealth;
            activeState.enabled = true;
            
            //Debug.Log("HEALTH " + this.name + " " + result + ": " + Health + ": " + Ammo);
        }
        else if (result < 0 && result > -2) // Collect Ammo
        {
            activeState.enabled = false;
            activeState = caCollectAmmo;
            activeState.enabled = true;
            
            //Debug.Log("AMMO " + this.name + " " + result + ": " + Health + ": " + Ammo);
        }
        else if (result > 0 && result <= 5) // Roam
        {
            activeState.enabled = false;
            activeState = rStateRoam;
            activeState.enabled = true;
           
            //Debug.Log(this.name + " " + result);
        }
        else if (result > 5) // Shoot
        {
            if (goEnGameObject == null)
            {
               return;
            }
            activeState.enabled = false;
            activeState = sShootState;
            activeState.enabled = true;
            //Debug.Log(this.name + " " + result);
        }
        if (Health <= 0) // Death.
        {
            this.gameObject.SetActive(false);
            GameManager2.Instance.UpdateDeaths(this.gameObject);
            Sender.SendMessage("IncreaseKillCount");
        }
    }
}
