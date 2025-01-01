using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AppShopWFP.Data;
using AppShopWFP.Models;
using System.Linq;
using static System.Net.WebRequestMethods;
using System.Configuration;
using System.Windows.Controls.Primitives;

namespace AppShopWFP
{
    class MainWindowViewModel : BaseViewModel
    {
        private Item selectedProduct;
        public Item SelectedProduct
        {
            get { return  selectedProduct; }
            set { selectedProduct = value;
                Properties = GetItemProperties(selectedProduct);
                OnPropertyChanged(nameof(selectedProduct));
            }
        }
        private List<Item> products;
        public List<Item> Products
        {
            get => products;
            set{ 
                products = value;
            OnPropertyChanged(nameof(Products));
            }
        }
        private IEnumerable<PropertyView> _properties;
        public IEnumerable<PropertyView> Properties
        {
            get { return _properties; }
            set
            {
                _properties = value;
                OnPropertyChanged(nameof(Properties));
            }
        }
        private string _connectionString;
        public string ConnectionString
        {
            get { return _connectionString; } 
            set { _connectionString = value;
                OnPropertyChanged(nameof(ConnectionString)); 
                HttpRequest.BaseUrl = value;
            }
        }

        private ICommand _changeConnStrCommand;
        public ICommand ChangeConnStrCommand
        {
            get
            {
                return _changeConnStrCommand ?? (_changeConnStrCommand = new Command((str) => {
                    ConnectionString = str.ToString();
                    LoadData();
                }));
            }
        }
        private ICommand _closeCommand;
        public ICommand CloseCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = new Command(() => {


                    var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
                    connectionStringsSection.ConnectionStrings["DefaultConnectionString"].ConnectionString = HttpRequest.BaseUrl;
                    config.Save();
                    ConfigurationManager.RefreshSection("connectionStrings");

                }));
            }
        }
        private ICommand _refreshDataCommand;
        public ICommand RefreshDataCommand
        {
            get
            {
                return _refreshDataCommand ?? (_refreshDataCommand = new MvvmHelpers.Commands.Command(() => 
                {
                    
                    LoadData();
                }));
            }
        }
        private ICommand _addProduct;
        public ICommand AddProduct
        {
            get {
                return _addProduct ?? (_addProduct = new Command(() => 
                {
                    Item item = new Item();
                    List<Item> products = Products.ToList();
                    products.Add(item);
                    Products = products;
                }));
                    
                }
        }
        private ICommand _deleteSelectedProduct;
        public ICommand DeleteSelectedProduct
        {
            get
            {
                return _deleteSelectedProduct ?? (_deleteSelectedProduct = new Command(() =>
                {
                    if (SelectedProduct != null)
                    {
                        Item item = SelectedProduct as Item ?? null;
                        List<Item> products = Products.ToList();
                        products.Remove(item);
                        Products = products;
                    }
                }));

            }
        }
        private ICommand _changeProductCommand;
        public ICommand ChangeProductCommand
        {
            get
            {
                return _changeProductCommand ?? (_changeProductCommand = new Command(() =>
                {
                    if (SelectedProduct != null)
                    {
                        Item changedItem = SelectedProduct as Item ?? null;
                        int index = Products.FindIndex(p => p.ID == SelectedProduct.ID);
                        foreach(PropertyView propertyView in Properties)
                        {
                            changedItem.GetType().GetProperty(propertyView.PropertyName).SetValue(changedItem, propertyView.PropertyValue);
                        }
                        List<Item> products = Products.ToList();
                        products[index] = changedItem;
                        Products = products;
                    }
                }));

            }
        }
        public MainWindowViewModel()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;
            
            LoadData();
        }

        public void LoadData()
        {
            Thread LoadProducts = new Thread(async () =>
            {
                Properties = GetItemProperties(SelectedProduct);
                Products = await HttpRequest.GetItemsFromWebApi();
                
            });
            LoadProducts.Start();
        }
        private List<PropertyView> GetItemProperties(Item item)
        {
            List<PropertyView> properties = new List<PropertyView>();
            foreach (PropertyInfo prop in typeof(Item).GetProperties())
            {
                if (prop.Name == "ID")
                    continue;
                if (item == null)
                {
                    properties.Add(new PropertyView(prop.Name, null));
                }
                else
                {
                    properties.Add(new PropertyView(prop.Name, prop.GetValue(item, null)));
                }
            }
            return properties;
        }

    }
}
