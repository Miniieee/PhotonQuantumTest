using System;
using System.Collections.Generic;
using UnityEngine;

namespace Quantum.Menu
{
    public class QuantumMenuUICharacterSelection: QuantumMenuUIScreen
    {
        [SerializeField] private CharacterModel[] characterModels;
        [SerializeField] private UI_SelectableCharacter selectableCharacter;
        [SerializeField] private Transform characterSelectionParent;
        [SerializeField] private QuantumMenuUIController quantumMenuUIController;

        private Dictionary<CharacterModel, UI_SelectableCharacter> _selectableCharacterMap = new Dictionary<CharacterModel, UI_SelectableCharacter>();
        private CharacterModel _characterModelCurrentlySelected;

        public override void Awake()
        {
            UI_SelectableCharacter.OnCharacterSelected += OnCharacterSelected;
            InitializeCharacterSelection();
        }

        private void InitializeCharacterSelection()
        {
            for(int i = 0; i < characterModels.Length; i++)
            {
                var characterModel = characterModels[i];
                var uiSelectableCharacter = Instantiate(selectableCharacter, characterSelectionParent);
                uiSelectableCharacter.Initialize(characterModel); // Select the first character by default
                _selectableCharacterMap[characterModel] = uiSelectableCharacter;
            }

            OnCharacterSelected(characterModels[0]);
        }

        private void OnCharacterSelected(CharacterModel model)
        {
            if(_characterModelCurrentlySelected == model)
                return;
            if(_characterModelCurrentlySelected != null)
            {
                _selectableCharacterMap[_characterModelCurrentlySelected].SetSelected(false);
            }
            _selectableCharacterMap[model].SetSelected(true);
            _characterModelCurrentlySelected = model;
            quantumMenuUIController.ConnectArgs.RuntimePlayers[0].PlayerAvatar = model.EntityPrototype;
        }

        public virtual void OnBackButtonPressed()
        {
            Controller.Show<QuantumMenuUIMain>();
        }

        public void OnDestroy()
        {
            UI_SelectableCharacter.OnCharacterSelected -= OnCharacterSelected;
        }
    }
}
