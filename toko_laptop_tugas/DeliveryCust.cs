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
    public partial class DeliveryCust : Form
    {
        public DeliveryCust()
        {
            InitializeComponent();
            CustNameLbl.Text = Login.CustName;
            loggedInCustomerId = Login.CustId;
            DisplayAllDelivery();
        }

        SqlConnection Conn = new SqlConnection(DBConnection.ConnectionString);

        private int loggedInCustomerId = 0; // ini di-set setelah login berhasil

        private void DisplayDeliveryByStatus(string status)
        {
            try
            {
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                string query = @"
                    SELECT 
                        dt.TrackingId,
                        e.EmpName AS [Diperbarui Oleh],
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
                        b.CustId = @CustId 
                        AND b.PaymentStatus = 'verified'
                        AND b.DeliveryStatus = @Status
                    ORDER BY 
                        b.BDate DESC";

                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@CustId", loggedInCustomerId); // dari variabel login
                cmd.Parameters.AddWithValue("@Status", status);

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


        private void DisplayAllDelivery()
        {
            try
            {
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                string query = @"
                SELECT 
                    dt.TrackingId,
                    e.EmpName AS [Diperbarui Oleh],
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
                    b.CustId = @CustId 
                    AND b.PaymentStatus = 'verified'
                ORDER BY 
                    b.BDate DESC";

                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@CustId", loggedInCustomerId);

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


        //route halaman
        private void HomeBtn_Click(object sender, EventArgs e)
        {
            HomeCust homeCustDelivery = new HomeCust();
            homeCustDelivery.Show();
            this.Hide();
        }

        private void ProdukBtn_Click(object sender, EventArgs e)
        {
            ProdukCust produkCustDelivery = new ProdukCust();
            produkCustDelivery.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TransaksiCust transaksiCustDelivery = new TransaksiCust();
            transaksiCustDelivery.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DeliveryCust deliveryCustDelivery = new DeliveryCust();
            deliveryCustDelivery.Show();
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
            ProfileCust profileCustDelivery = new ProfileCust();
            profileCustDelivery.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DisplayDeliveryByStatus("sampai_tujuan");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DisplayDeliveryByStatus("dalam_perjalanan");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DisplayDeliveryByStatus("dikemas");
        }

    }
}
