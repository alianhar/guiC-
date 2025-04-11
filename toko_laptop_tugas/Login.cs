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

        SqlConnection Conn = new SqlConnection(DBConnection.ConnectionString);
        public static string EmpName { get; set; }
        public static string CustName { get; set; }
        public static int UserType { get; set; } // 1 for Employee, 2 for Customer
        public static int CustId { get; set; } // Tambahkan ini
        public static int EmpId { get; set; } // Tambahkan ini


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
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                // Cek login sebagai employee
                string employeeQuery = "SELECT EmpNum, EmpName FROM EmployeeTbl WHERE EmpName=@Username AND EmpPass=@Password";
                SqlCommand empCmd = new SqlCommand(employeeQuery, Conn);
                empCmd.Parameters.AddWithValue("@Username", UsernameTb.Text.Trim());
                empCmd.Parameters.AddWithValue("@Password", PasswordTb.Text.Trim());

                using (SqlDataReader reader = empCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        EmpId = Convert.ToInt32(reader["EmpNum"]);
                        EmpName = reader["EmpName"].ToString();
                        UserType = 1; // Employee
                        MessageBox.Show("Login Sukses sebagai Karyawan", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        reader.Close();

                        Home home = new Home();
                        home.Show();
                        this.Hide();
                        return;
                    }
                }

                // Cek login sebagai customer
                string customerQuery = "SELECT CustId, CustName FROM CustomerTbl WHERE CustUsername=@Username AND CustPass=@Password";
                SqlCommand custCmd = new SqlCommand(customerQuery, Conn);
                custCmd.Parameters.AddWithValue("@Username", UsernameTb.Text.Trim());
                custCmd.Parameters.AddWithValue("@Password", PasswordTb.Text.Trim());

                using (SqlDataReader reader = custCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        CustId = Convert.ToInt32(reader["CustId"]);
                        CustName = reader["CustName"].ToString();
                        UserType = 2; // Customer
                        MessageBox.Show("Login Sukses sebagai Customer", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        reader.Close();

                        HomeCust homeCust = new HomeCust();
                        homeCust.Show();
                        this.Hide();
                        return;
                    }
                }

                // Jika sampai di sini, login gagal
                MessageBox.Show("Username atau password salah", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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