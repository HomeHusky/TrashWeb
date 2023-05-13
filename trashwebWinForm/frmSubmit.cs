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
            if (MessageBox.Show("Bạn chắc chắn danh sách trên đã chính xác?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
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
            if (MessageBox.Show("Xác nhận bỏ qua?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        // Hàm resize image
        public static System.Drawing.Image ResizeImage(System.Drawing.Image image, int width, int height)
        {
            // Tạo bitmap mới với kích thước mới
            Bitmap bitmap = new Bitmap(width, height);

            // Tạo đối tượng Graphics từ bitmap mới
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                // Thay đổi kích thước hình ảnh
                graphics.DrawImage(image, 0, 0, width, height);
            }

            // Trả về hình ảnh mới
            return bitmap;
        }

        public void LoadListImage()
        {
            System.Drawing.Image resizedImage = ResizeImage(ImageListNew.Images[0], 454, 383);
            pictureBox1.Image = resizedImage;
            btnPrev.Enabled = false;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            btnPrev.Enabled = true;
            if (indexImage < ImageListNew.Images.Count - 1)
            {
                pictureBox1.Refresh();
                indexImage++;
                System.Drawing.Image resizedImage = ResizeImage(ImageListNew.Images[indexImage], 454, 383);
                pictureBox1.Image = resizedImage;
                Console.WriteLine("Current Image: " + indexImage);
            }
            if (indexImage ==  ImageListNew.Images.Count - 1)
            {
                btnNext.Enabled = false;
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            btnNext.Enabled = true;
            if (indexImage > 0)
            {
                pictureBox1.Refresh();
                indexImage--;
                System.Drawing.Image resizedImage = ResizeImage(ImageListNew.Images[indexImage], 454, 383);
                pictureBox1.Image = resizedImage;
                Console.WriteLine("Current Image: " + indexImage);
            }
            if (indexImage == 0)
            {
                btnPrev.Enabled = false;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
