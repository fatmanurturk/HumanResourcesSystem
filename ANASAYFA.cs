using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;

namespace p1
{
    public partial class ANASAYFA : DevExpress.XtraBars.TabForm
    {
        public ANASAYFA()
        {
            InitializeComponent();
        }
                calisanislemleri calisan ;

        private void ANASAYFA_Load(object sender, EventArgs e)
        {

        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (calisan == null || calisan.IsDisposed)
            {
                calisan = new calisanislemleri();
                calisan.MdiParent = this;
                calisan.Show();
            }

        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            Formlar.FrmIsilanlari frm1 = new Formlar.FrmIsilanlari();
            frm1.MdiParent = this;
            frm1.Show(); 
        }

        private void barButtonItem5_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            Formlar.Izınler frm2 = new Formlar.Izınler();
            frm2.MdiParent = this;
            frm2.Show();
        }

        private void barButtonItem5_ItemClick_2(object sender, ItemClickEventArgs e)
        {
            Formlar.MaasBordro frm3 = new Formlar.MaasBordro();
            frm3.MdiParent = this;
            frm3.Show();
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            Formlar.Egitimvegelisim frm4 = new Formlar.Egitimvegelisim();
            frm4.MdiParent = this;
            frm4.Show();
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            Formlar.Raporlar frm5 = new Formlar.Raporlar();
            frm5.MdiParent = this;
            frm5.Show();
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            Formlar.Performans frm6 = new Formlar.Performans();
            frm6.MdiParent = this;
            frm6.Show();
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            Formlar.Primler frm7 = new Formlar.Primler();
            frm7.MdiParent = this;
            frm7.Show();

        }
    }
}