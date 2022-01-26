using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using Pocket_Book.Login__Register;

namespace Pocket_Book
{
    public partial class FormPocketbookLogin : Form
    {

        bool usernameLoginValid = false;
        bool passwordLoginValid = false;
        string usernameLogin;
        string passwordLogin;

        bool usernameRegisterValid = false;
        bool passwordRegisterValid = false;
        bool passwordTwoRegisterValid = false;
        bool emailRegisterValid = false;
        string usernameRegister;
        string passwordRegister;
        string passwordTwoRegister;
        string emailRegister;

        string errorMessage;

        public FormPocketbookLogin()
        {
            InitializeComponent();
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------------
        //Continuation Handlers------------------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------------------------------------------

        private void LabelPocketbookForgotPass_Click(object sender, EventArgs e)
        {

        }

        private void ButtonPocketbookRegister_Click(object sender, EventArgs e)
        {
            if (usernameRegisterValid && passwordRegisterValid && passwordTwoRegisterValid && emailRegisterValid)
            {
                ErrorProviderClickRegister.SetError(ButtonPocketbookRegister, "");

                if (CheckAvailable())
                {
                    DB db = new DB();

                    db.openConnection();

                    MySqlCommand NewUser = new MySqlCommand("INSERT INTO users (`username`, `password`, `email`) VALUES (@username, @password, @email)", db.GetConnection());

                    NewUser.Parameters.Add("@username", MySqlDbType.VarChar).Value = usernameRegister;
                    NewUser.Parameters.Add("@password", MySqlDbType.VarChar).Value = passwordRegister;
                    NewUser.Parameters.Add("@email", MySqlDbType.VarChar).Value = emailRegister;

                    if (NewUser.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Account Created, Logging in...");
                    }
                    else
                    {
                        MessageBox.Show("Query Error...");
                    }
                }
            }
            else
            {
                ErrorProviderClickRegister.SetError(ButtonPocketbookRegister, "Check your inputs!");
            }
        }

        private void ButtonPocketbookLogin_Click(object sender, EventArgs e)
        {

            usernameLogin = TextBoxPocketbookEmailLogin.Text;
            passwordLogin = TextBoxPocketbookPasswordLogin.Text;

            DB db = new DB();

            db.openConnection();

            MySqlCommand Login = new MySqlCommand("SELECT * FROM users WHERE `password` = @password AND (`username` = @username OR `email` = @username)", db.GetConnection());

            Login.Parameters.Add("@username", MySqlDbType.VarChar).Value = usernameLogin;
            Login.Parameters.Add("@password", MySqlDbType.VarChar).Value = passwordLogin;

            MySqlDataReader dataReader = Login.ExecuteReader();

            if (dataReader.HasRows)
            {
                MessageBox.Show("Logging In...");
            }
            else
            {
                MessageBox.Show("Incorrect Credentials!");
            }

        }

        //---------------------------------------------------------------------------------------------------------------------------------------------------
        //Form manipulation----------------------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------------------------------------------


        private void TextBoxPocketbookPasswordLogin_OnIconRightClick(object sender, EventArgs e)
        {
            if (TextBoxPocketbookPasswordLogin.UseSystemPasswordChar == true)
            {
                TextBoxPocketbookPasswordLogin.IconRight = Image.FromFile("Resources\\ShowPassword.png");
                TextBoxPocketbookPasswordLogin.UseSystemPasswordChar = false;
                TextBoxPocketbookPasswordLogin.PasswordChar = char.MinValue;
            }
            else if (TextBoxPocketbookPasswordLogin.UseSystemPasswordChar == false)
            {
                TextBoxPocketbookPasswordLogin.IconRight = Image.FromFile("Resources\\HidePassword.png");
                TextBoxPocketbookPasswordLogin.UseSystemPasswordChar = true;
            }
        }

        private void ButtonPocketbookCloseApp_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ButtonPocketbookCloseApp_MouseHover(object sender, EventArgs e)
        {
            ButtonPocketbookCloseApp.Image = Image.FromFile("Resources\\CloseWindowHover.png");
        }

        private void ButtonPocketbookCloseApp_MouseLeave(object sender, EventArgs e)
        {
            ButtonPocketbookCloseApp.Image = Image.FromFile("Resources\\CloseWindow.png");
        }

        private void ButtonPocketbookMinimizeApp_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void ButtonPocketbookMinimizeApp_MouseHover(object sender, EventArgs e)
        {
            ButtonPocketbookMinimizeApp.Image = Image.FromFile("Resources\\MinimizeWindowHover.png");
        }

        private void ButtonPocketbookMinimizeApp_MouseLeave(object sender, EventArgs e)
        {
            ButtonPocketbookMinimizeApp.Image = Image.FromFile("Resources\\MinimizeWindow.png");
        }

        private void LabelPocketbookCreateAccount_Click(object sender, EventArgs e)
        {
            PanelPocketbookLogin.SendToBack();
        }

        private void LabelPocketbookLogin_Click(object sender, EventArgs e)
        {
            PanelPocketbookRegister.SendToBack();
        }

        private void TextBoxPocketbookPasswordRegister_OnIconRightClick(object sender, EventArgs e)
        {
            if (TextBoxPocketbookPasswordRegister.UseSystemPasswordChar == true)
            {
                TextBoxPocketbookPasswordRegister.IconRight = Image.FromFile("Resources\\ShowPassword.png");
                TextBoxPocketbookPasswordRegister.UseSystemPasswordChar = false;
                TextBoxPocketbookPasswordRegister.PasswordChar = char.MinValue;
            }
            else if (TextBoxPocketbookPasswordRegister.UseSystemPasswordChar == false)
            {
                TextBoxPocketbookPasswordRegister.IconRight = Image.FromFile("Resources\\HidePassword.png");
                TextBoxPocketbookPasswordRegister.UseSystemPasswordChar = true;
            }
        }

        private void TextBoxPocketbookPassword2Register_OnIconRightClick(object sender, EventArgs e)
        {
            if (TextBoxPocketbookPassword2Register.UseSystemPasswordChar == true)
            {
                TextBoxPocketbookPassword2Register.IconRight = Image.FromFile("Resources\\ShowPassword.png");
                TextBoxPocketbookPassword2Register.UseSystemPasswordChar = false;
                TextBoxPocketbookPassword2Register.PasswordChar = char.MinValue;
            }
            else if (TextBoxPocketbookPassword2Register.UseSystemPasswordChar == false)
            {
                TextBoxPocketbookPassword2Register.IconRight = Image.FromFile("Resources\\HidePassword.png");
                TextBoxPocketbookPassword2Register.UseSystemPasswordChar = true;
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------------
        //validation of register form -----------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------------------------------------------------------------------

        private void RegisterValidation()
        {

            //username validated
            usernameRegister = TextBoxPocketbookUsernameRegister.Text;

            if (string.IsNullOrWhiteSpace(usernameRegister))
            {
                errorMessage = "Username Should Not Be Empty!";
                ErrorProviderUsernameRegister.SetError(TextBoxPocketbookUsernameRegister, errorMessage);
                usernameRegisterValid = false;
            }
            else if (usernameRegister.Length < 8)
            {
                errorMessage = "Username Should Not Be Less Than 8 Characters!";
                ErrorProviderUsernameRegister.SetError(TextBoxPocketbookUsernameRegister, errorMessage);
                usernameRegisterValid = false;
            }
            else
            {
                errorMessage = "";
                ErrorProviderUsernameRegister.SetError(TextBoxPocketbookUsernameRegister, errorMessage);
                usernameRegisterValid = true;
            }

            //email validated

            emailRegister = TextboxPocketbookEmailRegister.Text;
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(emailRegister);

            if (!match.Success)
            {
                emailRegisterValid = false;
                errorMessage = "Enter A Valid Email Address!";
                ErrorProviderUsernameRegister.SetError(TextboxPocketbookEmailRegister, errorMessage);
            }
            else if (match.Success)
            {
                emailRegisterValid = true;
                errorMessage = "";
                ErrorProviderUsernameRegister.SetError(TextboxPocketbookEmailRegister, errorMessage);
            }

            //password validated
            passwordRegister = TextBoxPocketbookPasswordRegister.Text;

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMinimum8Chars = new Regex(@".{8,}");

            var isValidated = hasNumber.IsMatch(passwordRegister) && hasUpperChar.IsMatch(passwordRegister) && hasMinimum8Chars.IsMatch(passwordRegister);

            if (isValidated)
            {
                passwordRegisterValid = true;
                errorMessage = "";
                ErrorProviderPasswordRegister.SetError(TextBoxPocketbookPasswordRegister, errorMessage);
            }
            else
            {
                passwordRegisterValid = false;
                errorMessage = "Use Correct Format (Upper-Case, Lower-Case and over 8 characters)";
                ErrorProviderPasswordRegister.SetError(TextBoxPocketbookPasswordRegister, errorMessage);
            }

            //backup password validated
            passwordTwoRegister = TextBoxPocketbookPassword2Register.Text;

            if (passwordTwoRegister == passwordRegister)
            {
                passwordTwoRegisterValid = true;
                errorMessage = "";
                ErrorProviderPasswordTwoRegister.SetError(TextBoxPocketbookPassword2Register, errorMessage);
            }
            else
            {
                passwordTwoRegisterValid = false;
                errorMessage = "Passwords Should Match!";
                ErrorProviderPasswordTwoRegister.SetError(TextBoxPocketbookPassword2Register, errorMessage);
            }
            if (string.IsNullOrWhiteSpace(passwordTwoRegister))
            {
                passwordTwoRegisterValid = false;
                errorMessage = "Password Should Not Be Empty!";
                ErrorProviderPasswordTwoRegister.SetError(TextBoxPocketbookPassword2Register, errorMessage);
            }
        }

        private void TextBoxPocketbookUsernameRegister_Validating(object sender, CancelEventArgs e)
        {
            RegisterValidation();
        }

        private void TextboxPocketbookEmailRegister_Validating(object sender, CancelEventArgs e)
        {
            RegisterValidation();
        }

        private void TextBoxPocketbookPasswordRegister_Validating(object sender, CancelEventArgs e)
        {
            RegisterValidation();
        }

        private void TextBoxPocketbookPassword2Register_Validating(object sender, CancelEventArgs e)
        {
            RegisterValidation();
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------
        //text changed validation---------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------------------------

        private void TextBoxPocketbookUsernameRegister_TextChanged(object sender, EventArgs e)
        {
            RegisterValidation();
        }

        private void TextboxPocketbookEmailRegister_TextChanged(object sender, EventArgs e)
        {
            RegisterValidation();
        }

        private void TextBoxPocketbookPasswordRegister_TextChanged(object sender, EventArgs e)
        {
            RegisterValidation();
        }

        private void TextBoxPocketbookPassword2Register_TextChanged(object sender, EventArgs e)
        {
            RegisterValidation();
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------
        //Button Use Validation-----------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------------------------


        private bool CheckAvailable()
        {
            DB db = new DB();

            //open the connection
            db.openConnection();

            string query = "(SELECT * FROM users WHERE username ='" + usernameRegister + "' OR email ='" + emailRegister + "')";

            MySqlCommand cmd = new MySqlCommand(query, db.GetConnection());
            MySqlDataReader dataReader = cmd.ExecuteReader();

            if (dataReader.HasRows)
            {
                ErrorProviderClickRegister.SetError(ButtonPocketbookRegister, "Username/Email is Taken");
                usernameRegisterValid = false;
                RegisterValidation();
                return false;
            }
            else
            {
                ErrorProviderClickRegister.SetError(ButtonPocketbookRegister, "");
                usernameRegisterValid = true;
                RegisterValidation();
                return true;
            }

        }
    }
}
