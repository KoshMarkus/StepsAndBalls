using UnityEngine;

namespace Project.Scripts
{
    public class ParalyzeField : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.transform.parent.GetComponent<Player>().GetParalyzed();
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
