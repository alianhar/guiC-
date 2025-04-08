using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace toko_laptop_tugas
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        SqlConnection Conn = new SqlConnection(@"Data Source=DESKTOP-PDGNVP8\SQLEXPRESS;Initial Catalog=db_toko_laptop_tugas;Integrated Security=True");

        public static string EmpName { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            // Validasi input
            if (UsernameTb.Text.Trim() == "" || PasswordTb.Text.Trim() == "")
            {
                MessageBox.Show("Username dan password harus diisi", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Pastikan koneksi dalam keadaan tertutup sebelum membuka
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                // Query untuk mengecek apakah ada data karyawan yang cocok dengan input
                string query = "SELECT EmpName FROM EmployeeTbl WHERE EmpName=@EmpName AND EmpPass=@EmpPass";
                SqlCommand cmd = new SqlCommand(query, Conn);

                // Gunakan string langsung tanpa konversi
                cmd.Parameters.AddWithValue("@EmpName", UsernameTb.Text);
                cmd.Parameters.AddWithValue("@EmpPass", PasswordTb.Text);

                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    // Simpan nama karyawan ke properti statis
                    EmpName = result.ToString();

                    MessageBox.Show("Login Sukses", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Home home = new Home();
                    home.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Username atau password salah", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error: " + Ex.Message);
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            Register regis = new Register();
            regis.Show();
            this.Hide();
        }
    }
}
