using Net.Code.ADONet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace formtest
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        EnityState objState = EnityState.Unchanged;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using (var db = Db.FromConfig("cn"))
            {
                studentBindingSource.DataSource = db.Sql("select * from Students").AsEnumerable<Student>();

            }
            pContainer.Enabled = false;
            Student obj = studentBindingSource.Current as Student;
            if( obj !=null)
            {
                if( !string.IsNullOrEmpty(obj.ImageUrl))
                {
                    try
                    {
                        pictureBox1.Image = Image.FromFile(obj.ImageUrl);
                    }
                    catch(Exception ex)
                    {

                    }
                }
            }
        }

        private void metroPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void metroTextBox2_Click(object sender, EventArgs e)
        {

        }

        private void metroLabel5_Click(object sender, EventArgs e)
        {

        }

        private void metroLabel6_Click(object sender, EventArgs e)
        {

        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            using(OpenFileDialog ofd = new OpenFileDialog() {  Filter ="JPEG|*.jpg|PNG|*.png", ValidateNames = true})
            {
                if( ofd.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = Image.FromFile(ofd.FileName);
                    Student obj = studentBindingSource.Current as Student;
                    if (obj != null)
                        obj.ImageUrl = ofd.FileName;
                }
            }
        }
        void ClearInput()
        {
            txtFullname.Text = null;
            txtEmail.Text = null;
            txtAddress.Text = null;
            chkGender.Checked = false;
            txtBirthday.Text = null;
            pictureBox1.Image = null;
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            if( MetroFramework.MetroMessageBox.Show(this,"sure?","Message",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
            {
                objState = EnityState.Deleted;
                try
                {
                    Student obj = studentBindingSource.Current as Student;
                    if( obj != null)
                    {
                        using (var db = Db.FromConfig("cn"))
                        {
                            db.Sql("delete from Students where StudentID = @StudentID").WithParameters(new { StudentID = obj.StudentID }).AsNonQuery();
                            studentBindingSource.RemoveCurrent();
                            pContainer.Enabled = false;
                            ClearInput();
                            objState = EnityState.Unchanged;
                        }
                    }
                }
                 catch(Exception ex)
                {

                }
            }
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            pContainer.Enabled = false;
            studentBindingSource.ResetBindings(false);
            ClearInput();
        }

        private void metroButton5_Click(object sender, EventArgs e)
        {
            objState = EnityState.Added;
            pictureBox1.Image = null;
            pContainer.Enabled = true;
            studentBindingSource.Add(new Student());
            studentBindingSource.MoveLast();
            txtFullname.Focus();

        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            objState = EnityState.Changed;
            pContainer.Enabled = true;
            txtFullname.Focus();


        }

        private void metroButton6_Click(object sender, EventArgs e)
        {
            try
            {
                studentBindingSource.EndEdit();
                Student ob = studentBindingSource.Current as Student;
                ob.FullName = txtFullname.Text;
                ob.Email = txtEmail.Text;
                ob.Address = txtAddress.Text;
                ob.Gender = chkGender.Checked;
                ob.Birthday = txtBirthday.Text;
                if (ob != null)
                {
                    using (var db = Db.FromConfig("cn"))
                    {
                        if (objState == EnityState.Added)
                        {
                            ob.StudentID = db.Sql("insert into Students([FullName],Birthday,Gender,Email,[Address],ImageUrl)values(@FullName, @Birthday, @Gender, @Email, @Address, @ImageUrl); select SCOPE_IDENTITY()").WithParameters(
                                new { FullName = ob.FullName, Birthday = ob.Birthday, Gender = ob.Gender, Email = ob.Email, Address = ob.Address, ImageUrl = ob.ImageUrl }).AsScalar<int>();
                        }
                        else if (objState == EnityState.Changed)
                        {
                            db.StoredProcedure("sp_Students_Update").WithParameters(new { FullName = ob.FullName, Birthday = ob.Birthday, Gender = ob.Gender, Email = ob.Email, Address = ob.Address, ImageUrl = ob.ImageUrl }).AsNonQuery();
                        }
                        metroGrid1.Refresh();
                        pContainer.Enabled = false;
                        objState = EnityState.Unchanged;
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void metroGrid1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Student obj = studentBindingSource.Current as Student;
            if (obj != null)
            {
                if (!string.IsNullOrEmpty(obj.ImageUrl))
                {
                    try
                    {
                        pictureBox1.Image = Image.FromFile(obj.ImageUrl);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        private void txtStudentID_Click(object sender, EventArgs e)
        {

        }

        private void chkGender_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtEmail_Click(object sender, EventArgs e)
        {

        }
    }
}
