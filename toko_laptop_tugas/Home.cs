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
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
            EmpNameLbl.Text = Login.EmpName;
            hitungLaptop();
            hitungHardware();
            hitungGadget();
            hitungSparePart();
            hitungTotalSaldo();

            soldLaptop();
            soldHardware();
            soldGadget();
            soldSparePart();
        }

        //koneksi
        SqlConnection Conn = new SqlConnection(DBConnection.ConnectionString);


        public void hitungTotalSaldo()
        {
            Conn.Open();
            SqlDataAdapter sda = new SqlDataAdapter("SELECT SUM(Amt) FROM BillTbl", Conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            SaldoLbl.Text = dt.Rows[0][0].ToString();
            Conn.Close();
        }

        public void hitungLaptop()
        {
            string laptop = "laptop";
            Conn.Open();
            SqlDataAdapter sda = new SqlDataAdapter("SELECT COUNT(*) FROM ProductTbl WHERE PrCat = '" + laptop + "'", Conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            laptopLbl.Text = dt.Rows[0][0].ToString();
            Conn.Close();
        }


        public void hitungHardware()
        {
            string hardware = "hardware";
            Conn.Open();
            SqlDataAdapter sda = new SqlDataAdapter("SELECT COUNT(*) FROM ProductTbl WHERE PrCat = '" + hardware + "'", Conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            HardwareLbl.Text = dt.Rows[0][0].ToString();
            Conn.Close();
        }

        public void hitungGadget()
        {
            string gadget = "gadget";
            Conn.Open();
            SqlDataAdapter sda = new SqlDataAdapter("SELECT COUNT(*) FROM ProductTbl WHERE PrCat = '" + gadget + "'", Conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            GadgetLbl.Text = dt.Rows[0][0].ToString();
            Conn.Close();
        }

        public void hitungSparePart()
        {
            string sparePart = "spare_part_pc";
            Conn.Open();
            SqlDataAdapter sda = new SqlDataAdapter("SELECT COUNT(*) FROM ProductTbl WHERE PrCat = '" + sparePart + "'", Conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            SparePartLbl.Text = dt.Rows[0][0].ToString();
            Conn.Close();
        }

        public void soldLaptop()
        {
            try
            {
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                // Inisialisasi kategori yang ingin dicek
                string laptop = "laptop";

                // Menghitung total Quantity terjual untuk kategori "laptop"
                // COALESCE(SUM(...), 0) digunakan agar jika nilainya NULL, hasilnya 0
                string query = @"
            SELECT COALESCE(SUM(bd.Quantity), 0)
            FROM BillDetailTbl bd
            INNER JOIN ProductTbl p ON bd.ProductId = p.PrId
            WHERE p.PrCat = '" + laptop + "'";

                SqlDataAdapter sda = new SqlDataAdapter(query, Conn);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                // Misalnya label untuk menampilkan jumlah laptop terjual adalah soldLaptopLbl
                SoldLaptopLbl.Text = dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saat menghitung laptop terjual: " + ex.Message);
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
            }
        }

        public void soldHardware()
        {
            try
            {
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                string hardware = "hardware";
                string query = @"
            SELECT COALESCE(SUM(bd.Quantity), 0)
            FROM BillDetailTbl bd
            INNER JOIN ProductTbl p ON bd.ProductId = p.PrId
            WHERE p.PrCat = '" + hardware + "'";

                SqlDataAdapter sda = new SqlDataAdapter(query, Conn);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                SoldHardwareLbl.Text = dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saat menghitung hardware terjual: " + ex.Message);
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
            }
        }

        public void soldGadget()
        {
            try
            {
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                string gadget = "gadget";
                string query = @"
            SELECT COALESCE(SUM(bd.Quantity), 0)
            FROM BillDetailTbl bd
            INNER JOIN ProductTbl p ON bd.ProductId = p.PrId
            WHERE p.PrCat = '" + gadget + "'";

                SqlDataAdapter sda = new SqlDataAdapter(query, Conn);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                SoldGadgetLbl.Text = dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saat menghitung gadget terjual: " + ex.Message);
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
            }
        }

        public void soldSparePart()
        {
            try
            {
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                string sparePart = "spare_part_pc";
                string query = @"
            SELECT COALESCE(SUM(bd.Quantity), 0)
            FROM BillDetailTbl bd
            INNER JOIN ProductTbl p ON bd.ProductId = p.PrId
            WHERE p.PrCat = '" + sparePart + "'";

                SqlDataAdapter sda = new SqlDataAdapter(query, Conn);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                SoldSparePartLbl.Text = dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saat menghitung spare part terjual: " + ex.Message);
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
            }
        }




        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Profile profileHome = new Profile();
            profileHome.Show();
            this.Hide();
        }

        private void ProdukBtn_Click(object sender, EventArgs e)
        {
            Produk produkHome = new Produk();
            produkHome.Show();
            this.Hide();
        }

        private void PegawaiBtn_Click(object sender, EventArgs e)
        {
            Pegawai pegawaiHome = new Pegawai();
            pegawaiHome.Show();
            this.Hide();
        }

        private void PelangganBtn_Click(object sender, EventArgs e)
        {
            Customer customerHome = new Customer();
            customerHome.Show();
            this.Hide();
        }

        private void BillingBtn_Click(object sender, EventArgs e)
        {
            Billing billingHome = new Billing();
            billingHome.Show();
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

        private void button1_Click(object sender, EventArgs e)
        {
            Transaksi transaksiHome = new Transaksi();
            transaksiHome.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Delivery deliveryHome = new Delivery();
            deliveryHome.Show();
            this.Hide();
        }
    }
}
