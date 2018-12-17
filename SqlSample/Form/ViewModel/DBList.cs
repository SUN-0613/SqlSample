using AYam.Common.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Data;

namespace SqlSample.Form.ViewModel
{

    /// <summary>
    /// DBList.ViewModel
    /// </summary>
    public class DBList : VMBase, IDisposable
    {

        #region BindingProperty

        /// <summary>
        /// データベース一覧
        /// </summary>
        public ObservableCollection<Model.DBList.Tree> DataBases { get; set; }

        /// <summary>
        /// TreeView.SelectedItemプロパティ
        /// </summary>
        public Model.DBList.Tree SelectedItem
        {
            get { return _Model.SelectedItem; }
            set
            {

                _Model.SelectedItem = value;
                CallPropertyChanged();

                TableName = _Model.GetSelectedTableName();

            }
        }

        /// <summary>
        /// TreeViewで選択しているテーブル名
        /// </summary>
        public string TableName
        {
            get { return _Model.TableName; }
            set
            {
                if (!_Model.TableName.Equals(value))
                {

                    _Model.TableName = value;
                    CallPropertyChanged();

                    ReadData = _Model.ReadSelectedTable();

                }
            }
        }

        /// <summary>
        /// 選択テーブルのデータ
        /// </summary>
        public DataTable ReadData
        {
            get { return _Model.ReadData; }
            set
            {
                _Model.ReadData = value;
                CallPropertyChanged();
            }
        }

        #endregion

        /// <summary>
        /// DBList.Model
        /// </summary>
        private Model.DBList _Model;

        /// <summary>
        /// new
        /// </summary>
        public DBList()
        {

            // new
            _Model = new Model.DBList();
            DataBases = new ObservableCollection<Model.DBList.Tree>();

            // 一覧追加
            _Model.GetDataBase().ForEach(DataBase => 
            {
                DataBases.Add(DataBase);
            });


        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Dispose()
        {

            _Model.Dispose();
            _Model = null;

            for (int iLoop = 0; iLoop < DataBases.Count; iLoop++)
            {
                DataBases[iLoop].Dispose();
                DataBases[iLoop] = null;
            }
            DataBases.Clear();
            DataBases = null;


        }
    }

}
