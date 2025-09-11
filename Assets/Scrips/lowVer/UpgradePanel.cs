using System.Collections.Generic;
using UnityEngine;

public class UpgradePanel : MonoBehaviour
{
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private UpgradeOptionUI[] optionPanels; // mỗi ô panel gắn script UpgradeOptionUI

    private List<UpgradeData> currentChoices;

    public void ShowUpgradeChoices()
    {
        currentChoices = UpgradeManager.Instance.GetUpgradeChoices();

        if (currentChoices.Count == 0)
        {
            Debug.Log("Không còn upgrade hợp lệ!");
            return;
        }

        panelRoot.SetActive(true);

        for (int i = 0; i < optionPanels.Length; i++)
        {
            if (i < currentChoices.Count)
            {
                optionPanels[i].gameObject.SetActive(true);
                optionPanels[i].SetData(currentChoices[i]);
            }
            else
            {
                optionPanels[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnEnable()
    {
        ShowUpgradeChoices();
    }
}
