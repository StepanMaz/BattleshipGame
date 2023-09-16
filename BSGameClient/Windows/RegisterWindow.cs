using BattleShipGameClient.Commands;
using System.Diagnostics;
using Terminal.Gui;

namespace BattleShipGameClient.Windows;

public class RegisterWindow : Window
{
    private IResultCommand<string, string, Task>? registerCommand;
    private ICommand? openLoginWindow;
    public RegisterWindow()
    {
        InitComponent();
    }

    public void SetRegister(IResultCommand<string, string, Task> registerCommand)
    {
        this.registerCommand = registerCommand;
    }

    public void SetOpenLogin(ICommand openLoginWindow)
    {
        this.openLoginWindow = openLoginWindow;
    }

    private void InitComponent()
    {
        Title = "Registration";

        var loginLablel = new Label() {
            Text = "Login:",
            X = 1,
            Y = 1
        };

        var loginTextField = new TextField("")
        {
            X = Pos.Right(loginLablel) + 1,
            Y = Pos.Top(loginLablel),
            Width = Dim.Fill() - 1
        };

        var passwordLablel = new Label() {
            Text = "Password:",
            X = Pos.Left(loginLablel),
            Y = Pos.Bottom(loginLablel) + 2
        };

        var passwordTextField = new TextField("")
        {
            X = Pos.Right(passwordLablel) + 1,
            Y = Pos.Bottom(passwordLablel) - 1,
            Width = Dim.Fill() - 1,
            Secret = true
        };

        var confirmPasswordLablel = new Label()
        {
            Text = "Confirm:",
            X = Pos.Left(passwordLablel),
            Y = Pos.Bottom(passwordLablel) + 2
        };

        var confirmPasswordTextField = new TextField("")
        {
            X = Pos.Right(confirmPasswordLablel) + 2,
            Y = Pos.Bottom(confirmPasswordLablel) - 1,
            Width = Dim.Fill() - 1,
            Secret = true
        };

        var frame = new FrameView()
        {
            X = Pos.Center(),
            Y = Pos.Center(),
            Width = 30,
            Height = 12,
            Border = new Border() { BorderStyle = BorderStyle.Rounded }
        };

        var loginButton = new Button("Register")
        {
            X = Pos.Center(),
            Y = Pos.AnchorEnd(1),
        };

        loginButton.Clicked += async () => {
            var login = loginTextField.Text.ToString();
            var password = passwordTextField.Text.ToString();
            var confirmPassword = confirmPasswordTextField.Text.ToString();

            if (string.IsNullOrEmpty(login)) {
                MessageBox.Query("Error", "Login is empty", "Ok");
                return;
            }

            if(string.IsNullOrEmpty(password)) {
                MessageBox.Query("Error", "Password is empty", "Ok");
                return;
            }

            if (confirmPassword != password) {
                MessageBox.Query("Error", "Passwords do not match", "Ok");
                confirmPasswordTextField.Text = string.Empty;
                passwordTextField.Text = string.Empty;
                return;
            }

            if (registerCommand != null)
            {
                try
                {
                    await registerCommand.Execute(login, password);
                }
                catch (Exception e)
                {
                    MessageBox.Query("Registration error", e.Message, "Ok");
                }
            }

            loginTextField.Text = string.Empty;
            confirmPasswordTextField.Text = string.Empty;
            passwordTextField.Text = string.Empty;
        };

        var goToLogin = new Button("Already have an account? Login") {
            X = Pos.Center(),
            Y = Pos.Bottom(frame)
        };

        goToLogin.Clicked += () => openLoginWindow?.Execute();

        frame.Add(loginLablel, passwordLablel, loginTextField, passwordTextField, loginButton, confirmPasswordLablel, confirmPasswordTextField);

        Add(frame, goToLogin);
    }
}