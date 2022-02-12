using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Scripts
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Transform body;
        [SerializeField] private Transform cameraTarget;

        [Header("Death and jump sounds")]
        [SerializeField] private AudioClip deathMeow;
        [SerializeField] private AudioClip oldManJump;

        private Vector2 fingerDownPosition;
        private Vector2 fingerUpPosition;
        
        [Header("How long must be swipe to register")]
        [SerializeField] private float minDistanceForSwipe = 300f;
        
        private enum SwipeDirection
        {
            None,
            Left,
            Right
        }
        private SwipeDirection whereToMove;
        
        private bool touchedMenu;
        private bool gameStarted;
        private Vector3 bodyPosition;
        private bool jumping;
        private Vector3 newPosition;
        private AudioSource audioSource;

        public UnityEvent onStepUp;
        public UnityEvent onDeath;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            bodyPosition = new Vector3(0, 0, -1);
        }

        private void Update()
        {
            if (!gameStarted)
            {
                foreach (Touch touch in Input.touches)
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        touchedMenu = true;
                    }
                }
            }
            
            if (gameStarted && !jumping)
            {
                var sequence = DOTween.Sequence();

                foreach (Touch touch in Input.touches)
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        fingerUpPosition = touch.position;
                        fingerDownPosition = touch.position;
                    }
                    
                    if (touch.phase == TouchPhase.Ended && !touchedMenu)
                    {
                        fingerDownPosition = touch.position;

                        whereToMove = DetectSwipe();
                        
                        if (whereToMove == SwipeDirection.None)
                        {
                            audioSource.PlayOneShot(oldManJump);

                            bodyPosition = body.position;
                            var cameraTargetPosition = cameraTarget.position;

                            newPosition = new Vector3(bodyPosition.x, bodyPosition.y + 1, bodyPosition.z + 1);
                            var newCameraTargetPosition = new Vector3(cameraTargetPosition.x, cameraTargetPosition.y + 1,
                                cameraTargetPosition.z + 1);

                            sequence.Append(body.transform.DOJump(
                                    endValue: newPosition,
                                    jumpPower: 1,
                                    numJumps: 1,
                                    duration: 0.2f
                                ).SetEase(Ease.InOutSine)
                                .OnStart(() =>
                                {
                                    cameraTarget.DOMove(newCameraTargetPosition, 0.2f);
                                    onStepUp.Invoke();
                                    jumping = true;
                                })
                                .OnComplete(() => { jumping = false; }));
                        }else if (whereToMove == SwipeDirection.Left)
                        {
                            audioSource.PlayOneShot(oldManJump);

                            bodyPosition = body.position;

                            newPosition = new Vector3(bodyPosition.x - 0.85f, bodyPosition.y, bodyPosition.z);

                            sequence.Append(body.transform.DOJump(
                                    endValue: newPosition,
                                    jumpPower: 1,
                                    numJumps: 1,
                                    duration: 0.1f
                                ).SetEase(Ease.InOutSine)
                                .OnStart(() => { jumping = true; })
                                .OnComplete(() => { jumping = false; }));
                        }else if (whereToMove == SwipeDirection.Right)
                        {
                            audioSource.PlayOneShot(oldManJump);

                            bodyPosition = body.position;

                            newPosition = new Vector3(bodyPosition.x + 0.85f, bodyPosition.y, bodyPosition.z);

                            sequence.Append(body.transform.DOJump(
                                    endValue: newPosition,
                                    jumpPower: 1,
                                    numJumps: 1,
                                    duration: 0.1f
                                ).SetEase(Ease.InOutSine)
                                .OnStart(() => { jumping = true; })
                                .OnComplete(() => { jumping = false; }));
                        }
                    }
                    else
                    {
                        touchedMenu = false;
                    }
                }
            }
        }
        
        private SwipeDirection DetectSwipe()
        {
            if (Mathf.Abs(fingerDownPosition.x - fingerUpPosition.x) > minDistanceForSwipe)
            {
                var direction = fingerDownPosition.x - fingerUpPosition.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;

                fingerUpPosition = fingerDownPosition;
                
                return direction;
            }
            
            return SwipeDirection.None;
        }
        
        private void ResetPosition()
        {
            body.position = new Vector3(0, bodyPosition.y, bodyPosition.z);
        }
        
        
        //ParalyzeField does that onTriggerEnter
        public void GetParalyzed()
        {
            gameStarted = false;
        }
        
        //Enemies and DeathField does it onTriggerEnter
        public void Die(bool enemy)
        {
            if (jumping)
            {
                bodyPosition = new Vector3(0, newPosition.y, newPosition.z);
            }
            
            onDeath.Invoke();
            audioSource.PlayOneShot(deathMeow);
            gameStarted = false;
        }
        
        //For UnityEvents
        
        //GameManager > OnGameStarted
        
        public void GameStarted()
        {
            ResetPosition();
            gameStarted = true;
        }


    }
}
