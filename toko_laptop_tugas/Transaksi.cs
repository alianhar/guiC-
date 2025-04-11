using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using System.Windows.Forms;

namespace toko_laptop_tugas
{
    public partial class Transaksi : Form
    {
        string selectedCustomerAddress = "";

        public Transaksi()
        {
            InitializeComponent();
            EmpNameLbl.Text = Login.EmpName;
            DisplayCustomers();

            CustomerTb.ReadOnly = true;
            ProdukNameTb.ReadOnly = true;
            QtyTb.ReadOnly = true;
            CustAddTb.ReadOnly = true;
            TotalHargaTb.ReadOnly = true;
            PaymentStatusCmb.DropDownStyle = ComboBoxStyle.DropDownList;

        }

        SqlConnection Conn = new SqlConnection(DBConnection.ConnectionString);

        private void DisplayCustomers()
        {
            try
            {
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                string query = @"
            SELECT 
                CustId, 
                CustName AS [Nama], 
                CustPhone AS [No. Telepon], 
                CustAdd AS [Alamat]
            FROM 
                CustomerTbl
            ORDER BY 
                CustName ASC";

                SqlCommand cmd = new SqlCommand(query, Conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                CustomerDGV.DataSource = dt;

                // Optional: Sembunyikan CustId agar tidak terlihat
                CustomerDGV.Columns["CustId"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat daftar customer: " + ex.Message);
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
            }
        }


        private void DisplayTransactionsByCustomer(int custId)
        {
            try
            {
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                string query = @"
            SELECT 
                b.BNum AS [No. Transaksi], 
                b.BDate AS [Tanggal], 
                b.CustName AS [Customer],
                p.PrName AS [Nama Produk], 
                bd.Quantity AS [Jumlah],
                b.Amt AS [Total Bayar],
                b.PaymentStatus,
                b.DeliveryStatus,
                b.PaymentProof AS [Bukti Pembayaran]
            FROM 
                BillTbl b
            INNER JOIN 
                BillDetailTbl bd ON b.BNum = bd.BillId
            INNER JOIN 
                ProductTbl p ON bd.ProductId = p.PrId
            WHERE 
                b.CustId = @CustId
            ORDER BY 
                b.BDate DESC";

                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@CustId", custId);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                TransaksiDGV.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat transaksi: " + ex.Message);
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
            }
        }








        //route halaman
        private void button1_Click(object sender, EventArgs e)
        {
            Home homeTransaksi = new Home();
            homeTransaksi.Show();
            this.Hide();
        }

        private void btnMenuProduk_Click(object sender, EventArgs e)
        {
            Produk produkTransaksi = new Produk();
            produkTransaksi.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Pegawai pegawaiTransaksi = new Pegawai();
            pegawaiTransaksi.Show();
            this.Hide();
        }

        private void btnMenuPelanggan_Click(object sender, EventArgs e)
        {
            Customer customerTransaksi = new Customer();
            customerTransaksi.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Billing billingTransaksi = new Billing();
            billingTransaksi.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Transaksi transaksiTransaksi = new Transaksi();
            transaksiTransaksi.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Delivery deliveryTransaksi = new Delivery();
            deliveryTransaksi.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Kosongkan variabel static agar tidak menyimpan sesi login sebelumnya
            Login.EmpName = null;

            // Tampilkan form login kembali
            Login loginForm = new Login();
            loginForm.Show();

            // Tutup form Home
            this.Close();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Profile profileTransaksi = new Profile();
            profileTransaksi.Show();
            this.Hide();
        }

        private int selectedCustomerRowIndex = -1;

        private void CustomerDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedCustomerRowIndex = e.RowIndex;
                int selectedCustId = Convert.ToInt32(CustomerDGV.Rows[e.RowIndex].Cells["CustId"].Value);

                // Ambil alamat customer dari DGV
                selectedCustomerAddress = CustomerDGV.Rows[e.RowIndex].Cells["Alamat"].Value.ToString();

                DisplayTransactionsByCustomer(selectedCustId);
            }
        }


        private int selectedTransactionRowIndex = -1;

        private void TransaksiDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                selectedTransactionRowIndex = e.RowIndex;

                DataGridViewRow row = TransaksiDGV.Rows[e.RowIndex];

                // Ambil nilai dari DataGridView dan tampilkan ke kontrol form
                CustomerTb.Text = row.Cells["Customer"].Value?.ToString(); // dulu kamu pakai "CustName"
                ProdukNameTb.Text = row.Cells["Nama Produk"].Value?.ToString();
                QtyTb.Text = row.Cells["Jumlah"].Value?.ToString();
                CustAddTb.Text = selectedCustomerAddress;
                PaymentStatusCmb.Text = row.Cells["PaymentStatus"].Value?.ToString();
                TotalHargaTb.Text = row.Cells["Total Bayar"].Value?.ToString();

                string proofPath = row.Cells["Bukti Pembayaran"].Value?.ToString();
                if (!string.IsNullOrEmpty(proofPath) && File.Exists(proofPath))
                {
                    using (var bmpTemp = new Bitmap(proofPath))
                    {
                        pictureBox9.Image = new Bitmap(bmpTemp);
                    }
                }
                else
                {
                    pictureBox9.Image = null;
                }
            }
        }

     
        private void btnUpdateTransaksi_Click(object sender, EventArgs e)
        {
            if (TransaksiDGV.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = TransaksiDGV.Rows[selectedTransactionRowIndex];
                // Ambil No. Transaksi dari DGV
                int transaksiId = Convert.ToInt32(TransaksiDGV.SelectedRows[0].Cells["No. Transaksi"].Value);
                string selectedStatus = PaymentStatusCmb.Text;

                if (string.IsNullOrEmpty(selectedStatus))
                {
                    MessageBox.Show("Silakan pilih status pembayaran terlebih dahulu.");
                    return;
                }

                try
                {
                    if (Conn.State == ConnectionState.Closed)
                        Conn.Open();

                    string query = "UPDATE BillTbl SET PaymentStatus = @status WHERE BNum = @id";
                    SqlCommand cmd = new SqlCommand(query, Conn);
                    cmd.Parameters.AddWithValue("@status", selectedStatus);
                    cmd.Parameters.AddWithValue("@id", transaksiId);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Status pembayaran berhasil diperbarui!");

                    // Refresh ulang transaksi customer
                    if (selectedCustomerRowIndex >= 0)
                    {
                        int custId = Convert.ToInt32(CustomerDGV.Rows[selectedCustomerRowIndex].Cells["CustId"].Value);
                        DisplayTransactionsByCustomer(custId);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal memperbarui status: " + ex.Message);
                }
                finally
                {
                    if (Conn.State == ConnectionState.Open)
                        Conn.Close();
                }
            }
            else
            {
                MessageBox.Show("Silakan klik salah satu transaksi terlebih dahulu.");
            }
        }

        private void btnHapusTransaksi_Click(object sender, EventArgs e)
        {
            if (selectedTransactionRowIndex >= 0)
            {
                DataGridViewRow selectedRow = TransaksiDGV.Rows[selectedTransactionRowIndex];
                int transaksiId = Convert.ToInt32(selectedRow.Cells["No. Transaksi"].Value);

                DialogResult result = MessageBox.Show("Apakah kamu yakin ingin menghapus transaksi ini?", "Konfirmasi", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        if (Conn.State == ConnectionState.Closed)
                            Conn.Open();

                        // Hapus dari BillDetailTbl dulu
                        string deleteDetailQuery = "DELETE FROM BillDetailTbl WHERE BillId = @id";
                        SqlCommand cmdDetail = new SqlCommand(deleteDetailQuery, Conn);
                        cmdDetail.Parameters.AddWithValue("@id", transaksiId);
                        cmdDetail.ExecuteNonQuery();

                        // Hapus dari BillTbl
                        string deleteBillQuery = "DELETE FROM BillTbl WHERE BNum = @id";
                        SqlCommand cmdBill = new SqlCommand(deleteBillQuery, Conn);
                        cmdBill.Parameters.AddWithValue("@id", transaksiId);
                        cmdBill.ExecuteNonQuery();

                        MessageBox.Show("Transaksi berhasil dihapus!");

                        // Refresh ulang TransaksiDGV
                        if (selectedCustomerRowIndex >= 0)
                        {
                            TransaksiDGV.DataSource = null; // optional, bikin tampilan lebih bersih dulu
                            int custId = Convert.ToInt32(CustomerDGV.Rows[selectedCustomerRowIndex].Cells["CustId"].Value);
                            DisplayTransactionsByCustomer(custId);
                        }



                        // Reset form
                        selectedTransactionRowIndex = -1;
                        ClearTransaksiFields();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Gagal menghapus transaksi: " + ex.Message);
                    }
                    finally
                    {
                        if (Conn.State == ConnectionState.Open)
                            Conn.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Silakan pilih transaksi terlebih dahulu.");
            }
        }

        private void ClearTransaksiFields()
        {
            CustomerTb.Text = "";
            ProdukNameTb.Text = "";
            QtyTb.Text = "";
            CustAddTb.Text = "";
            PaymentStatusCmb.Text = "";
            TotalHargaTb.Text = "";
            pictureBox9.Image = null;
        }

    }
}
