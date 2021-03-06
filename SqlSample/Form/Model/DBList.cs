﻿using AYam.Common.DB;
using System;
using System.Collections.Generic;
using System.Data;
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
            /// 自身の親
            /// </summary>
            public Tree Parent;

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
                Parent = null;
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
        /// TreeView.SelectedItem
        /// </summary>
        public Tree SelectedItem = null;

        /// <summary>
        /// TreeViewで選択しているテーブル名
        /// </summary>
        public string TableName = "";

        /// <summary>
        /// TreeViewでテーブルを選択しているか
        /// </summary>
        public bool IsSelectedTable = false;

        /// <summary>
        /// 選択テーブルのデータ
        /// </summary>
        private DataTable _ReadData;

        /// <summary>
        /// 選択テーブルのデータプロパティ
        /// </summary>
        public DataTable ReadData
        {
            get { return _ReadData; }
            set
            {

                // 初期化
                if (_ReadData != null)
                {
                    _ReadData.Dispose();
                    _ReadData = null;
                }

                _ReadData = value;

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

                        // データ呼出
                        while (dataReader.Read())
                        {
                            DataBase.Children.Add(new Tree()
                            {
                                Name = dataReader.GetString(0),
                                Parent = DataBase
                            });
                        }
                        
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

        /// <summary>
        /// TreeViewで選択しているテーブル名を取得
        /// </summary>
        /// <returns>Database名.Table名</returns>
        public string GetSelectedTableName()
        {

            string tableName = "";
            Tree selectedItem = SelectedItem;

            IsSelectedTable = false;

            while (selectedItem != null)
            {

                if (!tableName.Length.Equals(0))
                {
                    tableName = @".dbo." + tableName;
                    IsSelectedTable = true;
                }
                tableName = selectedItem.Name + tableName;

                selectedItem = selectedItem.Parent;

            }

            return tableName;

        }

        /// <summary>
        /// TreeViewで選択しているテーブルのデータを取得
        /// </summary>
        /// <returns>テーブルデータ一覧</returns>
        public DataTable ReadSelectedTable()
        {

            StringBuilder query = new StringBuilder(128);
            DataTable ReturnData = null;

            try
            {

                if (!TableName.Length.Equals(0))
                {

                    // テーブルデータ取得クエリ作成
                    query.Append(@"SELECT * FROM ").Append(TableName);

                    // クエリ実行
                    ReturnData = _SqlServer.ExecuteNonQuery(query.ToString());

                }

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

            return ReturnData;

        }

    }

}
