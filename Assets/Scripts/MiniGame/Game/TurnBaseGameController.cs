using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BrainyChef
{
    public class TurnBaseGameController : MonoBehaviour
    {
        internal Action<Turn> OnTurnChanged;

        internal enum Turn
        {
            Player,
            Enemy
        }

        [Header("Setting")]
        [SerializeField]
        Timer initGameTimer;

        [SerializeField]
        Timer gameTimer;

        [SerializeField]
        MiniGameController miniGameController;

        [Header("Entity")]
        [SerializeField]
        PlayerController playerController;

        [SerializeField]
        Status playerHealth;

        [SerializeField]
        Status enemyHealth;

        [Header("UI")]
        [SerializeField]
        RectTransform countdownMenu;

        [SerializeField]
        RectTransform currentTurnSplashMenu;

        [SerializeField]
        RectTransform gameOverMenu;

        [SerializeField]
        Text lblTurn;

        [SerializeField]
        Text lblTurnInSplash;

        [SerializeField]
        CanvasGroup splashCanvasGroup;

        [SerializeField]
        Text lblEnemyDamageReceived;

        [SerializeField]
        Animator lblEnemyDamageReceivedAnim;

        [Header("Player")]
        [SerializeField]
        CanvasGroup actionCanvasGroup;

        [SerializeField]
        Button btnAttack;

        [SerializeField]
        Button btnHeal;

        [SerializeField]
        Button btnUltimate;

        [SerializeField]
        RectTransform panelMeter;

        [SerializeField]
        UISliderBCI sliderAttack;

        [SerializeField]
        UISliderBCI sliderHealth;

        [SerializeField]
        UISliderBCI sliderUltimate;

        bool hasWinner;
        bool isInAttackPhrase;

        internal Turn CurrentTurn => turn;
        Turn turn = Turn.Player;

        void Awake()
        {
            SubscribeEvent();
        }

        void Start()
        {
            lblTurn.text = turn.ToString() + "Turn";
            lblTurnInSplash.text = turn.ToString() + "Turn";
            lblEnemyDamageReceived.text = "";

            btnUltimate.gameObject.SetActive(false);
            countdownMenu.gameObject.SetActive(true);

            initGameTimer.CountDown();
        }

        void Update()
        {
            if (isInAttackPhrase && !gameTimer.IsPause)
            {
                gameTimer.Pause(true);
            }
            else if (!isInAttackPhrase && gameTimer.IsPause)
            {
                gameTimer.Pause(false);
            }

            if (turn == Turn.Enemy)
            {
                EnemyHandler();
            }
        }

        void OnDestroy()
        {
            UnsubscribeEvent();
        }

        void SubscribeEvent()
        {
            initGameTimer.OnStopped += InitGameTimer_OnStopped;
            gameTimer.OnStopped += GameTimer_OnStopped;
            miniGameController.OnGameOver += OnGameOver;

            btnAttack.onClick.AddListener(() =>
            {
                if (isInAttackPhrase || turn == Turn.Enemy)
                    return;

                actionCanvasGroup.interactable = false;

                isInAttackPhrase = true;
                gameTimer.Pause(true);

                //Show attent meter then get value after timeout
                sliderAttack.gameObject.SetActive(true);
                panelMeter.gameObject.SetActive(true);

                //then
                /* playerController.AttackEnemy(); */
                /* Debug.Log("Player attack.."); */
            });

            sliderAttack.OnValueMax += SliderAttack_OnValueMax;
            sliderAttack.OnTimeoutValue += SliderAttack_OnTimeoutValue;

            playerController.OnAttacking += Player_OnAttacking;
            playerController.OnAttackFinished += Player_OnAttackFinished;
        }

        void EnemyHandler()
        {

        }

        void SliderAttack_OnValueMax(float value)
        {
            HidePanelMeter();
            playerController.AttackEnemy();
            Debug.Log("Player attack..");
        }

        void SliderAttack_OnTimeoutValue(float value)
        {
            HidePanelMeter();
            playerController.AttackEnemy(playerController.AttackPoint * 0.5f);
            Debug.Log("Player attack..");
        }

        void HidePanelMeter()
        {
            sliderAttack.gameObject.SetActive(false);
            panelMeter.gameObject.SetActive(false);
        }

        void UnsubscribeEvent()
        {
            initGameTimer.OnStopped -= InitGameTimer_OnStopped;
            gameTimer.OnStopped -= GameTimer_OnStopped;
            playerController.OnAttacking -= Player_OnAttacking;
            playerController.OnAttackFinished -= Player_OnAttackFinished;
            sliderAttack.OnValueMax -= SliderAttack_OnValueMax;
        }

        void InitGameTimer_OnStopped()
        {
            countdownMenu.gameObject.SetActive(false);
            miniGameController.GameStart();
            gameTimer.CountDown();
        }

        void GameTimer_OnStopped()
        {
            NextTurn();
        }

        void Player_OnAttacking(float value)
        {
            bool isMakingLessDamage = value < playerController.AttackPoint;
            lblEnemyDamageReceived.text = (isMakingLessDamage) ? value.ToString() : "Critical : " + value;
            lblEnemyDamageReceivedAnim.SetTrigger("Play");
        }

        void Player_OnAttackFinished()
        {
            isInAttackPhrase = false;
            StartCoroutine(Player_OnAttackFinished_Callback());
        }

        IEnumerator Player_OnAttackFinished_Callback()
        {
            yield return new WaitForSeconds(1.5f);
            lblEnemyDamageReceived.text = "";
            NextTurn();
        }

        void NextTurn()
        {
            hasWinner = playerHealth.IsEmpty | enemyHealth.IsEmpty;

            if (hasWinner)
            {
                miniGameController.GameOver();
                return;
            }

            Turn oldTurn = turn;
            turn = (oldTurn == Turn.Player) ? Turn.Enemy : Turn.Player;

            Debug.Log("Current turn : " + turn.ToString());
            actionCanvasGroup.interactable = (turn == Turn.Player);

            gameTimer.Reset();
            StartCoroutine(NextTurn_Callback(turn));
        }

        IEnumerator NextTurn_Callback(Turn turn)
        {
            lblTurn.text = turn.ToString() + "Turn";
            lblTurnInSplash.text = turn.ToString() + "Turn";

            currentTurnSplashMenu.gameObject.SetActive(true);

            yield return new WaitForSeconds(1.5f);
            currentTurnSplashMenu.gameObject.SetActive(false);

            OnTurnChanged?.Invoke(turn);
            gameTimer.CountDown();
        }

        void OnGameOver()
        {
            gameOverMenu.gameObject.SetActive(true);
        }

        public void SetInAttackPhrase(bool value)
        {
            isInAttackPhrase = value;
        }
    }
}

