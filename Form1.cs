
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Learning___Program
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // Устанавливаем фокус ввода на поле логина при запуске формы
            this.ActiveControl = txtLogin;
        }


        // Обработчик нажатия кнопки "Войти"
        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Получаем введенные логин и пароль
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Text;

            // Проверка на пустые поля
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // SQL-запрос для получения хэша пароля, соли и роли пользователя
                string query = "SELECT PasswordHash, Salt, Role FROM Users WHERE Username = @Username";
                SqlParameter[] parameters = { new SqlParameter("@Username", login) };

                // Выполняем запрос к базе данных
                DataTable result = DatabaseHelper.ExecuteQuery(query, parameters);

                // Проверяем, найден ли пользователь
                if (result.Rows.Count == 0)
                {
                    MessageBox.Show("Пользователь не найден!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Получаем данные из результата запроса
                string storedHash = result.Rows[0]["PasswordHash"].ToString(); // Хэш пароля из БД
                string salt = result.Rows[0]["Salt"].ToString(); // Соль для пароля
                string role = result.Rows[0]["Role"].ToString(); // Роль пользователя

                // Проверяем соответствие введенного пароля
                if (VerifyPassword(password, storedHash, salt))
                {
                    // Обновляем время последнего входа
                    UpdateLastLogin(login);

                    // Показываем сообщение об успешном входе
                    //MessageBox.Show("Вход выполнен успешно!", "Успех",
                    //    MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Открываем главную форму и передаем в нее логин и роль
                    MainForm mainForm = new MainForm(login, role);
                    mainForm.Show();
                    this.Hide(); // Скрываем текущую форму авторизации
                }
                else
                {
                    MessageBox.Show("Неверный пароль!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок при авторизации
                MessageBox.Show($"Ошибка при входе: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Обработчик кнопки "Регистрация"
        private void btnRegister_Click(object sender, EventArgs e)
        {
            // Добавьте в начало метода btnRegister_Click
            string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
            SqlParameter[] checkParams = { new SqlParameter("@Username", txtLogin.Text.Trim()) };
            int userCount = (int)DatabaseHelper.ExecuteScalar(checkQuery, checkParams);

            if (userCount > 0)
            {
                MessageBox.Show("Логин уже занят!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Открываем форму регистрации и скрываем текущую
            RegistrationForm registrationForm = new RegistrationForm();
            registrationForm.Show();
            this.Hide();
        }

        // Обработчик нажатия клавиш в поле пароля
        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            // Если нажата клавиша Enter - имитируем нажатие кнопки входа
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin.PerformClick();
            }
        }

        // Обработчик ссылки "Забыли пароль?"
        private void lnkRecover_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Открываем форму восстановления пароля и скрываем текущую
            PasswordRecoveryForm recoveryForm = new PasswordRecoveryForm();
            recoveryForm.Show();
            this.Hide();
        }

        // Метод для обновления времени последнего входа пользователя
        private void UpdateLastLogin(string username)
        {
            try
            {
                // SQL-запрос для обновления времени входа
                string query = "UPDATE Users SET LastLogin = GETDATE() WHERE Username = @Username";
                SqlParameter[] parameters = { new SqlParameter("@Username", username) };
                DatabaseHelper.ExecuteNonQuery(query, parameters);
            }
            catch (Exception ex)
            {
                // Логирование ошибки (в реальном приложении лучше использовать логгер)
                Console.WriteLine($"Ошибка при обновлении времени входа: {ex.Message}");
            }
        }
        // Метод для проверки пароля
        private bool VerifyPassword(string password, string storedHash, string salt)
        {
            try
            {
                if (string.IsNullOrEmpty(salt)) return false;
                byte[] saltBytes = Convert.FromBase64String(salt);

                using (var pbkdf2 = new Rfc2898DeriveBytes(
                    password,
                    saltBytes,
                    10000,
                    HashAlgorithmName.SHA256))
                {
                    byte[] hash = pbkdf2.GetBytes(32);
                    return Convert.ToBase64String(hash) == storedHash;
                }
            }
            catch
            {
                return false;
            }
        }
    }

}