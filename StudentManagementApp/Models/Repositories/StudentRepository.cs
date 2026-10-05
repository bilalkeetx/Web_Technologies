using Microsoft.Data.SqlClient;
using System.Data;

namespace StudentManagementApp.Models.Repositories
{
    public class StudentRepository
    {
        private readonly string connectionString =
            @"Data Source=(localdb)\MSSQLLocalDB;
              Initial Catalog=AttendanceDB;
              Integrated Security=True;
              Connect Timeout=30;
              Encrypt=True;
              Trust Server Certificate=False;
              Application Intent=ReadWrite;
              Multi Subnet Failover=False;
              Command Timeout=30";


        public async Task<List<Student>> GetAllStudentsAsync()
        {
            List<Student> students = new List<Student>();

            await Task.Run(() =>
            {
                string selectQuery =
                    "SELECT * FROM Students";


                SqlDataAdapter adapter =
                    new SqlDataAdapter(
                        selectQuery,
                        connectionString
                    );


                DataTable table =
                    new DataTable();


                adapter.Fill(table);


                foreach (DataRow row in table.Rows)
                {
                    Student student =
                        new Student
                        {
                            StudentId =
                                Convert.ToInt32(
                                    row["StudentId"]
                                ),

                            StudentName =
                                row["StudentName"].ToString(),

                            RegistrationNumber =
                                row["RegistrationNumber"].ToString(),

                            Department =
                                row["Department"].ToString(),

                            AttendancePercentage =
                                Convert.ToDecimal(
                                    row["AttendancePercentage"]
                                )
                        };


                    students.Add(student);
                }
            });


            return students;
        }



        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            Student? student = null;


            await Task.Run(() =>
            {
                string selectQuery =
                    "SELECT * FROM Students";


                SqlDataAdapter adapter =
                    new SqlDataAdapter(
                        selectQuery,
                        connectionString
                    );


                DataTable table =
                    new DataTable();


                adapter.Fill(table);


                foreach (DataRow row in table.Rows)
                {
                    if (Convert.ToInt32(row["StudentId"]) == id)
                    {
                        student =
                            new Student
                            {
                                StudentId =
                                    Convert.ToInt32(
                                        row["StudentId"]
                                    ),

                                StudentName =
                                    row["StudentName"].ToString(),

                                RegistrationNumber =
                                    row["RegistrationNumber"].ToString(),

                                Department =
                                    row["Department"].ToString(),

                                AttendancePercentage =
                                    Convert.ToDecimal(
                                        row["AttendancePercentage"]
                                    )
                            };


                        break;
                    }
                }
            });


            return student;
        }


        public async Task AddStudentAsync(Student student)
        {
            await Task.Run(() =>
            {
                // SELECT COMMAND

                string selectQuery =
                    "SELECT * FROM Students";


                SqlDataAdapter adapter =
                    new SqlDataAdapter(
                        selectQuery,
                        connectionString
                    );

                // INSERT COMMAND

                string insertQuery = @"
                    INSERT INTO Students
                    (
                        StudentName,
                        RegistrationNumber,
                        Department,
                        AttendancePercentage
                    )
                    VALUES
                    (
                        @StudentName,
                        @RegistrationNumber,
                        @Department,
                        @AttendancePercentage
                    )";


                SqlCommand insertCommand =
                    new SqlCommand(
                        insertQuery,
                        new SqlConnection(connectionString)
                    );

                // PARAMETERS

                insertCommand.Parameters.Add(
                    "@StudentName",
                    SqlDbType.NVarChar,
                    100,
                    "StudentName"
                );


                insertCommand.Parameters.Add(
                    "@RegistrationNumber",
                    SqlDbType.NVarChar,
                    30,
                    "RegistrationNumber"
                );


                insertCommand.Parameters.Add(
                    "@Department",
                    SqlDbType.NVarChar,
                    50,
                    "Department"
                );


                SqlParameter attendanceParameter =
                    insertCommand.Parameters.Add(
                        "@AttendancePercentage",
                        SqlDbType.Decimal,
                        0,
                        "AttendancePercentage"
                    );


                // Tell adapter which command to use
                // for Added rows.

                adapter.InsertCommand =
                    insertCommand;



                DataTable table =
                    new DataTable();


                adapter.Fill(table);

                DataRow newRow =
                    table.NewRow();


                newRow["StudentName"] =
                    student.StudentName;

                newRow["RegistrationNumber"] =
                    student.RegistrationNumber;

                newRow["Department"] =
                    student.Department;

                newRow["AttendancePercentage"] =
                    student.AttendancePercentage;

                table.Rows.Add(newRow);




                adapter.Update(table);
            });
        }




        public async Task UpdateStudentAsync(Student student)
        {
            await Task.Run(() =>
            {
                // SELECT

                string selectQuery =
                    "SELECT * FROM Students";


                SqlDataAdapter adapter =
                    new SqlDataAdapter(
                        selectQuery,
                        connectionString
                    );

                // UPDATE SQL

                string updateQuery = @"
                    UPDATE Students

                    SET StudentName = @StudentName,
                        RegistrationNumber = @RegistrationNumber,
                        Department = @Department,
                        AttendancePercentage = @AttendancePercentage

                    WHERE StudentId = @StudentId";


                SqlCommand updateCommand =
                    new SqlCommand(
                        updateQuery,
                        new SqlConnection(connectionString)
                    );

                // UPDATE PARAMETERS

                updateCommand.Parameters.Add(
                    "@StudentName",
                    SqlDbType.NVarChar,
                    100,
                    "StudentName"
                );


                updateCommand.Parameters.Add(
                    "@RegistrationNumber",
                    SqlDbType.NVarChar,
                    30,
                    "RegistrationNumber"
                );


                updateCommand.Parameters.Add(
                    "@Department",
                    SqlDbType.NVarChar,
                    50,
                    "Department"
                );


                SqlParameter attendanceParameter =
                    updateCommand.Parameters.Add(
                        "@AttendancePercentage",
                        SqlDbType.Decimal,
                        0,
                        "AttendancePercentage"
                    );


                attendanceParameter.Precision = 5;
                attendanceParameter.Scale = 2;


                SqlParameter idParameter =
                    updateCommand.Parameters.Add(
                        "@StudentId",
                        SqlDbType.Int,
                        0,
                        "StudentId"
                    );


                idParameter.SourceVersion =
                    DataRowVersion.Original;


                adapter.UpdateCommand =
                    updateCommand;

                // FILL DATATABLE

                DataTable table =
                    new DataTable();


                adapter.Fill(table);

                // FIND STUDENT

                foreach (DataRow row in table.Rows)
                {
                    if (
                        Convert.ToInt32(row["StudentId"])
                        == student.StudentId
                    )
                    {

                        row["StudentName"] =
                            student.StudentName;

                        row["RegistrationNumber"] =
                            student.RegistrationNumber;

                        row["Department"] =
                            student.Department;

                        row["AttendancePercentage"] =
                            student.AttendancePercentage;


                        break;
                    }
                }


                adapter.Update(table);
            });
        }


        // 5. DELETE STUDENT
        public async Task DeleteStudentAsync(int id)
        {
            await Task.Run(() =>
            {
                // SELECT

                string selectQuery =
                    "SELECT * FROM Students";


                SqlDataAdapter adapter =
                    new SqlDataAdapter(
                        selectQuery,
                        connectionString
                    );


                // DELETE COMMAND

                string deleteQuery =
                    @"DELETE FROM Students
                      WHERE StudentId = @StudentId";


                SqlCommand deleteCommand =
                    new SqlCommand(
                        deleteQuery,
                        new SqlConnection(connectionString)
                    );


                // DELETE PARAMETER

                SqlParameter deleteId =
                    deleteCommand.Parameters.Add(
                        "@StudentId",
                        SqlDbType.Int,
                        0,
                        "StudentId"
                    );


                deleteId.SourceVersion =
                    DataRowVersion.Original;


                adapter.DeleteCommand =
                    deleteCommand;



                DataTable table =
                    new DataTable();


                adapter.Fill(table);



                foreach (DataRow row in table.Rows)
                {
                    if (
                        Convert.ToInt32(row["StudentId"])
                        == id
                    )
                    {
                        row.Delete();


                        break;
                    }
                }


                
                adapter.Update(table);
            });
        }
    }
}