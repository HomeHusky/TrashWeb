using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using trashwebWinForm.Common.Const;
using trashwebWinForm.Controllers;
using trashwebWinForm.Models.DAO;
using trashwebWinForm.Models.DTO;

namespace trashwebWinForm
{
    public partial class frmInputId : Form
    {
        // Sự kiện trả về mảng string cho form Cha
        public delegate void MyEventHandler(string myString);
        public event MyEventHandler frmInputId_getString;

        // Tạo 3 biến lưu giá trị của rác
        private int countRecycle = 0;
        private int countDangerous = 0;
        private int countOther = 0;

        //Tạo controller để xử lý với database
        private website_trashlistController website_TrashlistController = new website_trashlistController();
        private website_trashdetailController website_TrashdetailController = new website_trashdetailController();
        private website_customerController website_CustomerController = new website_customerController();
        website_customerDTO customerDTO = new website_customerDTO();

        public frmInputId()
        {
            InitializeComponent();
        }

        //Check all textbox
        private Boolean ElementCheck()
        {
            if (txt_Id.Text == string.Empty)
            {
                MessageBox.Show("Vui lòng nhập ID", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;

        }

        public void setNumTrash(int recycle, int dangerous, int other)
        {
            this.countRecycle = recycle;
            this.countDangerous = dangerous;
            this.countOther = other;
        }

        public static int GenerateID()
        {
            Random random = new Random();
            return random.Next(0, 10000);
        }

        public int countTrash()
        {
            int count = 0;
            if (countRecycle != 0) count = 1;
            if (countOther != 0) count = 2;
            if (countDangerous != 0) count = 3;
            return count;
        }

        public float SumPoint()
        {
            return (countRecycle * POINTS.RECYCLE + countDangerous * POINTS.DANGEROUS + countOther * POINTS.OTHER);
        }

        private void frmInputId_Load(object sender, EventArgs e)
        {

        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            //Check các giá trị nhập vào
            if (!ElementCheck()) return;


            customerDTO = website_CustomerController.GetCustomerById(txt_Id.Text);

            if (customerDTO == null)
            {
                MessageBox.Show("Id sai, Mời nhập lại!");
            }
            else
            {
                //try
                //{
                //Create trashdetail
                website_trashdetailDTO trashdetailDTO = new website_trashdetailDTO();
                trashdetailDTO.ID = GenerateID();
                trashdetailDTO.Recycle = countRecycle;
                trashdetailDTO.Dangerous = countDangerous;
                trashdetailDTO.Othergarbage = countOther;
                trashdetailDTO.Description = "None";
                trashdetailDTO.Iduser_id = customerDTO.ID;
                website_TrashdetailController.CreateTrashDetailByIdCustomer(trashdetailDTO);

                //Create trashlist
                website_trashlistDTO trashlistDTO = new website_trashlistDTO();
                trashlistDTO.ID = trashdetailDTO.ID;
                trashlistDTO.Trash_detail_id = trashdetailDTO.ID;
                trashlistDTO.Iduser_id = customerDTO.ID;
                trashlistDTO.Totalscore = SumPoint();
                trashlistDTO.Description = "None";
                trashlistDTO.Createat = DateTime.Now;
                trashlistDTO.Numoftrash = countTrash();
                website_TrashlistController.CreateTrashListByIdCustomer(trashlistDTO);

                List<double> listScoreUser = website_TrashlistController.GetTotalScoreById(customerDTO.ID);

                double totalScore = 0;
                for (int i = 0; i < listScoreUser.Count; i++)
                {
                    totalScore += listScoreUser[i];
                }

                website_CustomerController.UpdateCustomerPointById(totalScore, txt_Id.Text);

                frmInputId_getString(customerDTO.ID);
                MessageBox.Show("Thành Công!");
                this.Close();
            }
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Đã xảy ra lỗi khi cập nhật điểm! \n" + ex.Message);
            //}
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
