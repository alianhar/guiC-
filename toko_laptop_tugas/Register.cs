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
        SqlConnection Conn = new SqlConnection(@"Data Source=DESKTOP-UB2KSKP\SQLEXPRESS;Initial Catalog=db_toko_laptop_tugas;Integrated Security=True");
        private void btnRegister_Click(object sender, EventArgs e)
        {
            // Pastikan semua field wajib terisi
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

                // Cek apakah username sudah ada
                string checkUserQuery = "SELECT COUNT(*) FROM EmployeeTbl WHERE EmpName = @username";
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

                // Jika belum ada, lakukan insert data ke tabel EmployeeTbl
                string insertQuery = @"INSERT INTO EmployeeTbl (EmpName, EmpPhone, EmpPass, EmpAdd, EmpDOB)
                                       VALUES (@username, @phone, @password, @address, @dob)";

                using (SqlCommand insertCmd = new SqlCommand(insertQuery, Conn))
                {
                    insertCmd.Parameters.AddWithValue("@username", UsernameTb.Text.Trim());
                    insertCmd.Parameters.AddWithValue("@phone", PhoneTb.Text.Trim());
                    insertCmd.Parameters.AddWithValue("@password", PasswordTb.Text.Trim());
                    // Set nilai default
                    insertCmd.Parameters.AddWithValue("@address", "Belum Disetel");
                    insertCmd.Parameters.AddWithValue("@dob", DateTime.Today); // Set tanggal lahir ke hari ini

                    int rowsAffected = insertCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Registrasi berhasil! Anda akan diarahkan ke halaman login.");
                        // Setelah registrasi, tampilkan halaman login
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
