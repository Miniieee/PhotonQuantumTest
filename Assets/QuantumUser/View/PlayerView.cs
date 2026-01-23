using System;
using System.Collections;
using System.Collections.Generic;
using Quantum;
using TMPro;
using UnityEngine;

public unsafe class PlayerView : QuantumEntityViewComponent
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject overheadUi;
    [SerializeField] private TMP_Text playerNameText;

    private bool _isLocalPlayer;
    private Renderer[] _renderers;

    private static readonly int MoveXHash = Animator.StringToHash("moveX");
    private static readonly int MoveZHash = Animator.StringToHash("moveZ");

    void Awake()
    {
        _renderers = GetComponentsInChildren<Renderer>(includeInactive: true);
    }

    public override void OnActivate(Frame frame)
    {
        var playerLink = frame.Get<PlayerLink>(EntityRef);
        _isLocalPlayer = _game.PlayerIsLocal(playerLink.Player);
        var playerData = frame.GetPlayerData(playerLink.Player);
        playerNameText.text = playerData.PlayerNickname;
        var layer = UnityEngine.LayerMask.NameToLayer(_isLocalPlayer ? "Player_Local" : "Player_Remote");

        foreach (var renderer in _renderers)
        {
            renderer.gameObject.layer = layer;
            renderer.enabled = true;
        }

        overheadUi.SetActive(true);
        QuantumEvent.Subscribe<EventOnPlayerEnteredGrass>(this, OnPlayerEnteredGrass);
        QuantumEvent.Subscribe<EventOnPlayerExitedGrass>(this, OnPlayerExitedGrass);
    }

    public override void OnDeactivate()
    {
        QuantumEvent.UnsubscribeListener<EventOnPlayerEnteredGrass>(this);
        QuantumEvent.UnsubscribeListener<EventOnPlayerExitedGrass>(this);
    }

    private void OnPlayerEnteredGrass(EventOnPlayerEnteredGrass callback)
    {
        SetRendererVisibility(callback.Player, false);
    }

    private void OnPlayerExitedGrass(EventOnPlayerExitedGrass callback)
    {


        SetRendererVisibility(callback.Player, true);
    }

    private void SetRendererVisibility(PlayerRef player, bool visible)
    {
        if (player != PredictedFrame.Get<PlayerLink>(EntityRef).Player)
            return;
        if (_isLocalPlayer)
            return;

        foreach (var renderer in _renderers)
        {
            renderer.enabled = visible;
        }

        overheadUi.SetActive(visible);
    }

    public override void OnUpdateView()
    {
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        if(!PredictedFrame.Exists(EntityRef))
            return;
        
        var input = PredictedFrame.GetPlayerInput(PredictedFrame.Get<PlayerLink>(EntityRef).Player);
        var kcc = PredictedFrame.Get<KCC>(EntityRef);
        var velocity = kcc.Velocity;

        animator.SetFloat(MoveXHash, velocity.X.AsFloat);
        animator.SetFloat(MoveZHash, velocity.Y.AsFloat);
    }
}
