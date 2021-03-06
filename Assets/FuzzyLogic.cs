﻿using System;
using UnityEngine;
using System.Collections;


public class FuzzyLogic 
{

    float healthLow = 0;
    float healthMid = 0;
    float healthHigh = 0;

    float ammoLow = 0;
    float ammoMid = 0;
    float ammoHigh = 0;

    float rangeLow = 0;
    float rangeMid = 0;
    float rangeHigh = 0;

    // Membership functions.

    #region

    /*
                          x1__
                         /
                    __x0/
        */

    float FuzzyGrade(float value, float x0, float x1)
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

    float FuzzyReverseGrade(float value, float x0, float x1)
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

    float FuzzyTriangle(float value, float x0, float x1, float x2)
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

    float FuzzyTrapezoid(float value, float x0, float x1, float x2, float x3)
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

    void EvalHealth(float value)
    {
        healthLow = FuzzyReverseGrade(value, 0, 40);
        healthMid = FuzzyTriangle(value, 30, 50, 80);
        healthHigh = FuzzyGrade(value, 50, 100);
    }

    void EvalAmmo(float value)
    {
        ammoLow = FuzzyReverseGrade(value, 0, 40);
        ammoMid = FuzzyTriangle(value, 30, 40, 70);
        ammoHigh = FuzzyGrade(value, 60, 100);
    }

    void EvalRange(float value)
    {
        rangeLow = FuzzyReverseGrade(value, 20, 40);
        rangeMid = FuzzyTriangle(value, 30, 40, 50);
        rangeHigh = FuzzyGrade(value, 40, 50);
    }

    public float getValues(float health , float ammo , float range)
    {
        EvalHealth(health);
        EvalAmmo(ammo);
        EvalRange(range);

        return Singleton();
    }

    #endregion

    // Fuzzy inference rules.

    #region

    float CollectHealth()
    {
        float result = 0;

        result = Math.Min(healthLow, Math.Max(1 - ammoLow, 1 - rangeLow));

        return result;
    }

    float CollectAmmo()
    {
        float result = 0;

        result = Math.Min(ammoLow, Math.Max(1 - healthLow, 1 - rangeLow));

        return result;
    }

    float Roam()
    {
        float result = 0;

        result = Math.Min(rangeHigh, Math.Max(1 - ammoLow, 1 - healthLow));

        return result;
    }

    float Shoot()
    {
        float result = 0;

        result = Math.Min(rangeLow, Math.Max(1 - ammoLow, 1 - healthLow));

        return result;
    }

    #endregion

    // Singleton output function.

    #region

    float Singleton()
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
