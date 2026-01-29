
using System.Globalization;
using Quantum;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using System;

public class CountdownUI : QuantumSceneViewComponent
{
    [SerializeField] TMP_Text timeRemainingText;
    [SerializeField] Image timeProgressImage;

    private QuantumRunner _runner;

    public override void OnActivate(Frame frame)
    {
        QuantumEvent.Subscribe<EventOnGameOver>(this, OnGameOver);
        QuantumEvent.Subscribe<EventOnGameStateChanged>(this, OnGameStateChanged);
        _runner = QuantumRunner.Default;
    }

    private void OnGameStateChanged(EventOnGameStateChanged callback)
    {
        var f = callback.Game.Frames.Predicted;
        var gameManager = f.GetSingleton<GameManager>();
        if (gameManager.CurrentGameState != GameState.WaitingForPlayers)
        {
            DisableRoomJoining();
        }
    }

    private void DisableRoomJoining()
    {
        if (_runner.NetworkClient == null)
            return;
        if (_runner.NetworkClient.CurrentRoom.IsVisible == false)
            return;
        _runner.NetworkClient.CurrentRoom.IsVisible = false;
        Quantum.Log.Info("Room is now closed for joining new players.");
    }

    private void OnGameOver(EventOnGameOver callback)
    {
        var f = callback.Game.Frames.Predicted;
        var playerRef = f.Get<PlayerLink>(callback.Winner).Player;
        var playerData = f.GetPlayerData(playerRef);

        Quantum.Log.Info($"Game Over! Winner: {playerData.PlayerNickname}");
    }

    public override void OnDeactivate()
    {
        QuantumEvent.UnsubscribeListener<EventOnGameOver>(this);
        QuantumEvent.UnsubscribeListener<EventOnGameStateChanged>(this);
    }

    public override void OnLateUpdateView()
    {
        var f = PredictedFrame;
        var shrinkingCircle = f.GetSingleton<ShrinkingCircle>();
        var time = shrinkingCircle.CurrentTimeToNextState;
        var currentState = shrinkingCircle.CurrentState;

        timeRemainingText.text = time.AsFloat.ToString("F2", CultureInfo.InvariantCulture);
        timeProgressImage.fillAmount = 1 - (time.AsFloat / currentState.TimeToNextState.AsFloat);

    }
}
