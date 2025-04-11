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
    public partial class Delivery : Form
    {
        public Delivery()
        {
            InitializeComponent();
            EmpNameLbl.Text = Login.EmpName;
            DisplayCustomers();

            CustomerTb.ReadOnly = true;
            ProdukTb.ReadOnly = true;
            QtyTb.ReadOnly = true;
            AlamatTb.ReadOnly = true;
            TotalTb.ReadOnly = true;
        }

        SqlConnection Conn = new SqlConnection(DBConnection.ConnectionString);



        int selectedBillId = -1;


        private void DisplayDeliveryByCustomer(int custId)
        {
            try
            {
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                string query = @"
                    SELECT 
                        dt.TrackingId,
                        e.EmpName AS [Diupdate Oleh],
                        b.BNum AS [No. Transaksi],
                        b.BDate AS [Tanggal],
                        b.CustName AS [Customer],
                        p.PrName AS [Nama Produk],
                        bd.Quantity AS [Jumlah],
                        b.Amt AS [Total Bayar],
                        b.DeliveryAddress AS [Alamat],
                        b.DeliveryStatus,
                        b.DeliveryFee,
                        dt.Notes AS [Catatan]
                    FROM 
                        BillTbl b
                    INNER JOIN 
                        BillDetailTbl bd ON b.BNum = bd.BillId
                    INNER JOIN 
                        ProductTbl p ON bd.ProductId = p.PrId
                    LEFT JOIN 
                        DeliveryTrackingTbl dt ON dt.BillId = b.BNum
                    LEFT JOIN 
                        EmployeeTbl e ON dt.UpdatedBy = e.EmpNum
                    WHERE 
                        b.CustId = @CustId AND b.PaymentStatus = 'Verified'
                    ORDER BY 
                        b.BDate DESC
                    ";

                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@CustId", custId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                DeliveryDGV.DataSource = dt;
                DeliveryDGV.Columns["TrackingId"].Visible = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data delivery: " + ex.Message);
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
            }
        }


        private void DisplayCustomers()
        {
            try
            {
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                string query = @"
            SELECT 
                CustId, 
                CustName AS [Nama Customer], 
                CustPhone AS [No. Telepon], 
                CustAdd AS [Alamat]
            FROM 
                CustomerTbl
            ORDER BY 
                CustName ASC";

                SqlDataAdapter da = new SqlDataAdapter(query, Conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                CustomersDGV.DataSource = dt;

                // Optional: Sembunyikan CustId kalau tidak perlu
                CustomersDGV.Columns["CustId"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data customer: " + ex.Message);
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
            Home homeDelivery = new Home();
            homeDelivery.Show();
            this.Hide();
        }

        private void btnMenuProduk_Click(object sender, EventArgs e)
        {
            Produk produkDelivery = new Produk();
            produkDelivery.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Pegawai pegawaiDelivery = new Pegawai();
            pegawaiDelivery.Show();
            this.Hide();
        }

        private void btnMenuPelanggan_Click(object sender, EventArgs e)
        {
            Customer customerDelivery = new Customer();
            customerDelivery.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Billing billingDelivery = new Billing();
            billingDelivery.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Transaksi transaksiDelivery = new Transaksi();
            transaksiDelivery.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Delivery deliveryDelivery = new Delivery();
            deliveryDelivery.Show();
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
            Profile profileDelivery = new Profile();
            profileDelivery.Show();
            this.Hide();
        }

        private void CustomersDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = CustomersDGV.Rows[e.RowIndex];
                int selectedCustId = Convert.ToInt32(selectedRow.Cells["CustId"].Value);

                // Simpan alamat customer juga kalau perlu
                string alamatCustomer = selectedRow.Cells["Alamat"].Value?.ToString();
                string namaCustomer = selectedRow.Cells["Nama Customer"].Value?.ToString();
                AlamatTb.Text = alamatCustomer;
                CustomerTb.Text = namaCustomer;

                // Panggil fungsi untuk tampilkan delivery-nya
                DisplayDeliveryByCustomer(selectedCustId);
            }
        }

        private void DeliveryDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = DeliveryDGV.Rows[e.RowIndex];

                selectedBillId = Convert.ToInt32(selectedRow.Cells["No. Transaksi"].Value); // pastikan ini kolom BNum
                ProdukTb.Text = selectedRow.Cells["Nama Produk"].Value?.ToString();
                QtyTb.Text = selectedRow.Cells["Jumlah"].Value?.ToString();
                TotalTb.Text = selectedRow.Cells["Total Bayar"].Value?.ToString();
                AlamatTb.Text = selectedRow.Cells["Alamat"].Value?.ToString();
                StatusDeliveryCmb.SelectedItem = selectedRow.Cells["DeliveryStatus"].Value?.ToString();
                DeliveryFeeTb.Text = selectedRow.Cells["DeliveryFee"].Value?.ToString();
                DeliveryNotesTb.Text = selectedRow.Cells["Catatan"].Value?.ToString();
            }
        }
        private int GetSelectedCustomerId()
        {
            if (CustomersDGV.SelectedRows.Count > 0)
            {
                return Convert.ToInt32(CustomersDGV.SelectedRows[0].Cells["CustId"].Value);
            }
            return -1;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (selectedBillId == -1)
            {
                MessageBox.Show("Silakan pilih data delivery terlebih dahulu.");
                return;
            }

            try
            {
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                // Cek apakah data tracking sudah ada
                string checkQuery = "SELECT COUNT(*) FROM DeliveryTrackingTbl WHERE BillId = @BillId";
                SqlCommand checkCmd = new SqlCommand(checkQuery, Conn);
                checkCmd.Parameters.AddWithValue("@BillId", selectedBillId);
                int count = (int)checkCmd.ExecuteScalar();

                string deliveryStatus = StatusDeliveryCmb.SelectedItem?.ToString() ?? "Pending";
                string deliveryNotes = DeliveryNotesTb.Text.Trim();
                int deliveryFee = int.TryParse(DeliveryFeeTb.Text.Trim(), out int fee) ? fee : 0;
                int updatedBy = Login.EmpId; // Pastikan Login.EmpId tersedia
                DateTime updateDate = DateTime.Now;

                if (count == 0)
                {
                    // Insert baru ke DeliveryTrackingTbl
                    string insertTracking = @"
                INSERT INTO DeliveryTrackingTbl (BillId, Status, UpdateDate, UpdatedBy, Notes)
                VALUES (@BillId, @Status, @UpdateDate, @UpdatedBy, @Notes)";
                    SqlCommand insertCmd = new SqlCommand(insertTracking, Conn);
                    insertCmd.Parameters.AddWithValue("@BillId", selectedBillId);
                    insertCmd.Parameters.AddWithValue("@Status", deliveryStatus);
                    insertCmd.Parameters.AddWithValue("@UpdateDate", updateDate);
                    insertCmd.Parameters.AddWithValue("@UpdatedBy", updatedBy);
                    insertCmd.Parameters.AddWithValue("@Notes", deliveryNotes);
                    insertCmd.ExecuteNonQuery();
                }
                else
                {
                    // Update existing tracking
                    string updateTracking = @"
                UPDATE DeliveryTrackingTbl
                SET Status = @Status, UpdateDate = @UpdateDate, UpdatedBy = @UpdatedBy, Notes = @Notes
                WHERE BillId = @BillId";
                    SqlCommand updateCmd = new SqlCommand(updateTracking, Conn);
                    updateCmd.Parameters.AddWithValue("@BillId", selectedBillId);
                    updateCmd.Parameters.AddWithValue("@Status", deliveryStatus);
                    updateCmd.Parameters.AddWithValue("@UpdateDate", updateDate);
                    updateCmd.Parameters.AddWithValue("@UpdatedBy", updatedBy);
                    updateCmd.Parameters.AddWithValue("@Notes", deliveryNotes);
                    updateCmd.ExecuteNonQuery();
                }

                // Update DeliveryFee dan Status ke BillTbl
                string updateBill = @"
            UPDATE BillTbl
            SET DeliveryFee = @DeliveryFee, DeliveryStatus = @Status
            WHERE BNum = @BillId";
                SqlCommand updateBillCmd = new SqlCommand(updateBill, Conn);
                updateBillCmd.Parameters.AddWithValue("@BillId", selectedBillId);
                updateBillCmd.Parameters.AddWithValue("@DeliveryFee", deliveryFee);
                updateBillCmd.Parameters.AddWithValue("@Status", deliveryStatus);
                updateBillCmd.ExecuteNonQuery();

                MessageBox.Show("Data delivery berhasil disimpan/diupdate.");

                // Refresh data jika perlu
                DisplayDeliveryByCustomer(GetSelectedCustomerId());

            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan saat menyimpan data: " + ex.Message);
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (DeliveryDGV.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Apakah kamu yakin ingin menghapus data pengiriman ini?",
                                                      "Konfirmasi", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        if (Conn.State == ConnectionState.Closed)
                            Conn.Open();

                        object trackIdObj = DeliveryDGV.SelectedRows[0].Cells["TrackingId"].Value;

                        if (trackIdObj == DBNull.Value)
                        {
                            MessageBox.Show("Data ini belum memiliki ID pengiriman,tambah catatan, delivery fee atau ubah status delivery terlebih dahulu agar bisa menghapus!");
                            return;
                        }

                        int deliveryId = Convert.ToInt32(trackIdObj);

                        string query = "DELETE FROM DeliveryTrackingTbl WHERE TrackingId = @id";
                        SqlCommand cmd = new SqlCommand(query, Conn);
                        cmd.Parameters.AddWithValue("@id", deliveryId);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Data pengiriman berhasil dihapus!");

                        // Refresh tabel setelah hapus
                        DisplayDeliveryByCustomer(GetSelectedCustomerId());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Gagal menghapus data pengiriman: " + ex.Message);
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
                MessageBox.Show("Silakan pilih data pengiriman yang ingin dihapus.");
            }
        }


    }
}
