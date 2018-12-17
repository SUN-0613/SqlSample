using AYam.Common.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                }
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
