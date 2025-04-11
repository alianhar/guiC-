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
    public partial class Profile : Form
    {

        public Profile()
        {
            InitializeComponent();
            EmpNameLbl.Text = Login.EmpName; // Menampilkan nama pengguna yang login
            LoadProfileData(); // Memuat data profil pengguna
        }

        SqlConnection Conn = new SqlConnection(DBConnection.ConnectionString);
        private void LoadProfileData()
        {
            try
            {
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                string query = "SELECT EmpName, EmpPhone, EmpAdd, EmpDOB FROM EmployeeTbl WHERE EmpNum=@EmpNum";
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@EmpNum", Login.EmpId);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    NameTb.Text = reader["EmpName"].ToString();
                    PhoneTb.Text = reader["EmpPhone"].ToString();
                    AddressTb.Text = reader["EmpAdd"].ToString();
                    TglLahirDTP.Value = Convert.ToDateTime(reader["EmpDOB"]);
                }
                else
                {
                    MessageBox.Show("Data profil tidak ditemukan.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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


        // Event handler untuk tombol-tombol navigasi
        private void HomeBtn_Click(object sender, EventArgs e)
        {
            Home homeProfile = new Home();
            homeProfile.Show();
            this.Hide();
        }

        private void ProdukBtn_Click(object sender, EventArgs e)
        {
            Produk produkProfile = new Produk();
            produkProfile.Show();
            this.Hide();
        }

        private void PegawaiBtn_Click(object sender, EventArgs e)
        {
            Pegawai pegawaiProfile = new Pegawai();
            pegawaiProfile.Show();
            this.Hide();
        }

        private void PelangganBtn_Click(object sender, EventArgs e)
        {
            Customer pelangganProfile = new Customer();
            pelangganProfile.Show();
            this.Hide();
        }

        private void BillingBtn_Click(object sender, EventArgs e)
        {
            Billing billingProfile = new Billing();
            billingProfile.Show();
            this.Hide();
        }

        private void SignOutBtn_Click(object sender, EventArgs e)
        {
            // Kosongkan variabel static agar tidak menyimpan sesi login sebelumnya
            Login.EmpName = null;

            // Tampilkan form login kembali
            Login loginForm = new Login();
            loginForm.Show();

            // Tutup form Home
            this.Close();
        }

        private void btnEditProfile_Click(object sender, EventArgs e)
        {
            if (PhoneTb.Text.Trim() == "")
            {
                MessageBox.Show("Nomor HP wajib diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                string updateQuery = @"UPDATE EmployeeTbl 
                               SET EmpPhone = @phone, 
                                   EmpAdd = @address, 
                                   EmpDOB = @dob 
                               WHERE EmpNum = @empId";

                using (SqlCommand cmd = new SqlCommand(updateQuery, Conn))
                {
                    cmd.Parameters.AddWithValue("@empId", Login.EmpId);
                    cmd.Parameters.AddWithValue("@phone", PhoneTb.Text.Trim());
                    cmd.Parameters.AddWithValue("@address", AddressTb.Text.Trim() == "" ? "Belum Disetel" : AddressTb.Text.Trim());
                    cmd.Parameters.AddWithValue("@dob", TglLahirDTP.Value);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Profil berhasil diperbarui!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Gagal memperbarui profil. Pastikan data benar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
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


        private void button8_Click(object sender, EventArgs e)
        {
            Transaksi transaksiProfile = new Transaksi();
            transaksiProfile.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Delivery deliveryProfile = new Delivery();
            deliveryProfile.Show();
            this.Hide();
        }
    }

}
