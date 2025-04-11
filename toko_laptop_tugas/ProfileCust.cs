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

namespace toko_laptop_tugas
{
    public partial class ProfileCust : Form
    {
        public ProfileCust()
        {
            InitializeComponent();
            EmpNameLbl.Text = Login.CustName;
            LoadProfileData(); // Memuat data profil pengguna
        }
        //koneksi
        SqlConnection Conn = new SqlConnection(DBConnection.ConnectionString);

        private void LoadProfileData()
        {
            try
            {
                // Pastikan koneksi dalam keadaan tertutup sebelum membuka
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                // Query untuk mengambil data pengguna berdasarkan nama pengguna
                string query = "SELECT CustName, CustPhone, CustAdd FROM CustomerTbl WHERE CustName=@CustName";
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@CustName", Login.CustName);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // Mengisi TextBox dengan data yang diambil
                    UsernameTb.Text = reader["CustName"].ToString();
                    PhoneTb.Text = reader["CustPhone"].ToString();
                    AddressTb.Text = reader["CustAdd"].ToString();
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

        private void btnEditProfile_Click(object sender, EventArgs e)
        {
            // Pastikan semua field tidak kosong (kecuali Address opsional)
            if (UsernameTb.Text.Trim() == "" || PhoneTb.Text.Trim() == "")
            {
                MessageBox.Show("Username dan Nomor HP wajib diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Pastikan koneksi dalam keadaan tertutup sebelum membuka
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                // Query UPDATE untuk mengubah data pengguna
                string updateQuery = @"UPDATE CustomerTbl 
                               SET CustPhone = @phone, 
                                   CustAdd = @address
                               WHERE CustName = @username";

                using (SqlCommand cmd = new SqlCommand(updateQuery, Conn))
                {
                    cmd.Parameters.AddWithValue("@username", UsernameTb.Text.Trim()); // Username tidak boleh berubah
                    cmd.Parameters.AddWithValue("@phone", PhoneTb.Text.Trim());
                    cmd.Parameters.AddWithValue("@address", AddressTb.Text.Trim() == "" ? "Belum Disetel" : AddressTb.Text.Trim());

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

        //route halaman
        private void HomeBtn_Click(object sender, EventArgs e)
        {
            HomeCust homeCustProfile = new HomeCust();
            homeCustProfile.Show();
            this.Hide();
        }

        private void ProdukBtn_Click(object sender, EventArgs e)
        {
            ProdukCust produkCustProfile = new ProdukCust();
            produkCustProfile.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TransaksiCust transaksiCustProfile = new TransaksiCust();
            transaksiCustProfile.Show();
            this.Hide();
        }

        private void BillingBtn_Click(object sender, EventArgs e)
        {
            DeliveryCust deliveryCustProfile = new DeliveryCust();
            deliveryCustProfile.Show();
            this.Hide();
        }

        private void SignOutBtn_Click(object sender, EventArgs e)
        {
            // Kosongkan variabel static agar tidak menyimpan sesi login sebelumnya
            Login.CustName = null;

            // Tampilkan form login kembali
            Login loginForm = new Login();
            loginForm.Show();

            // Tutup form Home
            this.Close();
        }

        
    }
}
