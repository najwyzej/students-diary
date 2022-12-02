using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentDiary
{
    public partial class AddEditStudent : Form
    {

        private int _studentId;
        private Student _student;        


        private FileHelper<List<Student>> _fileHelper =
            new FileHelper<List<Student>>(Program.FilePath);

        public AddEditStudent(int id = 0)
        {
            InitializeComponent();
            _studentId = id;


            GetStudentData();
            tbFirstname.Select();


        }


        private void GetStudentData()
        {
            if (_studentId != 0)
            {
                Text = "Edytowanie danych ucznia";
                var students = _fileHelper.DeserializeFromFile();
                _student = students.FirstOrDefault(x => x.Id == _studentId);

                if (_student == null)
                    throw new Exception("Brak użytkownika o podanym Id.");

                cbGroupId.Items.AddRange(Groups.names.ToArray());

                FillTextBoxes();

            }
        }

        private void FillTextBoxes()
        {
            tbId.Text = _student.Id.ToString();
            tbFirstname.Text = _student.Name;
            tbLastname.Text = _student.LastName;
            tbMath.Text = _student.Math;
            tbTechnology.Text = _student.Technology;
            tbPhysics.Text = _student.Physics;
            tbLanguage.Text = _student.Language;
            tbForeignLanguage.Text = _student.ForeignLanguage;
            rtbComments.Text = _student.Comments;
            cbAdditionalClasses.Checked = _student.AdditionalClasses;
            cbGroupId.SelectedItem = _student.GroupId;

        }

        private async void btnConfirm_Click(object sender, EventArgs e)
        {
            var students = _fileHelper.DeserializeFromFile();

            if (string.IsNullOrEmpty(cbGroupId.Text))
            {
                MessageBox.Show("Wybierz z listy klasę ucznia.");
            }
            else
            {

                if (_studentId != 0)
                    students.RemoveAll(x => x.Id == _studentId);
                else
                    AssignIdToNewStudent(students);

                AddNewUserToList(students);

                _fileHelper.SerializeToFile(students);

                await LongProcessAsync();

                Close();
            }
        }

        private async Task LongProcessAsync()
        {
            await Task.Run(() =>
            {

                Thread.Sleep(1000);
            });
        }

        private void AddNewUserToList(List<Student> students)
        {
            var student = new Student
            {
                Id = _studentId,
                GroupId = cbGroupId.SelectedItem.ToString(),
                Name = tbFirstname.Text,
                LastName = tbLastname.Text,
                Technology = tbTechnology.Text,
                Physics = tbPhysics.Text,
                Math = tbMath.Text,
                Language = tbLanguage.Text,
                ForeignLanguage = tbForeignLanguage.Text,
                Comments = rtbComments.Text,
                AdditionalClasses = cbAdditionalClasses.Checked,

            };
            students.Add(student);
        }

        private void AssignIdToNewStudent(List<Student> students)
        {
            var studentWithHighestId = students
                .OrderByDescending(x => x.Id).FirstOrDefault();


            _studentId = studentWithHighestId == null ?
                1 : studentWithHighestId.Id + 1;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AddEditStudent_Load(object sender, EventArgs e)
        {
            cbGroupId.Items.AddRange(Groups.names.ToArray());
        }
    }
}
