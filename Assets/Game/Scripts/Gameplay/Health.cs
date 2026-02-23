using System;
using UnityEngine;

namespace SpaceInvaders
{
    public class Health : MonoBehaviour
    {
        public Action OnDamageTaken;
        
        public void TakeDamage()
        {
            OnDamageTaken?.Invoke();
        }
    }
}