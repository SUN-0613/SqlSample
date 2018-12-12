using AYam.Common.ViewModel;
using System;
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
