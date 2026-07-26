using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [Header("Tutorial Pages")]
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private List<GameObject> pages = new List<GameObject>();

    [Header("Navigation Buttons")]
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;

    [SerializeField] private Button openButton;
    [SerializeField] private Button closeButton;


    private int currentPage = 0;

    private void Start()
    {
        prevButton.onClick.AddListener(PreviousPage);
        nextButton.onClick.AddListener(NextPage);

        ShowPage(currentPage);
    }

    private void ShowPage(int index)
    {
        if (pages.Count == 0)
            return;
        
        if (currentPage != pages.Count - 1)
        {
            closeButton.gameObject.SetActive(false);
        }
        else
        {
            closeButton.gameObject.SetActive(true);
        }

        currentPage = Mathf.Clamp(index, 0, pages.Count - 1);

        // Tắt toàn bộ page
        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].SetActive(i == currentPage);
        }

        UpdateButtons();
    }

    public void NextPage()
    {
        if (currentPage < pages.Count - 1)
        {
            ShowPage(currentPage + 1);
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            ShowPage(currentPage - 1);
        }
    }

    private void UpdateButtons()
    {
        prevButton.interactable = currentPage > 0;
        nextButton.interactable = currentPage < pages.Count - 1;
    }

    public void OpenTutorial()
    {
        tutorialPanel.SetActive(true);
        currentPage = 0;
    }

    public void CloseTutorial()
    {
        tutorialPanel.SetActive(false);
    }
}