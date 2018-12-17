using System;
using System.Windows;
using System.Windows.Controls;

namespace SqlSample.Custom
{

    /// <summary>
    /// TreeView.Bindable.SelectedItem
    /// </summary>
    public class TreeViewEx : TreeView, IDisposable
    {

        /// <summary>
        /// プロパティ定義
        /// </summary>
        public static readonly DependencyProperty SelectedItemExProperty 
            = DependencyProperty.Register(nameof(SelectedItemEx)
                , typeof(object), typeof(TreeViewEx), new UIPropertyMetadata(null));

        /// <summary>
        /// 選択データプロパティ
        /// </summary>
        public object SelectedItemEx
        {
            get { return (object)GetValue(SelectedItemExProperty); }
            set { SetValue(SelectedItemExProperty, value); }
        }

        /// <summary>
        /// new
        /// </summary>
        public TreeViewEx()
        {
            SelectedItemChanged += OnSelectedItemChanged;
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Dispose()
        {
            SelectedItemChanged -= OnSelectedItemChanged;
        }

        /// <summary>
        /// 選択データ変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (SelectedItem == null)
            {
                return;
            }

            SetValue(SelectedItemExProperty, SelectedItem);

        }

    }

}
