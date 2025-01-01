using Microsoft.Windows.Themes;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AppShopWFP.Models;

namespace AppShopWFP
{
    class AddProductViewModel : BaseViewModel
    {
        private IEnumerable<PropertyView> _properties;
        public IEnumerable<PropertyView> Properties
        {
            get { return _properties; }
            set { _properties = value;
                OnPropertyChanged(nameof(Properties));
            }
        }
        public Item NewItem = new Item();
        public AddProductViewModel()
        {

            Properties = GetItemProperties();
        }
        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new Command(() =>
                {
                    GetNewItem(NewItem,Properties);// how to save into Main List
                }));
            }
        }
        private ICommand _refreshCommand;
        public ICommand RefreshCommand
        {
            get { return _refreshCommand ??(_refreshCommand = new MvvmHelpers.Commands.Command(() => 
            { 
                Refresh();
            }));
            }
        }
        private Item GetNewItem(Item item,IEnumerable<PropertyView> properties)
        {
            foreach (var property in properties)
            {
                item.GetType().GetProperty(property.PropertyName).SetValue(item, property.PropertyValue);
            }
            return item;
        }
        private void Refresh()
        {
            Properties = GetItemProperties();
            NewItem = new Item();
        }
        private List<PropertyView> GetItemProperties()
        {
            List<PropertyView> properties = new List<PropertyView>();
            foreach (PropertyInfo prop in typeof(Item).GetProperties())
            {
                properties.Add(new PropertyView(prop.Name, prop.GetValue(NewItem, null)));
            }
            return properties;
        }
    }
    public class PropertyView
    {
        public PropertyView(string propertyName, object propertyValue)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }
        public string PropertyName { get; set; }
        public object PropertyValue { get; set; }
    }
}
