using System;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Scripts
{
    public class DeathField : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.transform.parent.GetComponent<Player>().Die(false);
            }

            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<Enemy>().Die();
            }
        }

        //For UnityEvents
        
        //Player > onStepUp
        
        public void Pursue()
        {
            var transformPosition = transform.position;

            transform.position = new Vector3(transformPosition.x, transformPosition.y + 1, transformPosition.z + 1);
        }
    }
}
