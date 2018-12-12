using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;

namespace SqlSample
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {

        /// <summary>
        /// 処理開始
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {

            //実行ファイル名取得
            string appName = Assembly.GetExecutingAssembly().GetName().Name;
            appName = Path.GetFileNameWithoutExtension(appName);

            //多重起動確認
            if (new Mutex(false, appName).WaitOne(0, false))
            {

                //基本処理
                base.OnStartup(e);

                //画面表示
                new Form.View.DBList().ShowDialog();

            }
            else
            {

                //多重起動時はメッセージ表示後、終了
                MessageBox.Show(SqlSample.Properties.Resources.App_MutexMessage, appName, MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();

            }

        }

    }

}
