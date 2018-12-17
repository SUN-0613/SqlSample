using System;
using System.Windows;

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
