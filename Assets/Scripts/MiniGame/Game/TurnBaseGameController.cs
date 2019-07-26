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
        EnemyController enemyController;

        [SerializeField]
        Status playerHealth;

        [SerializeField]
        Status playerEnergy;

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
        Text lblPlayerDamageReceived;

        [SerializeField]
        Animator lblPlayerDamageReceivedAnim;

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

        [SerializeField]
        RectTransform panelHealthRestore;

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

            lblPlayerDamageReceived.text = "";
            lblEnemyDamageReceived.text = "";

            btnUltimate.gameObject.SetActive(false);
            countdownMenu.gameObject.SetActive(true);
            panelHealthRestore.gameObject.SetActive(false);

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

                sliderAttack.gameObject.SetActive(true);
                panelMeter.gameObject.SetActive(true);
            });

            btnHeal.onClick.AddListener(() =>
            {
                if (isInAttackPhrase || turn == Turn.Enemy)
                    return;

                actionCanvasGroup.interactable = false;

                isInAttackPhrase = true;
                gameTimer.Pause(true);

                sliderHealth.gameObject.SetActive(true);
                panelMeter.gameObject.SetActive(true);
            });

            sliderAttack.OnValueMax += SliderAttack_OnValueMax;
            sliderAttack.OnTimeoutValue += SliderAttack_OnTimeoutValue;

            sliderHealth.OnValueMax += SliderHeal_OnValueMax;
            sliderHealth.OnTimeoutValue += SliderHeal_OnTimeoutValue;

            playerController.OnAttacking += Player_OnAttacking;
            playerController.OnAttackFinished += Player_OnAttackFinished;

            enemyController.OnAttacking += Enemy_OnAttacking;
            enemyController.OnAttackFinished += Enemy_OnAttackFinished;
        }

        void SliderAttack_OnValueMax(float value)
        {
            HidePanelMeter();
            playerController.AttackEnemy();
        }

        void SliderAttack_OnTimeoutValue(float value)
        {
            HidePanelMeter();
            playerController.AttackEnemy(playerController.AttackPoint * 0.5f);
        }

        void SliderHeal_OnValueMax(float value)
        {
            HidePanelMeter();

            playerHealth.Restore(20);
            panelHealthRestore.gameObject.SetActive(true);

            Debug.Log("Health is restore..");
            StartCoroutine(SliderHeal_Callback());
        }

        void SliderHeal_OnTimeoutValue(float value)
        {
            HidePanelMeter();
            StartCoroutine(SliderHeal_Callback());
        }

        IEnumerator SliderHeal_Callback()
        {
            yield return new WaitForSeconds(1.5f);

            panelHealthRestore.gameObject.SetActive(false);
            isInAttackPhrase = false;

            NextTurn();
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

            enemyController.OnAttacking -= Enemy_OnAttacking;
            enemyController.OnAttackFinished -= Enemy_OnAttackFinished;
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
            lblEnemyDamageReceived.text = (isMakingLessDamage) ? value.ToString() : "(Critical)\n" + value;
            lblEnemyDamageReceivedAnim.SetTrigger("Play");
        }

        void Enemy_OnAttacking()
        {
            lblPlayerDamageReceived.text = enemyController.AttackPoint.ToString();
            lblPlayerDamageReceivedAnim.SetTrigger("Play");
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

        void Enemy_OnAttackFinished()
        {
            isInAttackPhrase = false;
            StartCoroutine(Enemy_OnAttackFinished_Callback());
        }

        IEnumerator Enemy_OnAttackFinished_Callback()
        {
            yield return new WaitForSeconds(1.5f);
            lblPlayerDamageReceived.text = "";
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
            btnUltimate.interactable = (turn == Turn.Player);

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

            switch (turn)
            {
                case Turn.Player:
                    bool canUseUltimate = (playerEnergy.Current >= playerEnergy.Maximum);
                    btnUltimate.gameObject.SetActive(canUseUltimate);
                    break;

                case Turn.Enemy:
                    gameTimer.Pause(true);
                    enemyController.AttackPlayer();
                    break;

                default:
                    break;
            }
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

