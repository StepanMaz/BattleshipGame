using BattleShipGameClient.Commands;
using BSGameClient.Commands.Exceptions;
using BSGameClient.Services;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;

namespace BSGameClient.Windows
{
    internal class AppWindow : Window
    {
        private ICommand<Guid>? joinLobbyCommand;
        private IResultCommand<Task<Guid>>? createLobbyCommand;
        private IResultCommand<Task<IEnumerable<Guid>>>? getAllLobbiesCommand;

        public AppWindow(IResultCommand<Task<IEnumerable<Guid>>> getAllLobbiesCommand)
        {
            this.getAllLobbiesCommand = getAllLobbiesCommand;

            InitComponents();
        }

        public void SetJoinLobby(ICommand<Guid> joinLobbyCommand)
        {
            this.joinLobbyCommand = joinLobbyCommand;
        }

        public void SetCreateLobby(IResultCommand<Task<Guid>> createLobbyCommand)
        {
            this.createLobbyCommand = createLobbyCommand;
        }

        private ScrollView lobbies = null!;

        private void InitComponents()
        {
            Title = "Game";

            var leftsideConteiner = new FrameView()
            {
                Width = Dim.Percent(60),
                Height = Dim.Fill(),
            };

            var rightsideConteiner = new FrameView()
            {
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                X = Pos.Right(leftsideConteiner) + 1
            };

            var createLobbyButton = new Button("Create lobby") 
            { 
                X = Pos.Center(),
                Y = Pos.Center()
            };
            createLobbyButton.Clicked += async () => joinLobbyCommand?.Execute(await (createLobbyCommand?.Execute() ?? throw new CommandWasNotProvidedException()));

            var refreshButton = new Button("Refresh");

            lobbies = new ScrollView()
            {
                ShowVerticalScrollIndicator = true,
                ShowHorizontalScrollIndicator = true,
                X = Pos.Center(),
                Border = new Border() { BorderStyle = BorderStyle.Rounded, Background = Color.Black },
                Y = Pos.Center(),
                Width = 18,
                Height = Dim.Percent(60),
                ContentSize = new Size(15, 23)
            };

            refreshButton.Clicked += LoadLobbies;

            leftsideConteiner.Add(createLobbyButton);
            rightsideConteiner.Add(refreshButton, lobbies);

            Add(leftsideConteiner, rightsideConteiner);
            
            LoadLobbies();
        }

        private async void LoadLobbies()
        {
            var lobbies = (await (getAllLobbiesCommand?.Execute() ?? throw new CommandWasNotProvidedException())).ToArray();
            
            this.lobbies.Clear();

            this.lobbies.ContentSize = new Size(16, lobbies.Length);

            for (int i = 0; i < lobbies.Length; i++)
            {
                var guid = lobbies[i];

                var button = new Button($"Lobby {i}") { Y = i, X = Pos.Center() };

                button.Clicked += () =>joinLobbyCommand?.Execute(guid);

                this.lobbies.Add(button);
            }
        }
    }
}
