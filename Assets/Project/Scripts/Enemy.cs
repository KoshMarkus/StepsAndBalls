using DG.Tweening;
using UnityEngine;

namespace Project.Scripts
{
    public class Enemy : MonoBehaviour
    {
        private float moveProgress;
        private float moveThreshold;
        private float minMoveThreshold;

        private void Start()
        {
            moveProgress = 0;
        }

        private void Update()
        {
            moveProgress += Time.deltaTime;

            if (moveProgress >= moveThreshold)
            {
                MoveDown();
                moveProgress = 0;
            }
        }

        private void MoveDown()
        {
            var bodyPosition = transform.position;
            var newBodyPosition = new Vector3(bodyPosition.x, bodyPosition.y, bodyPosition.z - 1);

            transform.DOMove(newBodyPosition, 0.2f).OnComplete(() =>
            {
                transform.DOMove(new Vector3(newBodyPosition.x, newBodyPosition.y - 1, newBodyPosition.z), 0.2f);
            });
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.transform.parent.GetComponent<Player>().Die(true);
            }
        }

        //EnemyManager sets immediatly after initiatie
        public void SetMoveThreshold(float minThreshold, float threshold)
        {
            minMoveThreshold = minThreshold;
            moveThreshold = threshold >= minMoveThreshold ? threshold : minMoveThreshold;
        }
        
        public void Die()
        {
            Destroy(gameObject);
        }
    }
}
