using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotVVM.BusinessPack.Controls;
using DotVVM.Framework.Controls;
using DotVVM.Framework.ViewModel;


namespace DotvvmWeb.Views.Docs.Controls.businesspack.GridView.sample7
{
    public class ViewModel : DotvvmViewModelBase
    {
        public bool IsInserting { get; set; }
        public BusinessPackDataSet<Customer> Customers { get; set; } = new BusinessPackDataSet<Customer>()
        {
            SortingOptions = {SortExpression = nameof(Customer.Id)},
            RowEditOptions = {PrimaryKeyPropertyName = nameof(Customer.Id)}
        };
        public override Task Init()
        {
            if(Customers.IsRefreshRequired)
            {
                Customers.LoadFromQueryable(GetQueryable(15));
            }
            return base.Init();
        }

        public void InsertNewCustomer()
        {
            Customers.RowInsertOptions.InsertedItem = new Customer {
                Id = Customers.Items.Max(c => c.Id) + 1,
                Orders = 0
            };
            IsInserting = true;
        }

        public void CancelInsertNewCustomer()
        {
            Customers.RowInsertOptions.InsertedItem = null;
            IsInserting = false;
        }

        public void SaveNewCustomer()
        {
            // Save inserted item to database
            Customers.Items.Add(Customers.RowInsertOptions.InsertedItem);
            CancelInsertNewCustomer();
        }

        private IQueryable<Customer> GetQueryable(int size)
        {
            var customers = new List<Customer>();
            for (var i = 0; i < size; i++)
            {
                customers.Add(new Customer { Id = i + 1, Name = $"Customer {i + 1}", BirthDate = DateTime.Now.AddYears(-i), Orders = i });
            }
            return customers.AsQueryable();
        }
    }
}