using MVVMSample.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApplication1.ViewModels
{
    class DemoViewModel : INotifyPropertyChanged
    {
        private readonly Models.Demo domObject;
        private readonly ICommand upCmd;

        public DemoViewModel()
        {
            domObject = new Models.Demo();
            upCmd = new RelayCommand(Up, CanUp);
        }

        public int Number
        {
            get { return domObject.Number; }
            set
            {
                domObject.Number = value;
                RaisePropertyChanged(() => this.Number);
            }
        }

        private bool CanUp(object obj)
        {
            if(Number<5)
                return true;
            return false;
        }

        private void Up(object obj)
        {
            Number += 1;
        }

        public ICommand UpCmd { get { return upCmd; } }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            //Fire the PropertyChanged event in case somebody subscribed to it
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        protected void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression.Body.NodeType == ExpressionType.MemberAccess)
            {
                var memberExpr = propertyExpression.Body as MemberExpression;
                string propertyName = memberExpr.Member.Name;
                this.RaisePropertyChanged(propertyName);
            }
        }
    }
}
