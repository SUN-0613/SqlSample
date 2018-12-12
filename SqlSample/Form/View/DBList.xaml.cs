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
using System.Windows.Shapes;

namespace SqlSample.Form.View
{
    /// <summary>
    /// DBList.xaml の相互作用ロジック
    /// </summary>
    public partial class DBList : Window, IDisposable
    {

        /// <summary>
        /// DBList.ViewModel
        /// </summary>
        private ViewModel.DBList _ViewModel;

        /// <summary>
        /// new
        /// </summary>
        public DBList()
        {

            InitializeComponent();

            _ViewModel = new ViewModel.DBList();
            DataContext = _ViewModel;

        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Dispose()
        {

            _ViewModel.Dispose();
            _ViewModel = null;

        }

    }
}
