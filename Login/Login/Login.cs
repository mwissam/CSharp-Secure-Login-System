using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Login
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text.Trim();
            string enteredPassword = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(enteredPassword))
            {
                MessageBox.Show("Please Enter Your UserName Or Password");
                return;
            }

            using (SqlConnection cn = DatabaseHelper.CreateConnection())
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("SelecttUsers", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Username", username);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string storedHash = reader["Password"].ToString();
                    string storedSalt = reader["Salt"].ToString();
                    string role = reader["Security"].ToString();
                    string id = reader["Id"].ToString();

                    bool isValid = EncryptionHelper.PasswordHelper.VerifyPassword(enteredPassword, storedHash, storedSalt);

                    if (isValid)
                    {
                        MainForm frm = new MainForm();
                        frm.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("كلمة المرور غير صحيحة");
                    }
                }
                else
                {
                    MessageBox.Show("اسم المستخدم غير موجود");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Register frm = new Register();
            frm.ShowDialog();
        }
    }
}
