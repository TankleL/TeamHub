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
    /// Sysmenu.xaml 的交互逻辑
    /// </summary>
    public partial class Sysmenu : UserControl
    {
        public Sysmenu()
        {
            InitializeComponent();
        }

        // Internal Events
        #region Internal Events

        private void btn_close_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (null != _closeButtonClickEventHandler)
                _closeButtonClickEventHandler(sender, e);
        }

        private void btn_max_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (null != _maxButtonClickEventHandler)
                _maxButtonClickEventHandler(sender, e);
        }

        private void btn_min_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (null != _minButtonClickEventHandler)
                _minButtonClickEventHandler(sender, e);
        }

        #endregion // Internal Events


        // Methods
        #region Methods

        public void SetMaximizeIcon()
        {
            btn_max.Source = new BitmapImage(new Uri("pack://siteoforigin:,,,/Resources/img_maximize_btn.png", UriKind.RelativeOrAbsolute));
        }

        public void SetNormalizeIcon()
        {
            btn_max.Source = new BitmapImage(new Uri("pack://siteoforigin:,,,/Resources/img_normalize_btn.png", UriKind.RelativeOrAbsolute));
        }


        #endregion // Methods


        // EventHandlers
        #region EventHandlers
        private EventHandler _closeButtonClickEventHandler;
        public EventHandler closeButtonClickEventHandler
        {
            get { return _closeButtonClickEventHandler; }
            set { _closeButtonClickEventHandler = value; }
        }

        private EventHandler _maxButtonClickEventHandler;
        public EventHandler maxButtonClickEventHandler
        {
            get { return _maxButtonClickEventHandler; }
            set { _maxButtonClickEventHandler = value; }
        }

        private EventHandler _minButtonClickEventHandler;
        public EventHandler minButtonClickEventHandler
        {
            get { return _minButtonClickEventHandler; }
            set { _minButtonClickEventHandler = value; }
        }
        

        #endregion


    }
}
