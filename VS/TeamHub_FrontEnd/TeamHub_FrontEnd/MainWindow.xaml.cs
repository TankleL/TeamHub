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

using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace TeamHub_FrontEnd
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        // Enumerations
        #region Enumerations

        public enum ThemeColor : int
        {
            FileDriverTheme = 1,
            ConnectingTheme = 2,
            Teamwork = 3
        }

        #endregion // Enumerations

        // Constructors
        #region Constructors
        public MainWindow()
        {
            InitializeComponent();

            ucl_Sysmenu.closeButtonClickEventHandler += btn_close_click;
            ucl_Sysmenu.maxButtonClickEventHandler += btn_max_click;
            ucl_Sysmenu.minButtonClickEventHandler += btn_min_click;

            frm_Browser.NavigationUIVisibility = NavigationUIVisibility.Hidden;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;
            if (hwndSource != null)
                hwndSource.AddHook(new HwndSourceHook(this.WndProc));
        }

        #endregion // Constructors

        // Events
        #region Events

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetShadow();
        }

        private void frm_TitleFrame_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btn_close_click(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btn_max_click(object sender, EventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Normal)
            {
                this.WindowState = System.Windows.WindowState.Maximized;
                this.BorderThickness = new Thickness(0, 0, 0, 0);
                ucl_Sysmenu.SetNormalizeIcon();
            }
            else if (this.WindowState == System.Windows.WindowState.Maximized)
            {
                this.WindowState = System.Windows.WindowState.Normal;
                this.BorderThickness = new Thickness(this._customBorderThickness, this._customBorderThickness, this._customBorderThickness, this._customBorderThickness);
                ucl_Sysmenu.SetMaximizeIcon();
            }
        }
        
        private void btn_min_click(object sender, EventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void frm_TitleFrame_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btn_max_click(sender, e);
        }


        private void btn_FileDriver_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            btn_FileDriver.Foreground = Brushes.Black;
            frm_FileDriver.Opacity = 1.0;

            btn_Connecting.Foreground = Brushes.White;
            frm_Connecting.Opacity = 0.0;

            btn_Teamworks.Foreground = Brushes.White;
            frm_Teamworks.Opacity = 0.0;

            ChangedThemeColor(ThemeColor.FileDriverTheme);

            frm_Browser.Navigate(new Uri("FileDriver.xaml", UriKind.Relative));
            frm_Browser.NavigationService.RemoveBackEntry();
        }

        private void btn_Connecting_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            btn_FileDriver.Foreground = Brushes.White;
            frm_FileDriver.Opacity = 0.0;

            btn_Connecting.Foreground = Brushes.Black;
            frm_Connecting.Opacity = 1.0;

            btn_Teamworks.Foreground = Brushes.White;
            frm_Teamworks.Opacity = 0.0;

            ChangedThemeColor(ThemeColor.ConnectingTheme);

            frm_Browser.Navigate(new Uri("Connecting.xaml", UriKind.Relative));
            frm_Browser.NavigationService.RemoveBackEntry();
        }

        private void btn_Teamworks_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            btn_FileDriver.Foreground = Brushes.White;
            frm_FileDriver.Opacity = 0.0;

            btn_Connecting.Foreground = Brushes.White;
            frm_Connecting.Opacity = 0.0;

            btn_Teamworks.Foreground = Brushes.Black;
            frm_Teamworks.Opacity = 1.0;

            ChangedThemeColor(ThemeColor.Teamwork);

            frm_Browser.Navigate(new Uri("Teamworks.xaml", UriKind.Relative));
            frm_Browser.NavigationService.RemoveBackEntry();
        }
        #endregion // Events

        // WndProc
        #region WndProc
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {

            switch (msg)
            {
                case Win32.WM_GETMINMAXINFO: // WM_GETMINMAXINFO message  
                    WmGetMinMaxInfo(hwnd, lParam);
                    handled = true;
                    break;

                case Win32.WM_NCHITTEST: // WM_NCHITTEST message  
                    return WmNCHitTest(lParam, ref handled);
            }

            return IntPtr.Zero;
        }

        private void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            // MINMAXINFO structure  
            Win32.MINMAXINFO mmi = (Win32.MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(Win32.MINMAXINFO));

            // Get handle for nearest monitor to this window  
            WindowInteropHelper wih = new WindowInteropHelper(this);
            IntPtr hMonitor = Win32.MonitorFromWindow(wih.Handle, Win32.MONITOR_DEFAULTTONEAREST);

            // Get monitor info  
            Win32.MONITORINFOEX monitorInfo = new Win32.MONITORINFOEX();
            monitorInfo.cbSize = Marshal.SizeOf(monitorInfo);
            Win32.GetMonitorInfo(new HandleRef(this, hMonitor), monitorInfo);

            // Get HwndSource  
            HwndSource source = HwndSource.FromHwnd(wih.Handle);
            if (source == null)
                // Should never be null  
                throw new Exception("Cannot get HwndSource instance.");
            if (source.CompositionTarget == null)
                // Should never be null  
                throw new Exception("Cannot get HwndTarget instance.");

            // Get transformation matrix  
            Matrix matrix = source.CompositionTarget.TransformFromDevice;

            // Convert working area  
            Win32.RECT workingArea = monitorInfo.rcWork;
            Point dpiIndependentSize =
                matrix.Transform(new Point(
                        workingArea.Right - workingArea.Left,
                        workingArea.Bottom - workingArea.Top
                        ));

            // Convert minimum size  
            Point dpiIndenpendentTrackingSize = matrix.Transform(new Point(
                this.MinWidth,
                this.MinHeight
                ));

            // Set the maximized size of the window  
            mmi.ptMaxSize.x = (int)dpiIndependentSize.X;
            mmi.ptMaxSize.y = (int)dpiIndependentSize.Y;

            // Set the position of the maximized window  
            mmi.ptMaxPosition.x = 0;
            mmi.ptMaxPosition.y = 0;

            // Set the minimum tracking size  
            mmi.ptMinTrackSize.x = (int)dpiIndenpendentTrackingSize.X;
            mmi.ptMinTrackSize.y = (int)dpiIndenpendentTrackingSize.Y;

            Marshal.StructureToPtr(mmi, lParam, true);
        }

        private IntPtr WmNCHitTest(IntPtr lParam, ref bool handled)
        {
            if (this.WindowState == WindowState.Maximized)
                return IntPtr.Zero;

            // Update cursor point  
            // The low-order word specifies the x-coordinate of the cursor.  
            // #define GET_X_LPARAM(lp) ((int)(short)LOWORD(lp))  
            this._mousePoint.X = (int)(short)(lParam.ToInt32() & 0xFFFF);
            // The high-order word specifies the y-coordinate of the cursor.  
            // #define GET_Y_LPARAM(lp) ((int)(short)HIWORD(lp))  
            this._mousePoint.Y = (int)(short)(lParam.ToInt32() >> 16);

            // Do hit test  
            handled = false;
            if (Math.Abs(this._mousePoint.Y - this.Top) <= this._cornerWidth
                && Math.Abs(this._mousePoint.X - this.Left) <= this._cornerWidth)
            { // Top-Left
                handled = true;
                return new IntPtr((int)Win32.HitTest.HTTOPLEFT);
            }
            else if (Math.Abs(this.ActualHeight + this.Top - this._mousePoint.Y) <= this._cornerWidth
                && Math.Abs(this._mousePoint.X - this.Left) <= this._cornerWidth)
            { // Bottom-Left  
                handled = true;
                return new IntPtr((int)Win32.HitTest.HTBOTTOMLEFT);
            }
            else if (Math.Abs(this._mousePoint.Y - this.Top) <= this._cornerWidth
                && Math.Abs(this.ActualWidth + this.Left - this._mousePoint.X) <= this._cornerWidth)
            { // Top-Right  
                handled = true;
                return new IntPtr((int)Win32.HitTest.HTTOPRIGHT);
            }
            else if (Math.Abs(this.ActualWidth + this.Left - this._mousePoint.X) <= this._cornerWidth
                && Math.Abs(this.ActualHeight + this.Top - this._mousePoint.Y) <= this._cornerWidth)
            { // Bottom-Right  
                handled = true;
                return new IntPtr((int)Win32.HitTest.HTBOTTOMRIGHT);
            }
            else if (Math.Abs(this._mousePoint.X - this.Left) <= this._customBorderThickness)
            { // Left  
                handled = true;
                return new IntPtr((int)Win32.HitTest.HTLEFT);
            }
            else if (Math.Abs(this.ActualWidth + this.Left - this._mousePoint.X) <= this._customBorderThickness)
            { // Right  
                handled = true;
                return new IntPtr((int)Win32.HitTest.HTRIGHT);
            }
            else if (Math.Abs(this._mousePoint.Y - this.Top) <= this._customBorderThickness)
            { // Top  
                handled = true;
                return new IntPtr((int)Win32.HitTest.HTTOP);
            }
            else if (Math.Abs(this.ActualHeight + this.Top - this._mousePoint.Y) <= this._customBorderThickness)
            { // Bottom  
                handled = true;
                return new IntPtr((int)Win32.HitTest.HTBOTTOM);
            }
            else
            {
                return IntPtr.Zero;
            }
        }

        #endregion // WndProc

        // Front-Layer Operations
        #region Front-Layer Operations

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetClassLong(IntPtr hwnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassLong(IntPtr hwnd, int nIndex);

        private void SetShadow()
        {
            WindowInteropHelper wndHelper = new WindowInteropHelper(this);
            int res = SetClassLong(wndHelper.Handle, GCL_STYLE, GetClassLong(wndHelper.Handle, GCL_STYLE) | CS_DropSHADOW);
        }

        public void ChangedThemeColor(ThemeColor theme)
        {
            Color clr = new Color();
            clr.A = 0xFF;

            switch (theme)
            {
                case ThemeColor.FileDriverTheme:
                    clr.R = 0x4C; clr.G = 0x8B; clr.B = 0xF5;
                    break;

                case ThemeColor.ConnectingTheme:
                    clr.R = 0x75; clr.G = 0xBC; clr.B = 0x75;
                    break;

                case ThemeColor.Teamwork:
                    clr.R = 0xDE; clr.G = 0x51; clr.B = 0x45;
                    break;
            }

            frm_TitleFrame.Background = new SolidColorBrush(clr);
        }

        #endregion // Front-Layer Operations

        // Properties
        #region Properties

        private const int GCL_STYLE = (-26);
        private const int CS_DropSHADOW = 0x20000;


        /// <summary>  
        /// Mouse point used by HitTest  
        /// </summary>  
        private Point _mousePoint = new Point();

        /// <summary>  
        /// Corner width used in HitTest  
        /// </summary>  
        private readonly int _cornerWidth = 16;

        /// <summary>
        /// Cutom Border Thickness, also for topGrid's margin.
        /// </summary>
        private int _customBorderThickness = 15;

        #endregion // Front-Layer Operations

 

        

    }
}
