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

namespace TeamHub_FrontEnd
{
    /// <summary>
    /// Connecting.xaml 的交互逻辑
    /// </summary>
    public partial class Connecting : Page
    {
        public Connecting()
        {
            InitializeComponent();
        }

        private void btn_Members_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            btn_Members.Visibility = Visibility.Hidden;
            btn_MembersHidden.Visibility = Visibility.Visible;

            btn_Broadcast.Visibility = Visibility.Visible;
            btn_BroadcastHidden.Visibility = Visibility.Hidden;
            btn_Board.Visibility = Visibility.Visible;
            btn_BoardHidden.Visibility = Visibility.Hidden;
            btn_Groups.Visibility = Visibility.Visible;
            btn_GroupsHidden.Visibility = Visibility.Hidden;
        }

        private void btn_Groups_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            btn_Groups.Visibility = Visibility.Hidden;
            btn_GroupsHidden.Visibility = Visibility.Visible;

            btn_Broadcast.Visibility = Visibility.Visible;
            btn_BroadcastHidden.Visibility = Visibility.Hidden;
            btn_Board.Visibility = Visibility.Visible;
            btn_BoardHidden.Visibility = Visibility.Hidden;
            btn_Members.Visibility = Visibility.Visible;
            btn_MembersHidden.Visibility = Visibility.Hidden;
        }

        private void btn_Broadcast_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            btn_Broadcast.Visibility = Visibility.Hidden;
            btn_BroadcastHidden.Visibility = Visibility.Visible;

            btn_Groups.Visibility = Visibility.Visible;
            btn_GroupsHidden.Visibility = Visibility.Hidden;
            btn_Members.Visibility = Visibility.Visible;
            btn_MembersHidden.Visibility = Visibility.Hidden;
            btn_Board.Visibility = Visibility.Visible;
            btn_BoardHidden.Visibility = Visibility.Hidden;
        }

        private void btn_Board_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            btn_Board.Visibility = Visibility.Hidden;
            btn_BoardHidden.Visibility = Visibility.Visible;

            btn_Broadcast.Visibility = Visibility.Visible;
            btn_BroadcastHidden.Visibility = Visibility.Hidden;
            btn_Groups.Visibility = Visibility.Visible;
            btn_GroupsHidden.Visibility = Visibility.Hidden;
            btn_Members.Visibility = Visibility.Visible;
            btn_MembersHidden.Visibility = Visibility.Hidden;
        }
    }
}
