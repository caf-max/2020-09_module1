using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    public string wonLabel;
    public string lostLabel;
    public TextMeshProUGUI endHeader;
    public CanvasGroup menuPanel;
    public CanvasGroup buttonPanel;
    public CanvasGroup endGamePanel;
    public Button button;
    public Character[] playerCharacter;
    public Character[] enemyCharacter;
    Character currentTarget;
    bool waitingForInput;
    bool isPause;

    Character FirstAliveCharacter(Character[] characters)
    {
        // LINQ: return enemyCharacter.FirstOrDefault(x => !x.IsDead());
        foreach (var character in characters) {
            if (!character.IsDead())
                return character;
        }
        return null;
    }

    void PlayerWon()
    {
        Debug.Log("Player won.");
        endHeader.text = wonLabel;
        Utility.SetCanvasGroupEnabled(buttonPanel, false);
        Utility.SetCanvasGroupEnabled(endGamePanel, true);
    }

    void PlayerLost()
    {
        Debug.Log("Player lost.");
        endHeader.text = lostLabel;
        Utility.SetCanvasGroupEnabled(buttonPanel, false);
        Utility.SetCanvasGroupEnabled(endGamePanel, true);
    }

    public void PauseGame()
    {
        isPause = !isPause;
        Utility.SetCanvasGroupEnabled(buttonPanel, !isPause);
        Utility.SetCanvasGroupEnabled(menuPanel, isPause);
    }

    public void ReloadGame()
    {
        LoadingScreen.instance.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        LoadingScreen.instance.LoadScene("MainMenu");
    }

    bool CheckEndGame()
    {
        if (FirstAliveCharacter(playerCharacter) == null) {
            PlayerLost();
            return true;
        }

        if (FirstAliveCharacter(enemyCharacter) == null) {
            PlayerWon();
            return true;
        }

        return false;
    }

    void PlayerAttack()
    {
        waitingForInput = false;
    }

    public void NextTarget()
    {
        int index = Array.IndexOf(enemyCharacter, currentTarget);
        for (int i = 1; i < enemyCharacter.Length; i++) {
            int next = (index + i) % enemyCharacter.Length;
            if (!enemyCharacter[next].IsDead()) {
                currentTarget.targetIndicator.gameObject.SetActive(false);
                currentTarget = enemyCharacter[next];
                currentTarget.targetIndicator.gameObject.SetActive(true);
                return;
            }
        }
    }

    IEnumerator GameLoop()
    {
        yield return null;
        while (!CheckEndGame()) {
            foreach (var player in playerCharacter) {
                if (!player.IsDead()) {
                    currentTarget = FirstAliveCharacter(enemyCharacter);
                    if (currentTarget == null)
                        break;

                    currentTarget.targetIndicator.gameObject.SetActive(true);
                    Utility.SetCanvasGroupEnabled(buttonPanel, true);

                    waitingForInput = true;
                    while (waitingForInput)
                        yield return null;

                    Utility.SetCanvasGroupEnabled(buttonPanel, false);
                    currentTarget.targetIndicator.gameObject.SetActive(false);

                    player.target = currentTarget.transform;
                    player.AttackEnemy();

                    while (!player.IsIdle())
                        yield return null;

                    break;
                }
            }

            foreach (var enemy in enemyCharacter) {
                if (!enemy.IsDead()) {
                    Character target = FirstAliveCharacter(playerCharacter);
                    if (target == null)
                        break;

                    enemy.target = target.transform;
                    enemy.AttackEnemy();

                    while (!enemy.IsIdle())
                        yield return null;

                    break;
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(PlayerAttack);
        Utility.SetCanvasGroupEnabled(buttonPanel, false);
        Utility.SetCanvasGroupEnabled(menuPanel, false);
        Utility.SetCanvasGroupEnabled(endGamePanel, false);
        StartCoroutine(GameLoop());
        isPause = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
