using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace p1
{
    public partial class form_baslangic : DevExpress.XtraEditors.XtraForm
    {
        public form_baslangic()
        {
            InitializeComponent();
        }
        private async Task RunProgressBarAsync()
        {
            // Progress bar yükleme işlemi başlat
            progressBarControl1.Properties.Maximum = 100;
            progressBarControl1.Properties.Minimum = 0;
            progressBarControl1.EditValue = 0;

            // 2 saniyeye yayılmış olarak progress bar'ı doldur
            for (int i = 0; i <= 100; i += 5)
            {
                progressBarControl1.EditValue = i;
                await Task.Delay(100); // 100 ms gecikme ile güncelle
            }
        }

        private async void form_baslangic_Load(object sender, EventArgs e)
        {
           
           
                try
                {
                    await RunProgressBarAsync();

                // Veritabanı yapılandırmasını kontrol et
                if (ProgramDatabaseConfig.SelectAndInitializeDatabase())
                    {
                        // Giriş formunu aç
                      Form1 login = new Form1();
                        login.Show();
                        this.Hide();
                    }
                    else
                    {
                        // Veritabanı yapılandırması başarısızsa çık
                        Application.Exit();
                    }
                }
                catch (Exception ex)
                {
                    // Hata durumunda kullanıcıya mesaj göster ve çık
                    MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
            }

        private void progressBarControl1_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
    }
