using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace toko_laptop_tugas
{
    public partial class TransaksiCust : Form
    {
        public TransaksiCust()
        {
            InitializeComponent();
            CustNameLbl.Text = Login.CustName;
            DisplayTransactions();
            dataGridView1.CellClick += dataGridView1_CellClick_1;


        }

        SqlConnection Conn = new SqlConnection(DBConnection.ConnectionString);


        public void DisplayTransactions()
        {
            try
            {
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                string query = @"
            SELECT 
                b.BNum AS [No. Transaksi], 
                b.BDate AS [Tanggal], 
                b.CustName AS [Nama Customer],
                p.PrName AS [Nama Produk],
                p.PrPrice AS [Harga Produk],   
                bd.Quantity AS [Jumlah], 
                b.Amt AS [Total Bayar], 
                b.DeliveryAddress AS [Alamat Pengiriman], 
                b.PaymentProof AS [Bukti Pembayaran],
                b.PaymentStatus AS [Payment Status]
            FROM 
                BillTbl b
            JOIN 
                BillDetailTbl bd ON b.BNum = bd.BillId
            JOIN 
                ProductTbl p ON bd.ProductId = p.PrId
            WHERE 
                b.CustId = @CustId
            ORDER BY 
                b.BDate DESC";

                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@CustId", Login.CustId); // ambil dari sesi login

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // ⬇️ Tampilkan ke DataGridView1
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menampilkan riwayat transaksi: " + ex.Message);
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
            }
        }

        string selectedBillId = ""; // global di class (di atas agar bisa diakses di event lain)
        string selectedProofPath = "";


        //route halaman
        private void HomeBtn_Click(object sender, EventArgs e)
        {
            HomeCust homeCustTransaksi = new HomeCust();
            homeCustTransaksi.Show();
            this.Hide();
        }

        private void ProdukBtn_Click(object sender, EventArgs e)
        {
            ProdukCust produkCustTransaksi = new ProdukCust();
            produkCustTransaksi.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TransaksiCust transaksiCustTransaksi = new TransaksiCust();
            transaksiCustTransaksi.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DeliveryCust deliveryCustTransaksi = new DeliveryCust();
            deliveryCustTransaksi.Show();
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

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            ProfileCust profileCustTransaksi = new ProfileCust();
            profileCustTransaksi.Show();
            this.Hide();
        }

        private void btnAddPaymentProof_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedBillId))
            {
                MessageBox.Show("Silakan pilih transaksi terlebih dahulu.");
                return;
            }

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                openFileDialog.Title = "Pilih Bukti Pembayaran";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    string folderPath = Path.Combine(Application.StartupPath, "BuktiTransfer");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string newFileName = "bukti_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".jpg";
                    string newPath = Path.Combine(folderPath, newFileName);

                    // Simpan gambar ke folder aplikasi
                    File.Copy(filePath, newPath, true);

                    try
                    {
                        if (Conn.State == ConnectionState.Closed)
                            Conn.Open();

                        string updateQuery = "UPDATE BillTbl SET PaymentProof = @proofPath WHERE BNum = @billId";
                        SqlCommand cmd = new SqlCommand(updateQuery, Conn);
                        cmd.Parameters.AddWithValue("@proofPath", newPath);
                        cmd.Parameters.AddWithValue("@billId", selectedBillId);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Bukti pembayaran berhasil ditambahkan!");

                        // Refresh preview dan DataGridView
                        pictureBox4.Image = Image.FromFile(newPath);
                        DisplayTransactions(); // untuk refresh DGV
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Gagal mengunggah bukti: " + ex.Message);
                    }
                    finally
                    {
                        if (Conn.State == ConnectionState.Open)
                            Conn.Close();
                    }
                }
            }
        }

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                // Ambil ID dan path
                selectedBillId = selectedRow.Cells["No. Transaksi"].Value?.ToString();
                selectedProofPath = selectedRow.Cells["Bukti Pembayaran"].Value?.ToString();

                // Preview gambar
                if (!string.IsNullOrEmpty(selectedProofPath) && File.Exists(selectedProofPath))
                {
                    using (var bmpTemp = new Bitmap(selectedProofPath))
                    {
                        pictureBox4.Image = new Bitmap(bmpTemp);
                    }
                }
                else
                {
                    pictureBox4.Image = null;
                    // opsional: beri info
                }

                // Isi TextBox
                PrNameTb.Text = selectedRow.Cells["Nama Produk"].Value?.ToString();
                PrPriceTb.Text = selectedRow.Cells["Harga Produk"].Value?.ToString(); // setelah kamu tambahkan di SQL
                QtyTb.Text = selectedRow.Cells["Jumlah"].Value?.ToString();
                TotalAmtTb.Text = selectedRow.Cells["Total Bayar"].Value?.ToString();
                paymentStatusTb.Text = selectedRow.Cells["Payment Status"].Value?.ToString();
            }
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
