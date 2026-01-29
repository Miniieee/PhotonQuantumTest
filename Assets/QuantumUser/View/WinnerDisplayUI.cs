using System.Collections;
using System.Collections.Generic;
using Quantum;
using TMPro;
using UnityEngine;

public class WinnerDisplayUI : QuantumSceneViewComponent
{
    [SerializeField] private GameObject winnerPanel;
    [SerializeField] private TMP_Text winnerText;

    public override void OnActivate(Frame frame)
    {
        QuantumEvent.Subscribe<EventOnGameOver>(this, OnGameOver);
        winnerPanel.SetActive(false);
    }

    private void OnGameOver(EventOnGameOver callback)
    {
        var f = callback.Game.Frames.Predicted;
        var playerRef = f.Get<PlayerLink>(callback.Winner).Player;
        var playerData = f.GetPlayerData(playerRef);

        winnerPanel.SetActive(true);
        winnerText.text = $"Winner: {playerData.PlayerNickname}";
    }

    public override void OnDeactivate()
    {
        QuantumEvent.UnsubscribeListener<EventOnGameOver>(this);
    }
}
