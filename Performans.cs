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
    public partial class Performans : Form
    {
        // Bağlantı dizesini ProgramDatabaseConfig sınıfından alıyoruz
        private readonly string connectionString = ProgramDatabaseConfig.ConnectionString;

        public Performans()
        {
            InitializeComponent();
        }

        private void Performans_Load(object sender, EventArgs e)
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
                string komut = "SELECT * FROM Performans";

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
                    string komut = "INSERT INTO Performans (CalisanID, DegerlendirmeTarihi, GenelPuan, HedeflereUlasma, TakimCalismasi, IletisimBecerisi, ProblemCozme, Yaraticilik, Aciklama, OlusturmaTarihi) " +
                                   "VALUES (@CalisanID, @DegerlendirmeTarihi, @GenelPuan, @HedeflereUlasma, @TakimCalismasi, @IletisimBecerisi, @ProblemCozme, @Yaraticilik, @Aciklama, @OlusturmaTarihi)";

                    // SQLiteCommand nesnesi oluşturuyoruz
                    using (SQLiteCommand cmd = new SQLiteCommand(komut, conn))
                    {
                        // Girişlerin boş olup olmadığını kontrol ediyoruz
                        if (string.IsNullOrWhiteSpace(textEdit1.Text) ||
                            string.IsNullOrWhiteSpace(textEdit2.Text) ||
                            string.IsNullOrWhiteSpace(textEdit3.Text) ||
                            string.IsNullOrWhiteSpace(textEdit4.Text) ||
                            string.IsNullOrWhiteSpace(textEdit5.Text) ||
                            string.IsNullOrWhiteSpace(textEdit6.Text) ||
                            string.IsNullOrWhiteSpace(textEdit7.Text) ||
                            string.IsNullOrWhiteSpace(textEdit8.Text) ||
                            string.IsNullOrWhiteSpace(textEdit9.Text) ||
                            string.IsNullOrWhiteSpace(textEdit10.Text))
                        {
                            MessageBox.Show("Lütfen tüm alanları doldurun!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Parametreleri ekliyoruz
                        cmd.Parameters.AddWithValue("@CalisanID", textEdit1.Text);  // Çalışan ID
                        cmd.Parameters.AddWithValue("@DegerlendirmeTarihi", textEdit2.Text);  // Değerlendirme Tarihi
                        cmd.Parameters.AddWithValue("@GenelPuan", textEdit3.Text);  // Genel Puan
                        cmd.Parameters.AddWithValue("@HedeflereUlasma", textEdit4.Text);  // Hedeflere Ulaşma Puanı
                        cmd.Parameters.AddWithValue("@TakimCalismasi", textEdit5.Text);  // Takım Çalışması Puanı
                        cmd.Parameters.AddWithValue("@IletisimBecerisi", textEdit6.Text);  // İletişim Becerisi Puanı
                        cmd.Parameters.AddWithValue("@ProblemCozme", textEdit7.Text);  // Problem Çözme Puanı
                        cmd.Parameters.AddWithValue("@Yaraticilik", textEdit8.Text);  // Yaratıcılık Puanı
                        cmd.Parameters.AddWithValue("@Aciklama", textEdit9.Text);  // Açıklama
                        cmd.Parameters.AddWithValue("@OlusturmaTarihi", textEdit10.Text);  // Oluşturma Tarihi

                        // Komutu çalıştırıyoruz (veriyi ekliyoruz)
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Performans kaydı başarıyla eklendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    string id = gridView1.GetFocusedRowCellValue("DegerlendirmeID").ToString();
                    SQLiteCommand cmd = new SQLiteCommand("DELETE FROM Performans WHERE DegerlendirmeID='" + id + "'", conn);
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
                // Performans ID boş veya geçersiz mi kontrol et
                if (string.IsNullOrWhiteSpace(textEdit11.Text) || !int.TryParse(textEdit11.Text, out int performansId))
                {
                    MessageBox.Show("Geçersiz Performans ID!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Gerekli tüm alanları kontrol et
                if (string.IsNullOrWhiteSpace(textEdit1.Text) ||
                    string.IsNullOrWhiteSpace(textEdit2.Text) ||
                    string.IsNullOrWhiteSpace(textEdit3.Text) ||
                    string.IsNullOrWhiteSpace(textEdit4.Text) ||
                    string.IsNullOrWhiteSpace(textEdit5.Text) ||
                    string.IsNullOrWhiteSpace(textEdit6.Text) ||
                    string.IsNullOrWhiteSpace(textEdit7.Text) ||
                    string.IsNullOrWhiteSpace(textEdit8.Text) ||
                    string.IsNullOrWhiteSpace(textEdit9.Text) ||
                    string.IsNullOrWhiteSpace(textEdit10.Text))
                {
                    MessageBox.Show("Lütfen tüm alanları doldurun!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Veritabanı bağlantısını aç
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();  // Bağlantıyı aç

                    // SQL UPDATE sorgusu
                    string komut = @"
            UPDATE Performans SET 
            CalisanID = @CalisanID, 
            DegerlendirmeTarihi = @DegerlendirmeTarihi, 
            GenelPuan = @GenelPuan, 
            HedeflereUlasma = @HedeflereUlasma, 
            TakimCalismasi = @TakimCalismasi, 
            IletisimBecerisi = @IletisimBecerisi, 
            ProblemCozme = @ProblemCozme, 
            Yaraticilik = @Yaraticilik, 
            Aciklama = @Aciklama, 
            OlusturmaTarihi = @OlusturmaTarihi 
            WHERE DegerlendirmeID = @DegerlendirmeID";

                    using (SQLiteCommand cmd = new SQLiteCommand(komut, conn))
                    {
                        // Parametreleri ekle
                        cmd.Parameters.AddWithValue("@CalisanID", textEdit1.Text);
                        cmd.Parameters.AddWithValue("@DegerlendirmeTarihi", textEdit2.Text);
                        cmd.Parameters.AddWithValue("@GenelPuan", textEdit3.Text);
                        cmd.Parameters.AddWithValue("@HedeflereUlasma", textEdit4.Text);
                        cmd.Parameters.AddWithValue("@TakimCalismasi", textEdit5.Text);
                        cmd.Parameters.AddWithValue("@IletisimBecerisi", textEdit6.Text);
                        cmd.Parameters.AddWithValue("@ProblemCozme", textEdit7.Text);
                        cmd.Parameters.AddWithValue("@Yaraticilik", textEdit8.Text);
                        cmd.Parameters.AddWithValue("@Aciklama", textEdit9.Text);
                        cmd.Parameters.AddWithValue("@OlusturmaTarihi", textEdit10.Text);
                        cmd.Parameters.AddWithValue("@DegerlendirmeID", textEdit11.Text);

                        // Komutu çalıştır ve etkilenen satır sayısını kontrol et
                        int etkilenenSatir = cmd.ExecuteNonQuery();

                        if (etkilenenSatir > 0)
                        {
                            MessageBox.Show("Performans kaydı başarıyla güncellendi!");
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
            textEdit11.Text = gridView1.GetFocusedRowCellValue("DegerlendirmeID").ToString();
            textEdit1.Text = gridView1.GetFocusedRowCellValue("CalisanID").ToString();
            textEdit2.Text = gridView1.GetFocusedRowCellValue("DegerlendirmeTarihi").ToString();
            textEdit3.Text = gridView1.GetFocusedRowCellValue("GenelPuan").ToString();
            textEdit4.Text = gridView1.GetFocusedRowCellValue("HedeflereUlasma").ToString();
            textEdit5.Text = gridView1.GetFocusedRowCellValue("TakimCalismasi").ToString();
            textEdit6.Text = gridView1.GetFocusedRowCellValue("IletisimBecerisi").ToString();
            textEdit7.Text = gridView1.GetFocusedRowCellValue("ProblemCozme").ToString();
            textEdit8.Text = gridView1.GetFocusedRowCellValue("Yaraticilik").ToString();
            textEdit9.Text = gridView1.GetFocusedRowCellValue("Aciklama").ToString();
            textEdit10.Text = gridView1.GetFocusedRowCellValue("OlusturmaTarihi").ToString();

            simpleButton3.Text = "Değiştir";
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            //Düzenle
            BilgiCek();
        }

        private void xtraTabPage1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
