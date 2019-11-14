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

namespace CourseWorkGUIv2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region
        //Variables
        string fileNameRead;

        //Creates CourseWork Object
        CourseWork work = new CourseWork();
        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Method to Exit Program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemExit_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(1);
        }

        
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
            #region
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


           // if (ofd.ShowDialog() == true)
          //  {
               // fileNameTextBox.Text = ofd.FileName;
               // fileNameRead = ofd.FileName;
          //  }
           // else { return; }

            //Reads File to End
            FileStream reader = new FileStream(fileNameRead, FileMode.Open, FileAccess.Read);
            StreamReader streamReader = new StreamReader(reader, Encoding.UTF8);
            string jsonString = streamReader.ReadToEnd();

            //Creates a Byte Array from the Json String
            byte[] byteArray = Encoding.UTF8.GetBytes(jsonString);
            MemoryStream stream = new MemoryStream(byteArray);

            //Populates Libary with Json Data
            DataContractJsonSerializer inputSerializer;
            inputSerializer = new DataContractJsonSerializer(typeof(CourseWork));
            work = (CourseWork)inputSerializer.ReadObject(stream);
            stream.Close();
            #endregion

            //////////////////////////////////////////////
            /// Process To Load Data Into Database
            /// Uses Foreach Loops to Populate Database
            /////////////////////////////////////////////
            foreach(Assignment ass in work.Assigments)
            {
                /////////////////////////////////////////////////////////
                /// Connection to the Database
                //////////////////////////////////////////////////////
                string connString = @"server=(LocalDB)\MSSQLLocalDB;" +
                "integrated security=SSPI;" +
                "database=Submissions" + "MultipleActiveresultSets = True";

                string sql = string.Format("INSERT INTO Submissions" + 
                "(AssignmentName) Values" + (work.Assigments.ToString()));

                SqlConnection sqlConn;
                sqlConn = new SqlConnection(connString);
                sqlConn.Open();

                SqlCommand command = new SqlCommand(sql, sqlConn);
                int rowsAffected = command.ExecuteNonQuery();
                
            }


            //////////////////////////////////////////////
            /// Process To Load Data Into GUI
            /// Uses Foreach Loops & Sets Texts Boxes
            /////////////////////////////////////////////
            #region
            /*/Populates ListView for Category
            foreach (Category cat in work.Categories)
            {
                categoriesListView.Items.Add(cat);
            }

            //Populates ListView for Assignment
            foreach (Assignment ass in work.Assigments)
            {
                assignmentsListView.Items.Add(ass);
            }

            //Populates ListView for Submission
            foreach (Submission sub in work.Submissions)
            {
                submissionListView.Items.Add(sub);
            }

            //Populates TextBox for Course Name
            courseNameBox.Text = work.CourseName;

            //Populates TextBox for Grade
            overallGradeTextBox.Text = work.CalculateGrade().ToString();
            #endregion*/
        }
        #endregion
    

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
    }
}
