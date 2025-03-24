using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Swift_Blade.UI
{
    public class HelpPopup : PopupUI
    {
        private int index;
        [SerializeField] private List<GameObject> pages;
        [SerializeField] private Button nextPageButton;
        [SerializeField] private Button prevPageButton;

        private void OnEnable()
        {
            index = 0;
        }
    }
}
