using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace toko_laptop_tugas
{
    public partial class Register: Form
    {
        public Register()
        {
            InitializeComponent();
        }
        SqlConnection Conn = new SqlConnection(DBConnection.ConnectionString);
        private void btnRegister_Click(object sender, EventArgs e)
        {
            // Pastikan semua field wajib terisi (Username, Password, Confirm Password, Phone)
            if (UsernameTb.Text.Trim() == "" || PasswordTb.Text.Trim() == "" || ConfirmPassTb.Text.Trim() == "" || PhoneTb.Text.Trim() == "")
            {
                MessageBox.Show("Harap isi semua field yang diperlukan (Username, Password, Confirm Password, Phone).");
                return;
            }

            // Cek kecocokan Password dan Confirm Password
            if (PasswordTb.Text != ConfirmPassTb.Text)
            {
                MessageBox.Show("Password dan konfirmasi password tidak cocok!");
                return;
            }

            try
            {
                Conn.Open();

                // Cek apakah username sudah ada di CustomerTbl
                string checkUserQuery = "SELECT COUNT(*) FROM CustomerTbl WHERE CustUsername = @username";
                using (SqlCommand cmd = new SqlCommand(checkUserQuery, Conn))
                {
                    cmd.Parameters.AddWithValue("@username", UsernameTb.Text.Trim());
                    int userCount = Convert.ToInt32(cmd.ExecuteScalar());
                    if (userCount > 0)
                    {
                        MessageBox.Show("Username sudah terdaftar. Silakan pilih username lain.");
                        return;
                    }
                }

                // Insert data ke CustomerTbl
                string insertQuery = @"INSERT INTO CustomerTbl (CustName, CustUsername, CustPhone, CustPass, CustAdd)
                               VALUES (@custName, @username, @phone, @password, @address)";

                using (SqlCommand insertCmd = new SqlCommand(insertQuery, Conn))
                {
                    // Pada contoh ini, kita gunakan UsernameTb sebagai sumber untuk CustName dan CustUsername.
                    insertCmd.Parameters.AddWithValue("@custName", UsernameTb.Text.Trim());
                    insertCmd.Parameters.AddWithValue("@username", UsernameTb.Text.Trim());
                    insertCmd.Parameters.AddWithValue("@phone", PhoneTb.Text.Trim());
                    insertCmd.Parameters.AddWithValue("@password", PasswordTb.Text.Trim());
                    // Set nilai default untuk CustAdd
                    insertCmd.Parameters.AddWithValue("@address", "Belum Disetel");

                    int rowsAffected = insertCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Registrasi berhasil! Silahkan login.");
                        // Tampilkan halaman login setelah registrasi
                        Login login = new Login();
                        login.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Registrasi gagal, silakan coba lagi.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi error: " + ex.Message);
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }
    }
}
