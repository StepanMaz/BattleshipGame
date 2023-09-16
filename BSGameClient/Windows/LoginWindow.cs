using System.Diagnostics;
using BattleShipGameClient.Commands;
using Terminal.Gui;

namespace BattleShipGameClient.Windows;

public class LoginWindow : Window
{
    private IResultCommand<string, string, Task>? logInCommand;
    private ICommand? openRegitserWindow;
    public LoginWindow()
    {
        InitComponent();    
    }

    public void SetLogIn(IResultCommand<string, string, Task> logInCommand)
    {
        this.logInCommand = logInCommand;
    }

    public void SetOpenRegister(ICommand openRegitserWindow)
    {
        this.openRegitserWindow = openRegitserWindow;
    }

    private void InitComponent()
    {
        lock(this)
        {
            Title = "Login";

            var loginLablel = new Label()
            {
                Text = "Login:",
                X = 1,
                Y = 1
            };

            var passwordLablel = new Label()
            {
                Text = "Password:",
                X = Pos.Left(loginLablel),
                Y = Pos.Bottom(loginLablel) + 2
            };

            var loginTextField = new TextField("")
            {
                X = Pos.Right(loginLablel) + 1,
                Y = Pos.Top(loginLablel),
                Width = Dim.Fill() - 1,
            };

            var passwordTextField = new TextField("")
            {
                X = Pos.Right(passwordLablel) + 1,
                Y = Pos.Bottom(passwordLablel) - 1,
                Width = Dim.Fill() - 1,
                Secret = true
            };

            var frame = new FrameView()
            {
                X = Pos.Center(),
                Y = Pos.Center(),
                Width = 30,
                Height = 10,
                Border = new Border() { BorderStyle = BorderStyle.Rounded }
            };

            var loginButton = new Button("Login")
            {
                X = Pos.Center(),
                Y = Pos.AnchorEnd(1)
            };

            loginButton.Clicked += async () => {
                var login = loginTextField.Text.ToString();
                var password = passwordTextField.Text.ToString();

                if (string.IsNullOrEmpty(login))
                {
                    MessageBox.Query("Error", "Login is empty", "Ok");
                    return;
                }

                if (string.IsNullOrEmpty(password))
                {
                    MessageBox.Query("Error", "Password is empty", "Ok");
                    return;
                }

                if(logInCommand != null)
                {
                    try
                    {
                        await logInCommand.Execute(login, password);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Query("Login error", e.Message, "Ok");
                    }
                }

                loginTextField.Text = string.Empty;
                passwordTextField.Text = string.Empty;
            };

            var goToRegister = new Button("Don`t have an account? Register")
            {
                X = Pos.Center(),
                Y = Pos.Bottom(frame)
            };

            goToRegister.Clicked += () => openRegitserWindow?.Execute();

            frame.Add(loginLablel, passwordLablel, loginTextField, passwordTextField, loginButton);

            Add(frame, goToRegister);
        }
    }
}