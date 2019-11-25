using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CourseWorkLibraryV2;
using Microsoft.Win32;

//////////////////////////////////////////////////////////////
// File: MainWindow.xaml.cs                                 //
//                                                          //
// Purpose: Contains Methods & Querires to populate GUI     //
//                                                          //
// Written By: Earl Platt III                               //
//                                                          //
// Compiler: Visual Studio 2019                             //
//                                                          //
//////////////////////////////////////////////////////////////

namespace CourseWorkGUIv2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region
        //Variables
        string fileNameRead = " ";
        SqlConnection sqlConn;

        //Creates CourseWork Object
        CourseWork work = new CourseWork();
        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }



        #region
        /// <summary>
        /// Method to Import Json File
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void importFileMenuItem_Click(object sender, RoutedEventArgs e)
        {

            ///////////////////////////////////////////////////
            /// Read Json File In 
            /// From Explorer
            //////////////////////////////////////////////////
            
            OpenFileDialog ofd = new OpenFileDialog();

            //Opens Windows Explorer
            ofd.InitialDirectory = @"C:\Users\sickl\Documents\BCS426\CourseWorkGUI\CourseWorkGUI\bin\Debug";
            ofd.Title = "Find JSON file source";

            //Checks Windows Explorer For File Based on Conditions
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Filter = "(*.json) | *.json";
            ofd.FilterIndex = 2;
            ofd.RestoreDirectory = true;
            ofd.ReadOnlyChecked = true;
            ofd.ShowReadOnly = true;


            if (ofd.ShowDialog() == true)
            {
               
               fileNameRead = ofd.FileName;
            }
            else { return; }

            //Reads File to End
            FileStream read = new FileStream(fileNameRead, FileMode.Open, FileAccess.Read);
            StreamReader streamReader = new StreamReader(read, Encoding.UTF8);
            string jsonString = streamReader.ReadToEnd();

            //Creates a Byte Array from the Json String
            byte[] byteArray = Encoding.UTF8.GetBytes(jsonString);
            MemoryStream stream = new MemoryStream(byteArray);

            //Populates Libary with Json Data
            DataContractJsonSerializer inputSerializer;
            inputSerializer = new DataContractJsonSerializer(typeof(CourseWork));
            work = (CourseWork)inputSerializer.ReadObject(stream);
            stream.Close();
            

            #region
            //////////////////////////////////////////////
            /// Process To Load Data Into Database
            /// Uses Foreach Loops to Populate Database
            /////////////////////////////////////////////

            foreach (Submission sub in work.Submissions)
            {
                /////////////////////////////////////////////////////////
                /// Connection to the Database
                //////////////////////////////////////////////////////

                string connString = @"server=(LocalDB)\MSSQLLocalDB;" +
                "integrated security=SSPI;" +
                "database=CourseWork;" + "MultipleActiveresultSets = True";

                //Inserts Values from DLL into Database
                string sql = string.Format("INSERT INTO Submissions" +
                        "(AssignmentName, CategoryName, Grade) Values" +
                        "('{0}', '{1}', '{2}')", sub.AssignmentName, sub.CategoryName, sub.Grade);
                
                sqlConn = new SqlConnection(connString);
                sqlConn.Open();

                SqlCommand command = new SqlCommand(sql, sqlConn);
                int rowsAffected = command.ExecuteNonQuery();

                //Selects All Assignments from Database
                string sqls = "SELECT AssignmentName FROM Submissions;";
                SqlCommand commands = new SqlCommand(sqls, sqlConn);
                SqlDataReader reader = commands.ExecuteReader();

                reader.Read();

                int fieldCount = reader.FieldCount;

                //Loop Adds Assignments Queried from Database into ListBox
                while (reader.Read())
                {
                    submissionsListBox.Items.Add(reader["AssignmentName"]);
                }

               
            }
            #endregion

        }
        #endregion

        #region
        /// <summary>
        /// Method to Display About Pop-up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string s;
            s = "Course Work GUI \n Version 2.0 \n Written by Earl Platt III";
            MessageBox.Show(s);
        }
        #endregion

        #region
        /// <summary>
        /// Method to display AssignmentName, CategoryName & Grade in text Box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void submissionsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selected = submissionsListBox.SelectedItem.ToString();

            string sqls = "SELECT AssignmentName, CategoryName, Grade FROM Submissions WHERE AssignmentName =" 
                + "'" + selected + "'" + ";";
            SqlCommand commands = new SqlCommand(sqls, sqlConn);
            SqlDataReader reader = commands.ExecuteReader();
            reader.Read();
            int fieldCount = reader.FieldCount;
            assignmentNameTextBox.Text = reader["AssignmentName"].ToString();
            categoryNameTextBox.Text = reader["CategoryName"].ToString();
            gradeTextBox.Text = reader["Grade"].ToString();
        }
        #endregion

        #region
        /// <summary>
        /// Method to Exit Program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemExit_Click(object sender, RoutedEventArgs e)
        {
            //Command to delete all rows from table
            //string exitSql = "DELETE FROM Submissions;";
            //SqlCommand comm = new SqlCommand(exitSql, sqlConn);
            //int rowsAffected = comm.ExecuteNonQuery();
            System.Environment.Exit(1);
        }
        #endregion
    }
}
