using Assets.Scripts.DataProviders;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerChooserHandler : MonoBehaviour
{
    [SerializeField] private InputField newPlyerInputField;
    [SerializeField] private Button addPlayerButton;
    [SerializeField] private Button removePlayerButton;
    [SerializeField] private string mazeDataBaseFileName;

    private void Awake()
    {
        MazeDataBase.fileName = mazeDataBaseFileName;
    }

    void Start()
    {
        var dropdown = transform.GetComponent<Dropdown>();
        dropdown.options.Clear();        
        var listOfPlayersFromDataBase = new ListOfPlayersFromDataBase();
        listOfPlayersFromDataBase.CreateDataBaseFileIfNotExists();
        listOfPlayersFromDataBase.CreateTableForListOfPlayersIfNotExists();
        listOfPlayersFromDataBase.GetListOfPlayersFromDataBase();
        var players = listOfPlayersFromDataBase.players;
        var indexOfCurrentPlayer = 0;
        var i = 0;
        foreach (var player in players)
        {
            dropdown.options.Add(new Dropdown.OptionData() { text = player });
            if (player == MazePlayMode.currentPlayer)
            {
                indexOfCurrentPlayer = i;
            }
            i++;
        }
        dropdown.value = indexOfCurrentPlayer + 1;
        dropdown.value = indexOfCurrentPlayer;  // must toggle to get a visual refresh
        PlayerSelected(dropdown);
        dropdown.onValueChanged.AddListener(delegate { PlayerSelected(dropdown); });
        addPlayerButton.onClick.AddListener(delegate { AddPlayerButtonClicked(dropdown); });
        removePlayerButton.onClick.AddListener(delegate { RemovePlayerButtonClicked(dropdown); });
    }

    void PlayerSelected(Dropdown dropdown)
    {
        MazePlayMode.currentPlayer = dropdown.options[dropdown.value].text;
    }

    void AddPlayerButtonClicked(Dropdown dropdown)
    {
        var newPlayerName = newPlyerInputField.text;
        newPlyerInputField.Select();
        newPlyerInputField.text = "";

        if (string.IsNullOrEmpty(newPlayerName))
            return;
        if (string.IsNullOrEmpty(newPlayerName.Trim()))
            return;

        var listOfPlayersFromDataBase = new ListOfPlayersFromDataBase();
        listOfPlayersFromDataBase.AddPlayer(newPlayerName.Trim());

        UpdateDropdownWithSelection(dropdown, newPlayerName.Trim());
    }

    void RemovePlayerButtonClicked(Dropdown dropdown)
    {
        var playerNameToBeRemoved = dropdown.options[dropdown.value].text;

        var listOfHighScoresFromDataBase = new ListOfHighScoresFromDataBase();
        listOfHighScoresFromDataBase.RemoveHighScoresForThisPlayer(playerNameToBeRemoved);

        if (playerNameToBeRemoved == "Guest")
            return;

        var listOfPlayersFromDataBase = new ListOfPlayersFromDataBase();
        listOfPlayersFromDataBase.RemovePlayer(playerNameToBeRemoved);

        UpdateDropdownWithSelection(dropdown, "Guest");
    }

    void UpdateDropdownWithSelection(Dropdown dropdown, string nameToSelect)
    {
        dropdown.options.Clear();
        var listOfPlayersFromDataBase = new ListOfPlayersFromDataBase();
        listOfPlayersFromDataBase.GetListOfPlayersFromDataBase();
        var players = listOfPlayersFromDataBase.players;
        var indexOfSelected = 0;
        for (var i = 0; i < players.Count; i++)       
        {
            dropdown.options.Add(new Dropdown.OptionData() { text = players[i] });

            if (players[i] == nameToSelect)
                indexOfSelected = i;

        }
        dropdown.value = -1;  
        dropdown.value = indexOfSelected;  // must toggle to get a visual refresh
        PlayerSelected(dropdown);
    }

    public void OnEnterPressed()
    {        
        AddPlayerButtonClicked(transform.GetComponent<Dropdown>());
    }

}
