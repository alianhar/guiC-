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
    public partial class Billing : Form
    {
        public Billing()
        {

            InitializeComponent();
            printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(printDocument1_PrintPage);
            EmpNameLbl.Text = Login.EmpName;
            GetCustomer();
            DisplayProduct();
            DisplayTransactions();
        }

        // Gunakan koneksi global
        SqlConnection Conn = new SqlConnection(@"Data Source=DESKTOP-UB2KSKP\SQLEXPRESS;Initial Catalog=db_toko_laptop_tugas;Integrated Security=True");

        private void GetCustomer()
        {
            try
            {
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT CustId FROM CustomerTbl", Conn);
                SqlDataReader Rdr = cmd.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Columns.Add("CustId", typeof(int));
                dt.Load(Rdr);

                CustIdCb.ValueMember = "CustId";
                CustIdCb.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error di GetCustomer: " + ex.Message);
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
            }
        }

        private void DisplayTransactions()
        {
            try
            {
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                string Query = "SELECT * FROM BillTbl";
                SqlDataAdapter sda = new SqlDataAdapter(Query, Conn);
                SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                TransactionsDGV.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error di DisplayProduct: " + ex.Message);
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
            }
        }

        private void DisplayProduct()
        {
            try
            {
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                string Query = "SELECT * FROM ProductTbl";
                SqlDataAdapter sda = new SqlDataAdapter(Query, Conn);
                SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                ProductsDGV.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error di DisplayProduct: " + ex.Message);
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
            }
        }

        private void GetCustName()
        {
            try
            {
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                string Query = "SELECT * FROM CustomerTbl WHERE CustId = '" + CustIdCb.SelectedValue.ToString() + "'";
                SqlCommand cmd = new SqlCommand(Query, Conn);
                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    CustNameTb.Text = dr["CustName"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error di GetCustName: " + ex.Message);
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
            }
        }

        // Deklarasi variabel global untuk key dan stok
        int key = 0, Stock = 0;

        private void reset()
        {
            PrNameTb.Text = "";
            PrPriceTb.Text = "";
            QtyTb.Text = "";
            Stock = 0;
            key = 0;
        }

        private void UpdateStock()
        {
            try
            {
                int NewQty = Stock - Convert.ToInt32(QtyTb.Text);

                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                SqlCommand cmd = new SqlCommand("UPDATE ProductTbl SET PrQty=@PQ WHERE PrId=@PKey", Conn);
                cmd.Parameters.AddWithValue("@PQ", NewQty);
                cmd.Parameters.AddWithValue("@PKey", key);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error di UpdateStock: " + ex.Message);
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                // Refresh data produk setelah update stok
                DisplayProduct();
            }
        }

        int n = 0, GrdTotal = 0;

        private void btnAddBill_Click(object sender, EventArgs e)
        {
            if (QtyTb.Text.Trim() == "" || Convert.ToInt32(QtyTb.Text) > Stock)
            {
                MessageBox.Show("Stok Tidak Cukup!");
            }
            else if (QtyTb.Text.Trim() == "" || key == 0)
            {
                MessageBox.Show("Pilih Produk Terlebih Dahulu");
            }
            else
            {
                int total = Convert.ToInt32(QtyTb.Text) * Convert.ToInt32(PrPriceTb.Text);
                DataGridViewRow newRow = new DataGridViewRow();
                // Pastikan memasukkan juga ProductID ke kolom tersembunyi
                newRow.CreateCells(BillsDGV, n + 1, key, PrNameTb.Text, QtyTb.Text, PrPriceTb.Text, total);

                GrdTotal += total;
                BillsDGV.Rows.Add(newRow);
                n++;

                TotalLbl.Text = GrdTotal.ToString();
                UpdateStock();
                reset();
            }

        }


        private void btnReset_Click(object sender, EventArgs e)
        {
            reset();
        }

        private void Billing_Load(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            // Pastikan DataGridView kosong terlebih dahulu
            BillsDGV.Columns.Clear();
            BillsDGV.Columns.Add("No", "No");
            BillsDGV.Columns.Add("ProductID", "Product ID");  // Kolom ini bisa disembunyikan
            BillsDGV.Columns.Add("ProductName", "Product Name");
            BillsDGV.Columns.Add("Quantity", "Quantity");
            BillsDGV.Columns.Add("Price", "Price");
            BillsDGV.Columns.Add("Total", "Total");

            // Sembunyikan kolom ProductID agar tidak tampil ke user
            BillsDGV.Columns["ProductID"].Visible = false;

        }

        string prodname;
        private void btnPrint_Click(object sender, EventArgs e)
        {
            int billId = InsertBill();
            InsertBillDetails(billId);
            printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("pprnm", 600, 600);
            if(printPreviewDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void InsertBillDetails(int billId)
        {
            try
            {
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                foreach (DataGridViewRow row in BillsDGV.Rows)
                {
                    // Pastikan baris tidak kosong
                    if (row.Cells["No"].Value == null)
                        continue;

                    // Ambil ProductID dari kolom tersembunyi
                    int productId = Convert.ToInt32(row.Cells["ProductID"].Value);
                    int quantity = Convert.ToInt32(row.Cells["Quantity"].Value);

                    SqlCommand cmd = new SqlCommand("INSERT INTO BillDetailTbl(BillId, ProductId, Quantity) VALUES (@bid, @pid, @qty)", Conn);
                    cmd.Parameters.AddWithValue("@bid", billId);
                    cmd.Parameters.AddWithValue("@pid", productId);
                    cmd.Parameters.AddWithValue("@qty", quantity);
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saat memasukkan detail bill: " + ex.Message);
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
            }
        }


        private int InsertBill()
        {
            int billId = 0;
            try
            {
                Conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO BillTbl(BDate, CustId, CustName, EmpName, Amt) OUTPUT INSERTED.BNum VALUES (@BD, @CI, @CN, @EN, @Am)", Conn);
                cmd.Parameters.AddWithValue("@BD", DateTime.Today.Date);
                cmd.Parameters.AddWithValue("@CI", CustIdCb.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@CN", CustNameTb.Text);
                cmd.Parameters.AddWithValue("@EN", EmpNameLbl.Text);
                cmd.Parameters.AddWithValue("@Am", GrdTotal);

                billId = Convert.ToInt32(cmd.ExecuteScalar());
                MessageBox.Show("Berhasil Menambahkan Bill");
                DisplayTransactions();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
            }
            return billId;
        }


        //int prodid, prodqty, prodprice, total, pos = 60;

        private void btnMenuProduk_Click(object sender, EventArgs e)
        {
            Produk produkBilling = new Produk();
            produkBilling.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Pegawai pegawaiBilling = new Pegawai();
            pegawaiBilling.Show();
            this.Hide();
        }

        private void btnMenuPelanggan_Click(object sender, EventArgs e)
        {
            Customer pelangganBilling = new Customer();
            pelangganBilling.Show();
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

        private void button1_Click(object sender, EventArgs e)
        {
            Home homeBilling = new Home();
            homeBilling.Show();
            this.Hide();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Profile profileBilling = new Profile();
            profileBilling.Show();
            this.Hide();
        }

        private void TransactionsDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            // Pengaturan dasar posisi dan ukuran
            int startX = 26;
            int startY = 10;
            int headerY = 40;
            int rowHeight = 20;

            // Buat objek StringFormat untuk center alignment
            StringFormat centerFormat = new StringFormat();
            centerFormat.Alignment = StringAlignment.Center;
            centerFormat.LineAlignment = StringAlignment.Center;

            // Judul
            Font titleFont = new Font("Times New Roman", 12, FontStyle.Bold);
            e.Graphics.DrawString("Bunny Toko Laptop", titleFont, Brushes.Red, new Point(startX + 80, startY));

            // Definisikan lebar tiap kolom
            int col1Width = 30;   // ID
            int col2Width = 120;  // PRODUK
            int col3Width = 80;   // HARGA
            int col4Width = 80;   // JUMLAH
            int col5Width = 100;  // TOTAL HARGA

            // Definisikan rectangle untuk header tiap kolom
            Rectangle rectCol1 = new Rectangle(startX, headerY, col1Width, rowHeight);
            Rectangle rectCol2 = new Rectangle(startX + col1Width, headerY, col2Width, rowHeight);
            Rectangle rectCol3 = new Rectangle(startX + col1Width + col2Width, headerY, col3Width, rowHeight);
            Rectangle rectCol4 = new Rectangle(startX + col1Width + col2Width + col3Width, headerY, col4Width, rowHeight);
            Rectangle rectCol5 = new Rectangle(startX + col1Width + col2Width + col3Width + col4Width, headerY, col5Width, rowHeight);

            // Font untuk header
            Font headerFont = new Font("Times New Roman", 10, FontStyle.Bold);

            // Cetak header dengan teks yang di-center di dalam kolomnya
            e.Graphics.DrawString("ID", headerFont, Brushes.Red, rectCol1, centerFormat);
            e.Graphics.DrawString("PRODUK", headerFont, Brushes.Red, rectCol2, centerFormat);
            e.Graphics.DrawString("HARGA", headerFont, Brushes.Red, rectCol3, centerFormat);
            e.Graphics.DrawString("JUMLAH", headerFont, Brushes.Red, rectCol4, centerFormat);
            e.Graphics.DrawString("TOTAL HARGA", headerFont, Brushes.Red, rectCol5, centerFormat);

            // Posisi awal untuk data (baris pertama data)
            int posY = headerY + rowHeight;

            // Cetak tiap baris data dari DataGridView
            foreach (DataGridViewRow row in BillsDGV.Rows)
            {
                // Lewati baris yang kosong (pastikan kolom "No" tidak null)
                if (row.Cells["No"].Value == null)
                    continue;

                // Ambil data dari setiap kolom
                int prodid = Convert.ToInt32(row.Cells["No"].Value);
                string prodname = row.Cells["ProductName"].Value.ToString();
                int prodqty = Convert.ToInt32(row.Cells["Quantity"].Value);
                int prodprice = Convert.ToInt32(row.Cells["Price"].Value);
                int total = Convert.ToInt32(row.Cells["Total"].Value);

                // Definisikan rectangle untuk data tiap kolom di baris saat ini
                Rectangle dataRect1 = new Rectangle(startX, posY, col1Width, rowHeight);
                Rectangle dataRect2 = new Rectangle(startX + col1Width, posY, col2Width, rowHeight);
                Rectangle dataRect3 = new Rectangle(startX + col1Width + col2Width, posY, col3Width, rowHeight);
                Rectangle dataRect4 = new Rectangle(startX + col1Width + col2Width + col3Width, posY, col4Width, rowHeight);
                Rectangle dataRect5 = new Rectangle(startX + col1Width + col2Width + col3Width + col4Width, posY, col5Width, rowHeight);

                // Font untuk isi data
                Font cellFont = new Font("Times New Roman", 12, FontStyle.Bold);
                e.Graphics.DrawString(prodid.ToString(), cellFont, Brushes.Blue, dataRect1, centerFormat);
                e.Graphics.DrawString(prodname, cellFont, Brushes.Blue, dataRect2, centerFormat);
                e.Graphics.DrawString(prodprice.ToString(), cellFont, Brushes.Blue, dataRect3, centerFormat);
                e.Graphics.DrawString(prodqty.ToString(), cellFont, Brushes.Blue, dataRect4, centerFormat);
                e.Graphics.DrawString(total.ToString(), cellFont, Brushes.Blue, dataRect5, centerFormat);

                // Pindah ke baris berikutnya
                posY += rowHeight;
            }

            // Cetak Grand Total dan info toko di bagian bawah
            e.Graphics.DrawString("GRAND TOTAL ITEM: Rp" + GrdTotal, new Font("Times New Roman", 12, FontStyle.Bold), Brushes.Crimson, new Point(50, posY + 50));
            e.Graphics.DrawString("-----------------PC STORE----------------", new Font("Times New Roman", 12, FontStyle.Bold), Brushes.Crimson, new Point(10, posY + 85));

            // Bersihkan DataGridView setelah pencetakan
            BillsDGV.Rows.Clear();
            BillsDGV.Refresh();
        }



        private void ProductsDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure the row index is valid
            {
                DataGridViewRow row = ProductsDGV.Rows[e.RowIndex];
                PrNameTb.Text = row.Cells[1].Value.ToString();
                Stock = Convert.ToInt32(row.Cells[3].Value.ToString());
                PrPriceTb.Text = row.Cells[4].Value.ToString();

                if (PrNameTb.Text == "")
                    key = 0;
                else
                    key = Convert.ToInt32(row.Cells[0].Value.ToString());
            }
        }

        private void CustIdCb_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetCustName();
        }
    }
}
