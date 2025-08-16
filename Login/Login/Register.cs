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
using System.Xml.Linq;

namespace Login
{
    public partial class Register : Form
    {
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter Da = new SqlDataAdapter();
        public Register()
        {
            InitializeComponent();
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            using (SqlConnection cn = DatabaseHelper.CreateConnection())
            {
                try
                {
                    if (string.IsNullOrEmpty(txtUserName.Text) || string.IsNullOrEmpty(txtPassword.Text))
                    {
                        MessageBox.Show("يرجى إدخال اسم المستخدم وكلمة المرور");
                        return;
                    }

                    // إنشاء Hash + Salt
                    EncryptionHelper.PasswordHelper.CreatePasswordHash(txtPassword.Text, out string hash, out string salt);

                    cn.Open();
                    cmd = new SqlCommand("InsertUsers", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", txtUserName.Text);
                    cmd.Parameters.AddWithValue("@Password", hash);   // نخزن الهاش
                    cmd.Parameters.AddWithValue("@Salt", salt);       // نخزن السالت
                    cmd.Parameters.AddWithValue("@Name", txtName.Text);
                    cmd.Parameters.AddWithValue("@Security", cmbSecurity.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("تم تسجيل المستخدم بنجاح");
                    txtUserName.Clear();
                    txtPassword.Clear();
                    txtName.Clear();
                    cmbSecurity.SelectedIndex = 0;
                    this.Close();
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 50000) // الكود الذي يظهر مع RAISERROR
                    {
                        MessageBox.Show("اسم المستخدم موجود مسبقًا. يرجى اختيار اسم آخر.");
                    }
                    else
                    {
                        MessageBox.Show("حدث خطأ: " + ex.Message);
                    }
                }
                finally
                {
                    cn.Close();
                }

            }
        }
    }
}
