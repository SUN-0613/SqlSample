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

            _Model = new Model.DBList();

            DataBases = new ObservableCollection<Model.DBList.Tree>();

        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Dispose()
        {

            _Model.Dispose();
            _Model = null;

        }
    }

}
