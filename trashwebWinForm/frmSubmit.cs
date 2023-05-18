using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace trashwebWinForm
{
    public partial class frmSubmit : Form
    {
        // Sự kiện trả về biến bool cho frm Cha
        public delegate void ButtonClickedEventHandler(object sender, bool isLeftButtonPressed);
        public event ButtonClickedEventHandler ButtonClicked;

        // Sự kiện trả về mảng string cho form Cha
        public delegate void MyEventHandler(List<string> myArray);
        public event MyEventHandler frmSubmit_getList;

        // Tạo biến xem xét đã edit hay không
        private bool _isEdit = false;

        // Tạo List lưu files Json sau khi edit
        private List<string> listJsonAfterEdit;

        public List<string> listType = new List<string>();

        public List<string> listImageAsStringFromParent;

        private string currenFolder;

        // Tạo 3 biến lưu giá trị của rác
        private int countRecycle = 0;
        private int countDangerous = 0;
        private int countOther = 0;
        private int indexImage = 0;
        private List<string> typeTrash = new List<string>();
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

        private void getPictureBoxSize(ref Size size)
        {
            size = pictureBox1.Size;
        }

        private void BtnLeft_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Xác nhận bỏ qua?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Gọi sự kiện ButtonClicked và truyền giá trị false
                ButtonClicked?.Invoke(this, false);

                // Đóng form con
                this.Close();
            }
        }

        private void BtnDone_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn chắc chắn danh sách trên đã chính xác?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (_isEdit == false)
                {
                    // Gọi sự kiện ButtonClicked và truyền giá trị false
                    ButtonClicked?.Invoke(this, false);                    
                }
                else
                {
                    // Gọi sự kiện ButtonClicked và truyền giá trị true
                    ButtonClicked?.Invoke(this, true);

                    // Kích hoạt event và gọi delegate tương ứng trong form cha
                    if (frmSubmit_getList != null)
                    {
                        listJsonAfterEdit = listType;
                        frmSubmit_getList(listJsonAfterEdit);
                    }
                }
                // Đóng form con
                this.Close();
            }
            
        }

        public void setNumTrash(int recycle, int dangerous, int other)
        {
            this.countRecycle = recycle;
            this.countDangerous = dangerous;
            this.countOther = other;
        }

        public void setlistType(List<string> type)
        {
            this.typeTrash = type;
            string[] jsonFiles = Directory.GetFiles(currenFolder, "*.json");

            foreach (string jsonFile in jsonFiles)
            {
                // Lấy hình ảnh từ file JSON và chuyển đổi thành đối tượng Image
                string jsonString = File.ReadAllText(jsonFile);

                JsonDocument doc = JsonDocument.Parse(jsonString);
                JsonElement root = doc.RootElement;

                string typeTr = root.GetProperty("type").GetString();
                this.listType.Add(typeTr);
            }
        }

        public void setImageAsStringList(List<string> imageList)
        {
            this.listImageAsStringFromParent = imageList;
        }

        public void giveCurrenFolder(string folder)
        {
            if (folder != null) 
            {
                this.currenFolder = folder;
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

        public Image ConvertToImage(string ImageString)
        {
            byte[] imgData = Convert.FromBase64String(ImageString);
            MemoryStream ms = new MemoryStream(imgData, 0, imgData.Length);
            Image image = Image.FromStream(ms);
            return image;
        }

        public void LoadListImage()
        {
            string imageStart = listImageAsStringFromParent[0];
            System.Drawing.Image resizedImage = ResizeImage(ConvertToImage(imageStart), 454, 383);
            pictureBox1.Image = resizedImage;
            btnPrev.Enabled = false;
            btnRecycle.Size = new Size(278, 59);
            btnRecycle.Location = new Point(12, 26);
            btnRecycle.Text = typeTrash[0];
            lblNumTrash.Text = listImageAsStringFromParent.Count.ToString();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            btnPrev.Enabled = true;
            if (indexImage < listImageAsStringFromParent.Count - 1)
            {
                pictureBox1.Refresh();
                indexImage++;
                System.Drawing.Image resizedImage = ResizeImage(ConvertToImage(listImageAsStringFromParent[indexImage]), 454, 383);
                pictureBox1.Image = resizedImage;
                btnRecycle.Text = listType[indexImage];
                Console.WriteLine("Current Image: " + indexImage);
            }
            if (indexImage == listImageAsStringFromParent.Count - 1)
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
                System.Drawing.Image resizedImage = ResizeImage(ConvertToImage(listImageAsStringFromParent[indexImage]), 454, 383);
                pictureBox1.Image = resizedImage;
                btnRecycle.Text = listType[indexImage];
                Console.WriteLine("Current Image: " + indexImage);
            }
            if (indexImage == 0)
            {
                btnPrev.Enabled = false;
            }
        }

        private void DisableAfterEdit()
        {
            btnRecycle.Size = new Size(278, 59);
            btnRecycle.Location = new Point(12, 26);
            btnRecycle.Enabled = false;
            btnDangerous.Visible = false;
            btnOther.Visible = false;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if(btnEdit.Text == "Sửa")
            {
                btnEdit.Text = "Hủy";
                btnRecycle.Size = new Size(79, 43);
                btnRecycle.Location = new Point(12, 41);
                btnRecycle.Enabled = true;
                btnDangerous.Visible = true;
                btnOther.Visible = true;
            }
            else
            {
                btnEdit.Text = "Sửa";
                DisableAfterEdit();
            }
            if (_isEdit)
            {
                Console.WriteLine("Changed: True");
            }
            else Console.WriteLine("Changed: False");

        }

        private void btnRecycle_Click(object sender, EventArgs e)
        {
            _isEdit = true;
            btnRecycle.Text = "Recycle";
            btnEdit.Text = "Sửa";
            listType[indexImage] = "Recycle";
            DisableAfterEdit();
        }

        private void btnDangerous_Click(object sender, EventArgs e)
        {
            _isEdit = true;
            btnRecycle.Text = "Dangerous";
            btnEdit.Text = "Sửa";
            listType[indexImage] = "Dangerous";
            DisableAfterEdit();
        }

        private void btnOther_Click(object sender, EventArgs e)
        {
            _isEdit = true;
            btnRecycle.Text = "Other";
            btnEdit.Text = "Sửa";
            listType[indexImage] = "Other";
            DisableAfterEdit();
        }

        private void btnOld_Click(object sender, EventArgs e)
        {
            
            btnRecycle.Text = typeTrash[indexImage];
            Console.WriteLine(typeTrash[indexImage]);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            
            btnRecycle.Text = listType[indexImage];
            Console.WriteLine(listType[indexImage]);
        }
    }
}
