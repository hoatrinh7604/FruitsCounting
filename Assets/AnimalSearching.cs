using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimalSearching : MonoBehaviour
{
    [SerializeField] Sprite[] listAnimal;
    [SerializeField] Image target;
    [SerializeField] Image nextTarget;
    [SerializeField] Button[] button;

    public void OnPress(int buttonIndex)
    {
        GamePlayController.Instance.OnPressHandle(buttonIndex);
    }

    public void UpdateTextButton(List<int> list)
    {
        for(int i = 0; i< list.Count; i++)
        {
            button[i].GetComponentInChildren<TextMeshProUGUI>().text = list[i].ToString();
        }
    }

    public void SetTarget(int id)
    {
        target.sprite = listAnimal[id];
        //name.text = listName[id];
    }

    public void SetNextTarget(int id)
    {
        nextTarget.sprite = listAnimal[id];
        //name.text = listName[id];
    }

    public int GetAmountAnimals()
    {
        return listAnimal.Length;
    }
}
