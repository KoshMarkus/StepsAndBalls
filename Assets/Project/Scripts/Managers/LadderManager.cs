using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Managers
{
    public class LadderManager : MonoBehaviour
    {
        [SerializeField] private Transform ladder;
        
        [SerializeField] private List<Transform> steps;
        [SerializeField] private Transform lowestStep;
        [SerializeField] private Transform highestStep;

        private void Awake()
        {
            foreach (Transform step in ladder)
            {
                steps.Add(step);

                if (steps.Count == 1)
                {
                    lowestStep = step;
                    highestStep = step;
                }
                else
                {
                    highestStep = step;
                }
            }
        }

        //For UnityEvents
        
        //Player > onStepUp
        
        public void MoveSteps()
        {
            var highestPosition = highestStep.position;

            lowestStep.position = new Vector3(highestPosition.x, highestPosition.y + 1, highestPosition.z + 1);

            highestStep = lowestStep;

            if (steps.IndexOf(lowestStep) < steps.Count - 1)
            {
                lowestStep = steps[steps.IndexOf(lowestStep) + 1].transform;
            }
            else if (steps.IndexOf(lowestStep) == steps.Count - 1)
            {
                lowestStep = steps[0];
            }
            
        }
    }
}