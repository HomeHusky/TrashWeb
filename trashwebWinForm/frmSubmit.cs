using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace trashwebWinForm
{
    public partial class frmSubmit : Form
    {
        // Tạo 3 biến lưu giá trị của rác
        private int countRecycle = 0;
        private int countDangerous = 0;
        private int countOther = 0;
        private int indexImage;
        bool leaveEdit = false;
        private ImageList ImageListNew = new ImageList();
        public frmSubmit()
        {
            InitializeComponent();
            FrmInit();
        }

        private void FrmInit()
        {
            pictureBox1.Dock = DockStyle.Fill;

        }

        private void frmSubmit_Load(object sender, EventArgs e)
        {
            
        }

        private void BtnDone_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Xác nhận hoàn thành?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
                leaveEdit = false;
                frmMain frmMain = new frmMain();
                frmMain.setMove(leaveEdit);
            }
            
        }

        public void setNumTrash(int recycle, int dangerous, int other)
        {
            this.countRecycle = recycle;
            this.countDangerous = dangerous;
            this.countOther = other;
        }

        public void setImageList(ImageList imageList)
        {
            ImageListNew = imageList;
        }

        private void BtnLeft_Click(object sender, EventArgs e)
        {
            this.Close();
            leaveEdit = true;
            frmMain frmMain = new frmMain();
            frmMain.setMove(leaveEdit);
        }

        public void LoadListImage()
        {
            pictureBox1.Image = ImageListNew.Images[0];
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
            indexImage++;
            pictureBox1.Image = ImageListNew.Images[indexImage];
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
            indexImage--;
            pictureBox1.Image = ImageListNew.Images[indexImage];
        }
    }
}
