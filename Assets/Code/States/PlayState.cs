﻿using Assets.Code.DataPipeline;
using Assets.Code.DataPipeline.Providers;
using Assets.Code.Messaging;
using Assets.Code.UI;
using Assets.Code.Ui.CanvasControllers;
using UnityEngine;

namespace Assets.Code.States
{
    public class PlayState : BaseState
    {
        private readonly Messager _messager;
        private readonly CanvasProvider _canvasProvider;
        private UiManager _uiManager;

        public PlayState(IoCResolver resolver) : base(resolver)
        {
            _resolver.Resolve(out _messager);
            _resolver.Resolve(out _canvasProvider);
        }

        public override void Initialize()
        {
            _uiManager = new UiManager();
            _uiManager.RegisterUi(new PlayStateCanvasController(_messager, _canvasProvider.GetCanvas("play_canvas")));
        }

        public override void Update()
        {
        }

        public override void HandleInput()
        {
        }

        public void TalkButtonPressed()
        {
            Debug.Log("Talk Button Pressed!");
        }

        public override void TearDown()
        {
        }
    }
}
