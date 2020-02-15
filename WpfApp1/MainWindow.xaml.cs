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
    ///f
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
            var client = new RestClient("apage");

            //  create new restrequest class, withe the rest of the url we want filled in
            var request = new RestRequest("api/project");

            // we define the method
            request.Method = Method.GET;

            // add the header Accept
            request.AddHeader("Accept", "application/json");

            // add the default header with authorization needed
            client.AddDefaultHeader("Authorization",
                string.Format("Bearer {0}", "TOKEN"));

            // we execute and get the content
            var response = client.Execute(request);


            var deserialize = new JsonDeserializer();
            ProjectList output = deserialize.Deserialize<ProjectList>(response);


            Project firstProjectInResponse = output.Projects[0];

            tb.Text = firstProjectInResponse.ToString();
            
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