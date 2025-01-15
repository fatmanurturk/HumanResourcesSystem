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


namespace p1.Formlar
{
    public partial class Raporlar : Form
    {


        // Bağlantı dizesini ProgramDatabaseConfig sınıfından alıyoruz
        private readonly string connectionString = ProgramDatabaseConfig.ConnectionString;

        public Raporlar()
        {
            InitializeComponent();
        }

        private void Raporlar_Load(object sender, EventArgs e)
        {
            Listele();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            //Listele
            Listele();

        }
        public void Listele()
        {

            try
            {
                // SQL sorgusu
                string komut = "SELECT * FROM Raporlar";

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

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //Ekle
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
                    string komut = "INSERT INTO Raporlar (RaporAdi, RaporTuru, RaporTarihi, IlgiliCalisanID, Aciklama) " +
                                   "VALUES (@RaporAdi, @RaporTuru, @RaporTarihi, @IlgiliCalisanID, @Aciklama)";

                    // SQLiteCommand nesnesi oluşturuyoruz
                    using (SQLiteCommand cmd = new SQLiteCommand(komut, conn))
                    {
                        // Girişlerin boş olup olmadığını kontrol ediyoruz
                        if (string.IsNullOrWhiteSpace(textEdit1.Text) ||
                            string.IsNullOrWhiteSpace(textEdit2.Text) ||
                            string.IsNullOrWhiteSpace(textEdit3.Text) ||
                            string.IsNullOrWhiteSpace(textEdit4.Text) ||
                            string.IsNullOrWhiteSpace(textEdit5.Text) ||
                            string.IsNullOrWhiteSpace(textEdit6.Text))
                        {
                            MessageBox.Show("Lütfen tüm alanları doldurun!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Parametreleri ekliyoruz
                        cmd.Parameters.AddWithValue("@RaporAdi", textEdit1.Text);  // Rapor Adı
                        cmd.Parameters.AddWithValue("@RaporTuru", textEdit2.Text);  // Rapor Türü
                        cmd.Parameters.AddWithValue("@RaporTarihi", textEdit3.Text); // Rapor Tarihi
                        cmd.Parameters.AddWithValue("@IlgiliCalisanID", textEdit4.Text); // İlgili Çalışan ID 
                        cmd.Parameters.AddWithValue("@Aciklama", textEdit5.Text);  // Açıklama
                        cmd.Parameters.AddWithValue("@OlusturmaTarihi", textEdit6.Text);  // Oluşturma Tarihi

                        // Komutu çalıştırıyoruz (veriyi ekliyoruz)
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Rapor başarıyla eklendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Çalışan ID alanına geçerli bir sayı giriniz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                    string id = gridView1.GetFocusedRowCellValue("RaporID").ToString();
                    SQLiteCommand cmd = new SQLiteCommand("DELETE FROM Raporlar WHERE RaporID='" + id + "'", conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    Listele();


                    // Silme işlemini gerçekleştir
                }
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            //Güncelle
            Guncelle();

          
        }

        public void Guncelle()
        {
            try
            {
                // RaporID boş veya geçersiz mi kontrol et
                if (string.IsNullOrWhiteSpace(textEdit7.Text) || !int.TryParse(textEdit7.Text, out int raporId))
                {
                    MessageBox.Show("Geçersiz Rapor ID!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Veritabanı bağlantısını aç
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();  // Bağlantıyı aç

                    // SQL UPDATE sorgusu
                    string komut = @"
                UPDATE Raporlar SET 
                RaporAdi = @RaporAdi, 
                RaporTuru = @RaporTuru, 
                RaporTarihi = @RaporTarihi, 
                IlgiliCalisanID = @IlgiliCalisanID, 
                Aciklama = @Aciklama, 
                OlusturmaTarihi = @OlusturmaTarihi 
                WHERE RaporID = @RaporID";

                    using (SQLiteCommand cmd = new SQLiteCommand(komut, conn))
                    {
                        // Parametreleri ekle
                        cmd.Parameters.AddWithValue("@RaporAdi", textEdit1.Text);
                        cmd.Parameters.AddWithValue("@RaporTuru", textEdit2.Text);
                        cmd.Parameters.AddWithValue("@RaporTarihi", textEdit3.Text);
                        cmd.Parameters.AddWithValue("@IlgiliCalisanID", textEdit4.Text);
                        cmd.Parameters.AddWithValue("@Aciklama", textEdit5.Text);
                        cmd.Parameters.AddWithValue("@OlusturmaTarihi", textEdit6.Text);
                        cmd.Parameters.AddWithValue("@RaporID", textEdit7.Text);

                        // Komutu çalıştır ve etkilenen satır sayısını kontrol et
                        int etkilenenSatir = cmd.ExecuteNonQuery();

                        if (etkilenenSatir > 0)
                        {
                            MessageBox.Show("Rapor başarıyla güncellendi!");
                            Listele(); // Güncellemeden sonra listeyi yenile
                        }
                        else
                        {
                            MessageBox.Show("Güncellenecek kayıt bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Güncelleme Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public void BilgiCek()
        {
            if (gridView1.FocusedRowHandle < 0)
            {
                MessageBox.Show("Lütfen bir satır seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            textEdit7.Text = gridView1.GetFocusedRowCellValue("RaporID").ToString();
            textEdit1.Text = gridView1.GetFocusedRowCellValue("RaporAdi")?.ToString();
            textEdit2.Text = gridView1.GetFocusedRowCellValue("RaporTuru")?.ToString();
            textEdit3.Text = gridView1.GetFocusedRowCellValue("RaporTarihi")?.ToString();
            textEdit4.Text = gridView1.GetFocusedRowCellValue("IlgiliCalisanID")?.ToString();
            textEdit5.Text = gridView1.GetFocusedRowCellValue("Aciklama")?.ToString();
            textEdit6.Text = gridView1.GetFocusedRowCellValue("OlusturmaTarihi")?.ToString();

            simpleButton3.Text = "Güncelle";
        }

        private void xtraTabPage2_Paint(object sender, PaintEventArgs e)
        {


        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            //Düzenle
            BilgiCek();
        }
    }
}
