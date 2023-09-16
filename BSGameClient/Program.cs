using BattleShipGameClient.Commands;
using BattleShipGameClient.Windows;
using BSGameClient.Commands;
using BSGameClient.Game.FieldFiller;
using BSGameClient.Services;
using BSGameClient.Windows;
using Terminal.Gui;

Application.Init();

var apiClient = new ApiClient(new Uri("https://localhost:7001"));

var login = new LoginWindow();
var register = new RegisterWindow();
var app = new AppWindow(new GetAllLobbiesCommand(apiClient));

var loginCommand = new LogInCommand(apiClient);
loginCommand.SetOnSuccess(new OpenWindowCommand(app));

login.SetLogIn(loginCommand);
login.SetOpenRegister(new OpenWindowCommand(register));

var registerCommand = new RegisterCommand(apiClient);
registerCommand.SetOnSuccess(new OpenWindowCommand(app));

register.SetOpenLogin(new OpenWindowCommand(login));
register.SetRegister(registerCommand);

app.SetCreateLobby(new CreateLobbyCommand(apiClient));
app.SetJoinLobby(new JoinLobbyCommand("https://localhost:7001/game", new FieldFIllingStrategy()));

GameWindow.SetReturnCommand(new OpenWindowCommand(app));

Application.Top.Add(login);

Application.Run();
