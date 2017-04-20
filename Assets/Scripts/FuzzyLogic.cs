using System;
using UnityEngine;
using System.Collections;


public class FuzzyLogic 
{

    static float healthLow = 0;
    static float healthMid = 0;
    static float healthHigh = 0;

    static float ammoLow = 0;
    static float ammoMid = 0;
    static float ammoHigh = 0;

    static float rangeLow = 0;
    static float rangeMid = 0;
    static float rangeHigh = 0;

    // Membership functions.

    #region

    /*
                          x1__
                         /
                    __x0/
        */

    static float FuzzyGrade(float value, float x0, float x1)
    {
        float result = 0;
        float x = value;

        if (x <= x0)
        {
            result = 0;
        }
        else if (x >= x1)
        {
            result = 1;
        }
        else
        {
            result = (x / (x1 - x0)) - (x0 / (x1 - x0));
        }

        return result;
    }

    /*
                __x0
                  \
                   \x1__
    */

    static float FuzzyReverseGrade(float value, float x0, float x1)
    {
        float result = 0;
        float x = value;

        if (x <= x0)
        {
            result = 1;
        }
        else if (x >= x1)
        {
            result = 0;
        }
        else
        {
            result = (-x / (x1 - x0)) + (x1 / (x1 - x0));
        }

        return result;
    }


    /*               x1   
                     /\ 
                __x0/  \x2__
    */

    static float FuzzyTriangle(float value, float x0, float x1, float x2)
    {
        float result = 0;
        float x = value;

        if (x <= x0)
        {
            result = 0;
        }
        else if (x == x1)
        {
            result = 1;
        }
        else if ((x > x0) && (x < x1))
        {
            result = (x / (x1 - x0)) - (x0 / (x1 - x0));
        }
        else
        {
            result = (-x / (x2 - x1)) + (x2 / (x2 - x1));
        }
        return result;
    }

    /*
                  x1___x2
                 /      \
            __x0/        \x3__
    */

    static float FuzzyTrapezoid(float value, float x0, float x1, float x2, float x3)
    {
        float result = 0;
        float x = value;

        if (x <= x0)
        {
            result = 0;
        }
        else if ((x >= x1) && (x <= x2))
        {
            result = 1;
        }
        else if ((x > x0) && (x < x1))
        {
            result = (x / (x1 - x0)) - (x0 / (x1 - x0));
        }
        else
        {
            result = (-x / (x3 - x2)) + (x3 / (x3 - x2));
        }

        return result;
    }

    #endregion

    // Get Values.

    #region

    static void EvalHealth(float value)
    {
        healthLow = FuzzyReverseGrade(value, 0, 20);
        healthMid = FuzzyTriangle(value, 10, 40, 50);
        healthHigh = FuzzyGrade(value, 30, 100);
    }

    static void EvalAmmo(float value)
    {
        ammoLow = FuzzyReverseGrade(value * 10, 0, 20);   // 0,40
        ammoMid = FuzzyTriangle(value * 10, 10, 20, 50);  // 30,40,70
        ammoHigh = FuzzyGrade(value * 10, 30, 100);       // 60,100
    }

    static void EvalRange(float value)
    {
        rangeLow = FuzzyReverseGrade(value, 20, 40);
        rangeMid = FuzzyTriangle(value, 30, 40, 50);
        rangeHigh = FuzzyGrade(value, 40, 50);
    }

    static public float getValues(float health , float ammo , float range)
    {
        EvalHealth(health);
        EvalAmmo(ammo);
        EvalRange(range);

        return Singleton();
    }

    #endregion

    // Fuzzy inference rules.

    #region

    static float CollectHealth()
    {
        float result = 0;

        result = Math.Min(healthLow, Math.Max(1 - ammoLow, 1 - rangeLow));

        return result;
    }

    static float CollectAmmo()
    {
        float result = 0;

        result = Math.Min(ammoLow, Math.Max(1 - healthLow, 1 - rangeLow));

        return result;
    }

    static float Roam()
    {
        float result = 0;

        result = Math.Min(rangeHigh, Math.Max(1 - ammoLow, 1 - healthLow));

        return result;
    }

    static float Shoot()
    {
        float result = 0;

        result = Math.Min(Math.Max(rangeLow, rangeMid), Math.Max(1 - ammoLow, 1 - healthLow));

        return result;
    }

    #endregion

    // Singleton output function.

    #region

    static float Singleton()
    {
        // divide by zero check
        float check = healthLow + healthMid + healthHigh + ammoLow + ammoMid + ammoHigh + rangeLow + rangeMid + rangeHigh;
        if (check == 0)
        {
            return 0;
        }

        float result = 0;

        result = ((CollectHealth() * -10) + (CollectAmmo() * -5) + (Roam() * 1) + (Shoot() * 10)) /
                 (CollectHealth() + CollectAmmo() + Roam() + Shoot());

        return result;
    }

#endregion
}
