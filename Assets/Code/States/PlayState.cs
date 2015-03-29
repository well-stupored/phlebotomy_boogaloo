﻿﻿using System;
﻿using System.Runtime.InteropServices;
using Assets.Code.DataPipeline;
using Assets.Code.DataPipeline.Providers;
using Assets.Code.Messaging;
using Assets.Code.Messaging.Messages;
using Assets.Code.UI;
using Assets.Code.Ui.CanvasControllers;
using UnityEngine;

namespace Assets.Code.States
{
    public enum MistakeType
    {
        NameMismatch = 0,
        IdMismatch,
        WrongTube,
        NoPermission
    }

    public class PlayState : BaseState
    {
        /* PROPERTIES */
        private readonly Messager _messager;
        private readonly CanvasProvider _canvasProvider;
		private readonly PrefabProvider _prefabProvider;
        private UiManager _uiManager;

        private int _currentStage;

        /* REFERENCES */

        /* TOKENS */
        private MessagingToken _onTalkButtonClicked;
        private static PatientGenerator _patientGenerator = new PatientGenerator();

        private Canvas _playCanvas;
		private Tube _tube;
		private GameObject _tubeSlider;

        public PlayState(IoCResolver resolver) : base(resolver)
        {
            _resolver.Resolve(out _messager);
            _resolver.Resolve(out _canvasProvider);
            _resolver.Resolve(out _prefabProvider);
        }

        public override void Initialize()
        {
            _currentStage = 0;

            _uiManager = new UiManager();
            _uiManager.RegisterUi(new PlayStateCanvasController(_messager, _canvasProvider.GetCanvas("play_canvas")));

            _onTalkButtonClicked = _messager.Subscribe<TalkButtonClickedMessage>(OnTalkButtonClicked);

            _patientGenerator = new PatientGenerator();
			_playCanvas = _canvasProvider.GetCanvas("play_canvas");
			_playCanvas.gameObject.SetActive(true);
			_tubeSlider = GameObject.Instantiate(_prefabProvider.GetPrefab("Slider"));
			_tubeSlider.transform.SetParent(_playCanvas.transform);
			_tubeSlider.transform.localScale = new Vector3(2.5f, 10, 1);
			_tubeSlider.transform.localPosition = new Vector3(723.24f, 118.9f, 0);
			_tube = _tubeSlider.GetComponent<Tube>();
			_tube.StartDraw();
        }

        public override void Update()
        {
            _uiManager.Draw();
        }

        public override void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                _messager.Publish(new PatientTalkMessage());
            }
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				SwitchState(new MenuState(_resolver));
			}
        }

        public void OnTalkButtonClicked(TalkButtonClickedMessage message)
        {
            var Message = new PatientTalkMessage();

            switch (_currentStage)
            {
                case 0:
                case 1:
                    Message.Text = "I'm Adam Sandler.";
                    _currentStage = Math.Min(1, _currentStage);
                    break;
                case 2:
                    Message.Text = "Sure, go ahead.";
                    break;
                case 3:
                case 4:
                case 5:
                case 6:
                    Message.Text = "...";
                    break;
                case 7:
                case 8:
                    Message.Text = "Thanks";
                    break;
                default:
                    Message.Text = "Alright, I'll be going now";
                    break;
            }

            _messager.Publish(Message);
        }

        public override void TearDown()
        {
			_messager.CancelSubscription(_onTalkButtonClicked);

			_uiManager.TearDown();

			UnityEngine.Object.Destroy(_tubeSlider);
        }

        public void NewPatient()
        {
            //_patient = _patientGenerator.GeneratePatient();
        }

        public void HandleMistake(MistakeType mistake)
        {
            switch (mistake)
            {
                case MistakeType.NameMismatch:
                    NameMismatch();
                    break;
                case MistakeType.IdMismatch:
                    IdMismatch();
                    break;
                case MistakeType.WrongTube:
                    WrongTube();
                    break;
                case MistakeType.NoPermission:
                    NoPermission();
                    break;
            }
        }

        private void NameMismatch()
        {
        }

        private void IdMismatch()
        {
        }

        private void WrongTube()
        {
        }

        private void NoPermission()
        {
        }
    }
}
