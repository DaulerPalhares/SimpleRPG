﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CharacterSelection.cs" by="Akapagion">
//  © Copyright Dauler Palhares da Costa Viana 2017.
//          http://github.com/DaulerPalhares
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System.Collections;
using TMPro;
using UnityEngine;


public class CharacterSelection : MonoBehaviour
{
    /// <summary>
    /// Character
    /// </summary>
    public CharacterSkin SelectCharacterSkin { get; private set; }
    public PlayerStats SelectStats { get; private set; }
    public CharacterSkin[] CharactersSkin;
    public PlayerStats[] CharacterStatsBase;
    public TMP_Text SkinName;
    public StatsUiPolygon StatsUi;
    public string NickName;
    public TMP_InputField NickNameInput;

    private int _selectedCharacterIndex = 0;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        SelectStats = CharacterStatsBase[0];
        OnCharacterSelect(_selectedCharacterIndex);
        GameController.Instance.Player.PlayerStats.ResetStats();
        GameController.Instance.Player.PlayerStats.ResetExp();
        StartCoroutine("Rotate");
        StatsUi.UpdateStatsGui();
    }

    private void OnEnable()
    {
        StopCoroutine(Rotate());
        StartCoroutine("Rotate");
    }

    /// <summary>
    /// Cotinuos rotation of the character obj preview.
    /// </summary>
    /// <returns></returns>
    IEnumerator Rotate()
    {
        var rmp = 1f;
        while (true)
        {
            transform.Rotate(0,rmp,0);
            yield return null;
        }
    }

    public void ClearAllChildern()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void UpdateStatsGui()
    {
        var skin = SelectCharacterSkin.CharacterObj.name.Split('_');
        SkinName.text = skin[1];
        StatsUi.UpdateStatsGui();
    }
    /// <summary>
    /// Mehtod to assign player nickname.
    /// </summary>
    /// <param name="n"></param>
    public void AssignNickname(string n)
    {
        NickName = n;
    }
    /// <summary>
    /// Method called to change the current selected character obj.
    /// </summary>
    /// <param name="characterChoice">Character index</param>
    public void OnCharacterSelect(int characterChoice)
    {
        _selectedCharacterIndex += characterChoice;
        SelectCharacterSkin = CharactersSkin[Mathf.Abs(_selectedCharacterIndex%CharactersSkin.Length)];
        ClearAllChildern();
        transform.Rotate(Vector3.up*180);
        var temp = Instantiate(SelectCharacterSkin.CharacterObj, transform.position, Quaternion.identity);
        temp.transform.parent = transform;
        temp.transform.localScale = Vector3.one;
        temp.transform.rotation = transform.rotation;
        UpdateStatsGui();
    }
    /// <summary>
    /// Method called by changing the character class.
    /// </summary>
    /// <param name="statsChoice">Id of dropdown menu</param>
    public void OnStatsSelect(int statsChoice)
    {
        SelectStats = CharacterStatsBase[statsChoice];
        ApplyStats();
        UpdateStatsGui();
    }

    public void ApplyStats()
    {
        GameController.Instance.Player.PlayerStats.PlayerClass = SelectStats.PlayerClass;
        GameController.Instance.Player.PlayerStats.PlayerAgi = SelectStats.PlayerAgi;
        GameController.Instance.Player.PlayerStats.PlayerCon = SelectStats.PlayerCon;
        GameController.Instance.Player.PlayerStats.PlayerDex = SelectStats.PlayerDex;
        GameController.Instance.Player.PlayerStats.PlayerInt = SelectStats.PlayerInt;
        GameController.Instance.Player.PlayerStats.PlayerLuk = SelectStats.PlayerLuk;
        GameController.Instance.Player.PlayerStats.PlayerVit = SelectStats.PlayerVit;
    }
}
