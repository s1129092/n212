using System.Collections.Generic;
using System.Reflection.Emit;
using System.Windows;
using RestSharp;
using RestSharp.Serialization.Json;
using DataFormat = RestSharp.DataFormat;
using System.Web;
using System.Web.UI.WebControls;
using Newtonsoft.Json.Linq;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///
    ///
    // class that acts like a session
    // if it's null we do not allow them to login


    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Submit_Button_Click(object sender, RoutedEventArgs e)
        {

            if (Application.Current.Properties["ID"] == null)
            {
                MessageBox.Show("NOT LOGGED IN");

                return;
            }

            // create new restclient class, with the base url filled in the ()
            var client = new RestClient("http://scrum.clow.nl/");

            //  create new restrequest class, withe the rest of the url we want filled in
            var request = new RestRequest("api/project");

            // we define the method
            request.Method = Method.GET;

            // add the header Accept
            request.AddHeader("Accept", "application/json");

            // add the default header with authorization needed
            client.AddDefaultHeader("Authorization",
                string.Format("Bearer {0}", "0ecda346-571d-3dfc-8c8c-c0648cce87d7"));

            // we execute and get the content
            var response = client.Execute(request);


            var deserialize = new JsonDeserializer();
            ProjectList output = deserialize.Deserialize<ProjectList>(response);


            Project firstProjectInResponse = output.Projects[0];

            tb.Text = firstProjectInResponse.ToString();
            
        }

        private void login_click_1(object sender, RoutedEventArgs e)
        {
            // create new restclient class, with the base url filled in the ()
            var client = new RestClient("http://scrum.clow.nl/");

            //  create new restrequest class, withe the rest of the url we want filled in
            var request = new RestRequest("api/auth/signin");

            request.RequestFormat = DataFormat.Json;
            request.AddBody(new {email = "s1129092@student.windesheim.nl", password = "jojo1234"});

            // we define the method
            request.Method = Method.POST;

            // add the header Accept
            request.AddHeader("Accept", "application/json");

            // we execute and get the content
            var response = client.Execute(request);


            if (response.StatusCode.ToString() == "OK")
            {
                MessageBox.Show("User is allowed to login");

                Application.Current.Properties["ID"] = 1;

                int id = int.Parse(Application.Current.Properties["ID"].ToString());


            } else if (response.StatusCode.ToString() == "422")
            {
                MessageBox.Show("Incorrect login details");
            }
            else
            {
                MessageBox.Show("ERROR");
            }
        }

        private void logout_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Properties.Remove("ID");
        }


    }

    class Project
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string Deadline { get; set; }
        public int User_id { get; set; }
        public string Created_at { get; set; }
        public string Updated_at { get; set; }


    }

    class ProjectList
    {
        public Project[] Projects { get; set; }
    }

}