using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Data.SqlClient;
using System.Data;

namespace UserCrud
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadGrid();
        }

        SqlConnection con = new SqlConnection(@"Data Source=MSI;Initial Catalog=UserCRUD;Integrated Security=True");

        public void ClearData()
        {
            name_txt.Clear();
            password_txt.Clear();
            email_txt.Clear();
            search_txt.Clear();
        }
        public void LoadGrid()
        {
            SqlCommand cmd = new SqlCommand("select * from [User]", con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            dt.Load(reader);
            con.Close();
            datagrid.ItemsSource = dt.DefaultView;
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearData();
        }

        public bool isValid()
        {
            if(name_txt.Text == string.Empty)
            {
                MessageBox.Show("Name Is Required","Faild",MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (email_txt.Text == string.Empty)
            {
                MessageBox.Show("Email Is Required", "Faild", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (password_txt.Text == string.Empty)
            {
                MessageBox.Show("Password Is Required", "Faild", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isValid())
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO [User] VALUES (@Name, @Email, @Password)", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Name", name_txt.Text);
                    cmd.Parameters.AddWithValue("@Email", email_txt.Text);
                    cmd.Parameters.AddWithValue("@Password", password_txt.Text);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    LoadGrid();

                    MessageBox.Show("Data Has Been Added", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearData();
                }
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("delete From [User] where ID = " +search_txt.Text+"", con);
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data Has Been Deleted", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                con.Close();
                ClearData();
                LoadGrid();
                con.Close();
            } catch (SqlException ex)
            {
                MessageBox.Show("Data Fail To Deleted"+ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("Update [User] set Name=@Name, Email=@Email, Password=@Password "+ "Where ID = @id", con);
            cmd.Parameters.AddWithValue("@Name", name_txt.Text);
            cmd.Parameters.AddWithValue("@Email", email_txt.Text);
            cmd.Parameters.AddWithValue("@Password", password_txt.Text);
            cmd.Parameters.AddWithValue("@id", search_txt.Text);
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data Has Been Updated","Success",MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
                ClearData();
                LoadGrid();
            }
        }
    }

}
