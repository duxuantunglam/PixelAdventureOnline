using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_RefreshButton : MonoBehaviour
{
    [SerializeField] private Button refreshButton;

    private void Awake()
    {
        if (refreshButton == null)
        {
            refreshButton = GetComponent<Button>();
        }

        refreshButton.onClick.AddListener(RefreshSessionList);
    }

    private void RefreshSessionList()
    {
        StartCoroutine(RefreshSessionWait());
    }

    private IEnumerator RefreshSessionWait()
    {
        refreshButton.interactable = false;
        FusionManager.instance.RefreshSessionListUI();
        yield return new WaitForSeconds(3f);
        refreshButton.interactable = true;
    }
}