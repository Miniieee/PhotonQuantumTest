using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Quantum.Menu
{
    public class UI_SelectableCharacter : MonoBehaviour
    {
        public static event Action<CharacterModel> OnCharacterSelected;

        [SerializeField] private Image characterImage;
        [SerializeField] private TMP_Text characterName;
        [SerializeField] private GameObject characterSelected;

        private CharacterModel _model;

        public void Initialize(CharacterModel model)
        {
            _model = model;
            characterImage.sprite = model.CharacterImage;
            characterName.text = model.CharacterName;

        }

        public void SetSelected(bool isSelected)
        {
            characterSelected.SetActive(isSelected);
        }

        public void CharacterSelected()
        {
            OnCharacterSelected?.Invoke(_model);
        }
    }
}
