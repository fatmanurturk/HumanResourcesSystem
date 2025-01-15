using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace p1
{
    public partial class calisanislemleri : Form
    {

        // Bağlantı dizesini ProgramDatabaseConfig sınıfından alıyoruz
        private readonly string connectionString = ProgramDatabaseConfig.ConnectionString;



        public calisanislemleri()
        {
            InitializeComponent();
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void textEdit6_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void textEdit1_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //Güncelle
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //Sil
            Sil();
        }

        public void Sil()
        {
            // Veritabanı bağlantısını açıyoruz
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {

                //   DialogResult onay = new MessageBox.Show("Kaydı silmek istediğinize emin misiniz ? ", "Onay Kutusu", MessageBoxButtons.YesNo, MessageBoxIcon.Information);



                DialogResult onay = MessageBox.Show("Kaydı silmek istediğinize emin misiniz?", "Onay Kutusu", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (onay == DialogResult.Yes)
                {
                    conn.Open();
                    string id = gridView1.GetFocusedRowCellValue("CalisanID").ToString();
                    SQLiteCommand cmd = new SQLiteCommand("DELETE FROM Calisanlar WHERE CalisanID='" + id + "'", conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    Listele();


                    // Silme işlemini gerçekleştir
                }



            }


        }


        private void simpleButton3_Click(object sender, EventArgs e)
        {
            // Ekleme işlemi
            Ekle();
        }

        public void Ekle()
        {
            try
            {
                // Veritabanı bağlantısını açıyoruz
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open(); // Bağlantıyı açıyoruz

                    // INSERT komutunu hazırlıyoruz
                    string komut = "INSERT INTO Calisanlar (Ad, Soyad, Eposta, Telefon, IseGirisTarihi, DepartmanID, Gorev, Maas, OlusturmaTarihi) " +
                                   "VALUES (@Ad, @Soyad, @Eposta, @Telefon, @IseGirisTarihi, @DepartmanID, @Gorev, @Maas, @OlusturmaTarihi)";

                    // SQLiteCommand nesnesi oluşturuyoruz
                    using (SQLiteCommand cmd = new SQLiteCommand(komut, conn))
                    {
                        // Parametreleri ekliyoruz
                        cmd.Parameters.AddWithValue("@Ad", textEdit1.Text); // Ad
                        cmd.Parameters.AddWithValue("@Soyad", textEdit2.Text); // Soyad
                        cmd.Parameters.AddWithValue("@Eposta", textEdit3.Text); // Eposta (Integer olarak alınıyor)
                        cmd.Parameters.AddWithValue("@Telefon", textEdit4.Text); // Telefon
                                                                                
                        cmd.Parameters.AddWithValue("@IseGirisTarihi", textEdit5.Text); // Işe giriş tarihi (yyyyMMdd formatında)

                        cmd.Parameters.AddWithValue("@DepartmanID", textEdit6.Text); // DepartmanID
                        cmd.Parameters.AddWithValue("@Gorev", textEdit7.Text); // Görev
                        cmd.Parameters.AddWithValue("@Maas", textEdit8.Text); // Maaş
                        cmd.Parameters.AddWithValue("@OlusturmaTarihi", textEdit10.Text); // Oluşturma tarihi (şu anki tarih)

                        // Komutu çalıştırıyoruz (veriyi ekliyoruz)
                        cmd.ExecuteNonQuery();
                    }
                    Listele();

                    MessageBox.Show("Çalışan başarıyla eklendi!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }


        private void simpleButton4_Click(object sender, EventArgs e)
        {
            Listele();

        }

        private void calisanislemleri_Load(object sender, EventArgs e)
        {
            Listele();
        }
        public void Listele()
        {
            try
            {
                // SQL sorgusu
                string komut = "SELECT * FROM Calisanlar";

                // SQLiteConnection nesnesini burada oluşturuyoruz
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();  // Bağlantıyı aç

                    // SQLiteDataAdapter ile veriyi çekiyoruz
                    SQLiteDataAdapter da = new SQLiteDataAdapter(komut, conn);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    // Veriyi DataGridView'e yüklüyoruz
                    //  dataGridView1.DataSource = ds.Tables[0];

                    gridControl1.DataSource = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void textEdit2_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void textEdit5_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}