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

            // Add these lines in the constructor or InitializeComponent method
            //BillsDGV.CellClick += new DataGridViewCellEventHandler(BillsDGV_CellClick);
            //btnDeleteBill.Click += new EventHandler(btnDeleteBill_Click);
        }

        // Gunakan koneksi global
        SqlConnection Conn = new SqlConnection(DBConnection.ConnectionString);

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

                string Query = @"
                SELECT 
                    b.BDate, 
                    b.CustName, 
                    b.EmpName, 
                    b.Amt, 
                    b.PaymentDate,
                    SUM(d.Quantity) AS TotalQty
                FROM BillTbl b
                JOIN BillDetailTbl d ON b.BNum = d.BillId
                WHERE CAST(b.BDate AS DATE) = CAST(GETDATE() AS DATE)
                GROUP BY b.BDate, b.CustName, b.EmpName, b.Amt, b.PaymentDate
                ORDER BY b.BDate DESC, b.PaymentDate DESC";


                SqlDataAdapter sda = new SqlDataAdapter(Query, Conn);
                SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                TransactionsDGV.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error di DisplayTransactions: " + ex.Message);
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

        
        int n = 0, GrdTotal = 0;

        // First, remove the UpdateStock() call from btnAddBill_Click
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
                // UpdateStock() dipindahkan ke tombol Print
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

        // Add a method to validate stock before printing
        private bool ValidateStock()
        {
            // Create a dictionary to track total quantity per product
            Dictionary<int, int> productQuantities = new Dictionary<int, int>();
            Dictionary<int, int> productStocks = new Dictionary<int, int>();

            // First, get current stock for all products that are in the bill
            try
            {
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                foreach (DataGridViewRow row in BillsDGV.Rows)
                {
                    if (row.Cells["ProductID"].Value == null)
                        continue;

                    int productId = Convert.ToInt32(row.Cells["ProductID"].Value);

                    // Only query stock for products we haven't checked yet
                    if (!productStocks.ContainsKey(productId))
                    {
                        SqlCommand cmd = new SqlCommand("SELECT PrQty FROM ProductTbl WHERE PrId = @PID", Conn);
                        cmd.Parameters.AddWithValue("@PID", productId);
                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            productStocks[productId] = Convert.ToInt32(result);
                        }
                        else
                        {
                            MessageBox.Show($"Produk dengan ID {productId} tidak ditemukan!");
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saat validasi stok: " + ex.Message);
                return false;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
            }

            // Calculate total quantity per product in the bill
            foreach (DataGridViewRow row in BillsDGV.Rows)
            {
                if (row.Cells["ProductID"].Value == null)
                    continue;

                int productId = Convert.ToInt32(row.Cells["ProductID"].Value);
                int quantity = Convert.ToInt32(row.Cells["Quantity"].Value);

                if (productQuantities.ContainsKey(productId))
                {
                    productQuantities[productId] += quantity;
                }
                else
                {
                    productQuantities[productId] = quantity;
                }
            }

            // Validate if there's enough stock for each product
            foreach (var product in productQuantities)
            {
                int productId = product.Key;
                int totalQuantity = product.Value;

                if (totalQuantity > productStocks[productId])
                {
                    MessageBox.Show($"Stok tidak cukup untuk produk ID {productId}. " +
                                   $"Total yang diminta: {totalQuantity}, Stok tersedia: {productStocks[productId]}");
                    return false;
                }
            }

            return true;
        }

        // Method to update stock for all products in the bill
        private void UpdateAllStock()
        {
            try
            {
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();

                // Group by product ID and sum quantities
                Dictionary<int, int> productQuantities = new Dictionary<int, int>();

                foreach (DataGridViewRow row in BillsDGV.Rows)
                {
                    if (row.Cells["ProductID"].Value == null)
                        continue;

                    int productId = Convert.ToInt32(row.Cells["ProductID"].Value);
                    int quantity = Convert.ToInt32(row.Cells["Quantity"].Value);

                    if (productQuantities.ContainsKey(productId))
                    {
                        productQuantities[productId] += quantity;
                    }
                    else
                    {
                        productQuantities[productId] = quantity;
                    }
                }

                // Update stock for each product
                foreach (var product in productQuantities)
                {
                    int productId = product.Key;
                    int totalQuantity = product.Value;

                    SqlCommand cmd = new SqlCommand("UPDATE ProductTbl SET PrQty = PrQty - @Qty WHERE PrId = @PID", Conn);
                    cmd.Parameters.AddWithValue("@Qty", totalQuantity);
                    cmd.Parameters.AddWithValue("@PID", productId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saat mengupdate stok: " + ex.Message);
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();

                // Refresh product display after updating stock
                DisplayProduct();
            }
        }

        string prodname;
        // Update the btnPrint_Click method
        private void btnPrint_Click(object sender, EventArgs e)
        {
            // Validate if there are items in the bill
            if (BillsDGV.Rows.Count <= 0 || GrdTotal <= 0)
            {
                MessageBox.Show("Silakan tambahkan produk ke bill terlebih dahulu!");
                return;
            }

            // Validate stock before proceeding
            if (!ValidateStock())
            {
                return; // Stop if validation fails
            }

            // All validations passed, proceed with the transaction
            int billId = InsertBill();
            InsertBillDetails(billId);

            // Update stock for all products after validation and DB insertions
            UpdateAllStock();
            DisplayTransactions();
            

            // Print the bill
            printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("pprnm", 600, 600);
            if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
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
                SqlCommand cmd = new SqlCommand("INSERT INTO BillTbl(BDate, CustId, CustName, EmpName, Amt,PaymentDate) OUTPUT INSERTED.BNum VALUES (@BD, @CI, @CN, @EN, @Am,@Pd)", Conn);
                cmd.Parameters.AddWithValue("@BD", DateTime.Today.Date);
                cmd.Parameters.AddWithValue("@Pd", DateTime.Now);
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

     

        // Variable to track the currently selected row in BillsDGV
        private int selectedRow = -1;

        // Handle row selection in BillsDGV
        private void BillsDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Make sure we have a valid row index (not header row)
            if (e.RowIndex >= 0 && e.RowIndex < BillsDGV.Rows.Count)
            {
                // Store the selected row index
                selectedRow = e.RowIndex;

                // Optional: Highlight the selected row for better UX
                BillsDGV.ClearSelection();
                BillsDGV.Rows[selectedRow].Selected = true;
            }
        }

        // Delete selected row from BillsDGV
        private void btnDeleteBill_Click(object sender, EventArgs e)
        {
            // Temporarily remove the event handler to prevent double triggers
            btnDeleteBill.Click -= new EventHandler(btnDeleteBill_Click);

            try
            {
                // Check if a row is selected
                if (selectedRow == -1 || selectedRow >= BillsDGV.Rows.Count)
                {
                    MessageBox.Show("Pilih item yang akan dihapus terlebih dahulu!");
                    return;
                }

                // Get the total amount of the selected row
                int rowTotal = Convert.ToInt32(BillsDGV.Rows[selectedRow].Cells["Total"].Value);

                // Subtract the row total from the grand total
                GrdTotal -= rowTotal;

                // Update the total label
                TotalLbl.Text = GrdTotal.ToString();

                // Remove the row from the DataGridView
                BillsDGV.Rows.RemoveAt(selectedRow);

                // Renumber the remaining rows in the "No" column
                for (int i = 0; i < BillsDGV.Rows.Count; i++)
                {
                    if (BillsDGV.Rows[i].Cells["No"].Value != null)
                    {
                        BillsDGV.Rows[i].Cells["No"].Value = i + 1;
                    }
                }

                // Show success message
                MessageBox.Show("Item berhasil dihapus dari bill!");

                // Reset the selected row index AFTER all operations are done
                selectedRow = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saat menghapus item: " + ex.Message);
            }
            finally
            {
                // Re-add the event handler
                btnDeleteBill.Click += new EventHandler(btnDeleteBill_Click);
            }
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

        private void button8_Click(object sender, EventArgs e)
        {
            Transaksi transaksiBilling = new Transaksi();
            transaksiBilling.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Delivery deliveryBilling = new Delivery();
            deliveryBilling.Show();
            this.Hide();
        }


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
    }
}
