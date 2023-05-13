using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace trashwebWinForm
{
    public partial class frmMain : Form
    {
        private string HOST = "192.168.1.26";

        // Tạo biến để set cho form đi tiếp hay lùi lại
        public bool leaveEdit = false;

        // Tạo biến image mới để lưu dữ liệu ảnh từ client để luồng chính có thể set pictureBox
        System.Drawing.Image currentImage = null;


        // Tạo 3 biến lưu giá trị của rác
        public int countRecycle = 0;
        public int countDangerous = 0;
        public int countOther = 0;

        //Tạo listImage để lưu ảnh
        public ImageList imageList = new ImageList();

        private string message = "Start";

        // Khởi tạo server
        private Thread receiveThread;

        private TcpListener _tcpListener;
        //Khởi tạo biến xác nhận đã connect với client
        private bool _isListening = false;
        private bool stopRecv = false;
        private readonly string _jsonFolderPath = "D:\\GitTrashWeb\\trashwebWinForm\\json\\";

        public frmMain()
        {
            InitializeComponent();

            // Start the receive thread
            receiveThread = new Thread(Start_Server);
            receiveThread.Start();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            FormInit();
            LoadIntroForm();
        }

        private void FormInit()
        {
            pictureBox1.Visible = false;
            label7.Visible = false;
            lblTaiChe.Visible = false;
            lblLoaiKhac.Visible = false;
            lblNguyHiem.Visible = false;
            numericUpDown1.Visible = false;
            numericUpDown2.Visible = false;
            numericUpDown3.Visible = false;
            btnDone.Visible = false;
            label7.Text = "";
            btnCancel.Visible = false;
        }

        private void LoadIntroForm()
        {
            

        }

        private async void Start_Server()
        {
            if (_isListening)
            {
                return;
            }
            
            _isListening = true;

            //Khởi tạo TcpListener lắng nghe kết nối từ client
            IPAddress ipAddress = IPAddress.Parse(HOST);
            _tcpListener = new TcpListener(ipAddress, 5565);
            _tcpListener.Start();
            Console.WriteLine(ipAddress.ToString() + " Started!");

            
            while (_isListening)
            {
                if (_isListening)
                {
                    TcpClient client = await _tcpListener.AcceptTcpClientAsync();
                    if (client != null && client.Connected)
                    {
                        Console.WriteLine("Send to client: " + message);
                        await Task.Run(() => ProcessClient(client));
                    }
                }                
            }
        }

        private Task ProcessClient(TcpClient client)
        {
            using (client)
            {
                ReceiveData(client);
            }
            return Task.CompletedTask;
        }

        private void Load_Json()
        {
            _isListening = false;
            stopRecv = true;
            string[] jsonFiles = Directory.GetFiles(_jsonFolderPath, "*.json");            

            // Đặt kích thước hình ảnh mặc định
            imageList.ImageSize = new Size(255, 255);

            foreach (string jsonFile in jsonFiles)
            {
                // Lấy hình ảnh từ file JSON và chuyển đổi thành đối tượng Image
                string jsonString = File.ReadAllText(jsonFile);
                //dynamic obj = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonString);
                //string imageBase64String = obj.image;
                //byte[] imageBytes = Convert.FromBase64String(imageBase64String);
                //MemoryStream ms = new MemoryStream(imageBytes);
                //System.Drawing.Image image = System.Drawing.Image.FromStream(ms);

                //// Thêm hình ảnh vào ImageList
                //this.imageList.Images.Add(image);
                // Giải mã chuỗi JSON và truy cập các thuộc tính
                JsonDocument doc = JsonDocument.Parse(jsonString);
                JsonElement root = doc.RootElement;
                string image_data = root.GetProperty("image").GetString();

                // Lấy ảnh từ dữ liệu nhận được và hiển thị trên pictureBox
                byte[] imgData = Convert.FromBase64String(image_data);
                using (MemoryStream ms = new MemoryStream(imgData))
                {
                    System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
                    this.imageList.Images.Add(image);
                }
            }
            Console.WriteLine("Imagelist.count = " + imageList.Images.Count);
        }

        private void ReceiveData(TcpClient client)
        {
            if (stopRecv == true)
            {
                message = "Stop";
            }
            
            try
            {
                // Accept a client connection
                NetworkStream stream = client.GetStream();

                // Send a response to the client to notify that the server is ready to receive data
                byte[] response = Encoding.ASCII.GetBytes(message);;
                stream.Write(response, 0, response.Length);

                // Nhận dữ liệu từ client và giải mã chuỗi JSON
                StringBuilder sb = new StringBuilder();
                byte[] buffer = new byte[1024];
                int bytesRead = 0;
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    sb.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));

                }

                if (sb.Length > 0)
                {
                    string json_data = sb.ToString();

                    // Giải mã chuỗi JSON và truy cập các thuộc tính
                    JsonDocument doc = JsonDocument.Parse(json_data);
                    JsonElement root = doc.RootElement;
                    string image_data = root.GetProperty("image").GetString();
                    string str_data = root.GetProperty("string_data").GetString();

                    // Lấy ảnh từ dữ liệu nhận được và hiển thị trên pictureBox
                    byte[] imgData = Convert.FromBase64String(image_data);
                    using (MemoryStream ms = new MemoryStream(imgData))
                    {
                        System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
                        currentImage = image;
                    }

                    // Lấy chuỗi string từ dữ liệu nhận được và hiển thị trong một MessageBox
                    string[] numbers = str_data.Trim('[', ']').Split(',');
                    countRecycle = int.Parse(numbers[0]);
                    countDangerous = int.Parse(numbers[1]);
                    countOther = int.Parse(numbers[2]);

                    setVisual();

                    string jsonString = sb.ToString();
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".json";
                    string filePath = Path.Combine(_jsonFolderPath, fileName);

                    File.WriteAllText(filePath, jsonString);
                    client.Close();
                } 
            }catch (Exception e) 
            {
                MessageBox.Show("Error: " + e.Message);
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

        //Cross-thread operation not valid: Control '' accessed from a thread other than the thread it was created on.'
        private void setVisual()
        {
            //Invoke để đồng bộ hóa và có thể chạy được UI
            this.Invoke((MethodInvoker)delegate {
                label1.Text = "DETECTING";
                btnCancel.Visible = false;
                Enable();
                System.Drawing.Image resizedImage = ResizeImage(currentImage, 439, 303);
                // Set hiển thị image và số rác
                this.pictureBox1.Image = resizedImage;
                this.numericUpDown1.Value = countRecycle;
                this.numericUpDown2.Value = countDangerous;
                this.numericUpDown3.Value = countOther;
            });
            
        }

        public void Enable()
        {
            pictureBox1.Visible = true;
            //txt_Username.Visible = true;
            label7.Visible = true;
            lblTaiChe.Visible = true;
            lblLoaiKhac.Visible = true;
            lblNguyHiem.Visible = true;
            numericUpDown1.Visible = true;
            numericUpDown2.Visible = true;
            numericUpDown3.Visible = true;
            btnDone.Visible = true;
        }

        private void Disable()
        {
            label1.Text = "";
            pictureBox1.Visible = false;
            label7.Visible = false;
            lblTaiChe.Visible = false;
            lblLoaiKhac.Visible = false;
            lblNguyHiem.Visible = false;
            numericUpDown1.Visible = false;
            numericUpDown2.Visible = false;
            numericUpDown3.Visible = false;
            btnDone.Visible = false;
        }

        private void BtnDone_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn Chắn Chắn Đã Hoàn Thành?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Load_Json();
                Disable();
                frmSubmit frmSubmit = new frmSubmit();
                frmSubmit.setNumTrash(countRecycle, countDangerous, countOther);
                frmSubmit.setImageList(imageList);
                frmSubmit.TopLevel = false;
                frmSubmit.AutoScroll = true;
                frmSubmit.FormBorderStyle = FormBorderStyle.None;
                frmSubmit.Dock = DockStyle.Fill;
                frmSubmit.Show();
                frmSubmit.setImageList(imageList);
                panelMain.Controls.Add(frmSubmit);
                frmSubmit.LoadListImage();

                frmInputId frmInput = new frmInputId();
                frmInput.setNumTrash(countRecycle, countDangerous, countOther);
                frmInput.TopLevel = false;
                frmInput.AutoScroll = true;
                frmInput.FormBorderStyle = FormBorderStyle.None;
                frmInput.Dock = DockStyle.Fill;
                frmInput.Show();
                panelMain.Controls.Add(frmInput);

                // Đăng ký sự kiện Closed của form con
                frmInput.Closed += ChildForm_Closed;
            }            
        }

        private void ChildForm_Closed(object sender, EventArgs e)
        {
            // Hiển thị lại Label khi form con đã đóng
            label1.Text = "CHÀO MỪNG BẠN ĐẾN VỚI GREEN AI";
            label1.Location = new Point(143, 131);
            btnStart.Visible = true;
            label5.Visible = true;
            stopRecv = false;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Visible = false;
            label5.Visible = false;
            stopRecv = false;
            label1.Text = "Xác nhận người dùng...";
            label1.Location = new Point(20, 45);
            btnCancel.Visible = true;
            btnCancel.Location = new Point(578, 464);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _isListening = false;
            Disable();
            // Hiển thị lại Label khi form con đã đóng
            label1.Text = "CHÀO MỪNG BẠN ĐẾN VỚI GREEN AI";
            label1.Location = new Point(143, 131);
            btnStart.Visible = true;
            label5.Visible = true;
            btnCancel.Visible = false;
        }
    }
}
