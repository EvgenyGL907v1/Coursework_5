using PresenterContext;
using System;
using System.Windows.Forms;
using DatabaseManager;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ShippingCompanyManager
{
	public partial class LoginForm : Form
	{
		public LoginForm()
		{
			InitializeComponent();
			textBox2.PasswordChar = '*';

			button1_Click(this, EventArgs.Empty);

        }

		private void button1_Click(object sender, EventArgs e) //вход
		{
			string login = "admin";//textBox1.Text;
			string password = "admin";//textBox2.Text;


   //         if ((textBox1.Text == "") || (textBox2.Text == ""))
			//{
   //             MessageCaller.CallErrorMessage("Поля не могут быть пустыми");
			//	return;
   //         }

            try
			{
				SystemDataManager systemData = new SystemDataManager();
				int userId = systemData.UserAuthorization(login, password);

				UserData user = new UserData()
				{
					UserId = userId,
					Login = login,
				};

				MessageCaller.UserLogin(user);
				this.Hide();
			}
			catch (Exception ex)
			{
				MessageCaller.CallErrorMessage(ex.Message);
			}
		}

		private void button2_Click(object sender, EventArgs e) //рег
		{
			string login = textBox1.Text;
			string password = textBox2.Text;

            if ((textBox1.Text == "") || (textBox2.Text == ""))
            {
                MessageCaller.CallErrorMessage("Поля не могут быть пустыми");
                return;
            }

            if (textBox1.Text.Length < 3)
            {
                MessageCaller.CallErrorMessage("Длина логина не должна быть короче трех симвлов");
                return;
            }

            if (textBox2.Text.Length < 3)
            {
                MessageCaller.CallErrorMessage("Длина пароля не должна быть короче трех симвлов");
                return;
            }

            try
			{
                SystemDataManager systemData = new SystemDataManager();

                systemData.UserRegistration(login, password);
                MessageCaller.CallInfomationMessage("Пользователь успешно зарегистрирован");
            }
            catch (Exception ex)
            {
                MessageCaller.CallErrorMessage(ex.Message);
            }
        }

		private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			DatabaseConnection.Instance.CloseConnect();
			Environment.Exit(0);
		}
	}
}
