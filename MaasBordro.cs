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
    public partial class MaasBordro : Form
    {


        // Bağlantı dizesini ProgramDatabaseConfig sınıfından alıyoruz
        private readonly string connectionString = ProgramDatabaseConfig.ConnectionString;


        public MaasBordro()
        {
            InitializeComponent();
        }

        private void textEdit1_EditValueChanged(object sender, EventArgs e)
        {

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
                    string komut = "INSERT INTO MaasBordro (CalisanID, Maas, VergiKesintisi, SGKKesintisi, DigerKesintiler, NetMaas, BordroTarihi, Aciklama, OlusturmaTarihi) " +
                                   "VALUES (@CalisanID, @Maas, @VergiKesintisi, @SGKKesintisi, @DigerKesintiler, @NetMaas, @BordroTarihi, @Aciklama, @OlusturmaTarihi)";

                    // SQLiteCommand nesnesi oluşturuyoruz
                    using (SQLiteCommand cmd = new SQLiteCommand(komut, conn))
                    {
                        // Parametreleri ekliyoruz
                        cmd.Parameters.AddWithValue("@CalisanID", textEdit2.Text); // Çalışan ID'si
                        cmd.Parameters.AddWithValue("@Maas", textEdit3.Text); // Maaş
                        cmd.Parameters.AddWithValue("@VergiKesintisi", textEdit4.Text); // Vergi Kesintisi
                        cmd.Parameters.AddWithValue("@SGKKesintisi", textEdit5.Text); // SGK Kesintisi
                        cmd.Parameters.AddWithValue("@DigerKesintiler", textEdit6.Text); // Diğer Kesintiler
                        cmd.Parameters.AddWithValue("@NetMaas", textEdit7.Text); // Net Maaş
                        cmd.Parameters.AddWithValue("@BordroTarihi", textEdit8.Text); // Bordro Tarihi
                        cmd.Parameters.AddWithValue("@Aciklama", textEdit9.Text); // Açıklama (isteğe bağlı)
                        cmd.Parameters.AddWithValue("@OlusturmaTarihi", textEdit10.Text); // Oluşturma Tarihi

                        // Komutu çalıştırıyoruz (veriyi ekliyoruz)
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Maaş bordrosu başarıyla eklendi!");
                    Listele();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }


        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //Sil
            Sil();

        }
        public void Sil()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    DialogResult onay = MessageBox.Show("Kaydı silmek istediğinize emin misiniz?", "Onay Kutusu", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (onay == DialogResult.Yes)
                    {
                        conn.Open();
                        string id = gridView1.GetFocusedRowCellValue("BordroID").ToString();

                        // Parametreli sorgu kullanarak güvenli silme işlemi
                        string komut = "DELETE FROM MaasBordro WHERE BordroID = @BordroID";
                        using (SQLiteCommand cmd = new SQLiteCommand(komut, conn))
                        {
                            cmd.Parameters.AddWithValue("@BordroID", id);
                            int sonuc = cmd.ExecuteNonQuery();

                            if (sonuc > 0)
                                MessageBox.Show("Kayıt başarıyla silindi.");
                            else
                                MessageBox.Show("Silinecek kayıt bulunamadı!");
                        }

                        Listele(); // Silme sonrası listeyi güncelle
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
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
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    string id = textEdit1.Text; // Güncellenecek BordroID

                    if (string.IsNullOrEmpty(id))
                    {
                        MessageBox.Show("Lütfen güncellenecek kaydı seçiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Güncelleme sorgusu
                    string komut = "UPDATE MaasBordro SET CalisanID = @CalisanID, Maas = @Maas, VergiKesintisi = @VergiKesintisi, " +
                                   "SGKKesintisi = @SGKKesintisi, DigerKesintiler = @DigerKesintiler, NetMaas = @NetMaas, BordroTarihi = @BordroTarihi, " +
                                   "Aciklama = @Aciklama, OlusturmaTarihi = @OlusturmaTarihi WHERE BordroID = @BordroID";

                    using (SQLiteCommand cmd = new SQLiteCommand(komut, conn))
                    {
                        cmd.Parameters.AddWithValue("@BordroID", id);
                        cmd.Parameters.AddWithValue("@CalisanID", textEdit2.Text);
                        cmd.Parameters.AddWithValue("@Maas", textEdit3.Text);
                        cmd.Parameters.AddWithValue("@VergiKesintisi", textEdit4.Text);
                        cmd.Parameters.AddWithValue("@SGKKesintisi", textEdit5.Text);
                        cmd.Parameters.AddWithValue("@DigerKesintiler", textEdit6.Text);
                        cmd.Parameters.AddWithValue("@NetMaas", textEdit7.Text);
                        cmd.Parameters.AddWithValue("@BordroTarihi", textEdit8.Text);
                        cmd.Parameters.AddWithValue("@Aciklama", textEdit9.Text);
                        cmd.Parameters.AddWithValue("@OlusturmaTarihi", textEdit10.Text);

                        int sonuc = cmd.ExecuteNonQuery();

                        if (sonuc > 0)
                            MessageBox.Show("Kayıt başarıyla güncellendi.");
                        else
                            MessageBox.Show("Güncellenecek kayıt bulunamadı!");
                    }

                    Listele(); // Güncellemeden sonra listeyi yenile
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }


        public void BilgiCek()
        {
            textEdit1.Text = gridView1.GetFocusedRowCellValue("BordroID").ToString();
            textEdit2.Text = gridView1.GetFocusedRowCellValue("CalisanID").ToString();
            textEdit3.Text = gridView1.GetFocusedRowCellValue("Maas").ToString();
            textEdit4.Text = gridView1.GetFocusedRowCellValue("VergiKesintisi").ToString();
            textEdit5.Text = gridView1.GetFocusedRowCellValue("SGKKesintisi").ToString();
            textEdit6.Text = gridView1.GetFocusedRowCellValue("DigerKesintiler").ToString();
            textEdit7.Text = gridView1.GetFocusedRowCellValue("NetMaas").ToString();
            textEdit8.Text = gridView1.GetFocusedRowCellValue("BordroTarihi").ToString();
            textEdit9.Text = gridView1.GetFocusedRowCellValue("Aciklama").ToString();
            textEdit10.Text = gridView1.GetFocusedRowCellValue("OlusturmaTarihi").ToString();

           

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
                string komut = "SELECT * FROM MaasBordro";

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

        private void MaasBordro_Load(object sender, EventArgs e)
        {
            Listele();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            //Düzenle
            BilgiCek();
        }
    }
}
