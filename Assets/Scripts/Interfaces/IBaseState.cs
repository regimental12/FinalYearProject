using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public abstract class IBaseState : MonoBehaviour
    {
        public abstract void StateUpdate();

        //void UpdateHealth(int amount);

    }
}
