using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterCustomHandler : MonoBehaviour
{
    [SerializeField] private List<Transform> _heads;

    private async void Start()
    {
        await SetupHead();
        ShowRandomHead();
    }

    private async void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            await SetupHead();
            ShowRandomHead();
        }
    }

    private async UniTask SetupHead()
    {
        // Hide all heads asynchronously
        await UniTask.WhenAll(_heads.Select(HideHeadAsync));

        // Wait until all heads are hidden or timeout occurs
        try
        {
            await UniTask.WaitUntil(() => AllHeadsHidden()).Timeout(TimeSpan.FromSeconds(10));
        }
        catch (TimeoutException e)
        {
            Debug.LogException(e);
        }
    }

    // Function to hide a head asynchronously
    private async UniTask HideHeadAsync(Transform head)
    {
        head.gameObject.SetActive(false);
        await UniTask.Yield();
    }

    // Function to check if all heads are hidden
    private bool AllHeadsHidden()
    {
        return _heads.All(head => !head.gameObject.activeSelf);
    }

    private void ShowRandomHead()
    {
        int indexHead = UnityEngine.Random.Range(0, _heads.Count);
        ShowHeadByIndex(indexHead);
    }

    private void ShowHeadByIndex(int index)
    {
        if (index >= 0 && index < _heads.Count)
        {
            _heads[index].gameObject.SetActive(true);
        }
    }
}