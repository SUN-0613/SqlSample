using AYam.Common.DB;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using SqlSample.Properties;

namespace SqlSample.Form.Model
{

    /// <summary>
    /// DBList.Model
    /// </summary>
    public class DBList : IDisposable
    {

        /// <summary>
        /// データベース
        /// </summary>
        public class Tree : IDisposable
        {

            /// <summary>
            /// 名称
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// テーブル一覧
            /// </summary>
            public List<Tree> Children { get; set; }

            /// <summary>
            /// new
            /// </summary>
            public Tree()
            {
                Children = new List<Tree>();
            }

            /// <summary>
            /// 終了処理
            /// </summary>
            public void Dispose()
            {

                Children.ForEach(Child =>
                {
                    Child.Dispose();
                    Child = null;
                });

                Children.Clear();
                Children = null;

            }
        }

        /// <summary>
        /// SQL Server 接続クラス
        /// </summary>
        private SqlServer _SqlServer;

        /// <summary>
        /// データベース一覧
        /// </summary>
        private List<Tree> _DataBases;

        /// <summary>
        /// new
        /// </summary>
        public DBList()
        {

            // DB接続
            _SqlServer = new SqlServer(Settings.Default.ServerName, Settings.Default.DbName);


        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Dispose()
        {

            // List解放
            _DataBases.ForEach(DataBase => 
            {
                DataBase.Dispose();
                DataBase = null;
            });
            _DataBases.Clear();
            _DataBases = null;

            // DB切断
            _SqlServer.Dispose();
            _SqlServer = null;

        }

        /// <summary>
        /// データベース・テーブル一覧取得
        /// </summary>
        /// <returns>テーブル一覧</returns>
        public List<Tree> GetDataBase()
        {

            StringBuilder query = new StringBuilder(128);
            
            try
            {

                // 初期化
                if (_DataBases == null)
                {
                    _DataBases = new List<Tree>();
                }

                _DataBases.Clear();

                // データベース一覧取得クエリ作成
                query.Append(@"SELECT name FROM SYS.DATABASES ORDER BY database_id");

                // クエリ実行
                using (SqlDataReader dataReader = _SqlServer.ExecuteQuery(query.ToString()))
                {

                    // データ呼出
                    while (dataReader.Read())
                    {
                        _DataBases.Add(new Tree() { Name = dataReader.GetString(0) });
                    }

                }

                // データベース毎にテーブル一覧を取得する
                _DataBases.ForEach(DataBase => 
                {

                    // テーブル一覧取得クエリ作成
                    // ユーザテーブルのみを対象とする
                    query.Clear()
                        .Append(@"USE ").Append(DataBase.Name)
                        .Append(@" SELECT name FROM sys.objects WHERE type = 'U'");

                    // クエリ実行
                    using (SqlDataReader dataReader = _SqlServer.ExecuteQuery(query.ToString()))
                    {
                        DataBase.Children.Add(new Tree() { Name = dataReader.GetString(0) });
                    }

                });

            }
            catch (Exception ex)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine(ex.Message);
#endif
            }
            finally
            {

                query.Clear();
                query = null;

            }

            return _DataBases;

        }

    }

}
