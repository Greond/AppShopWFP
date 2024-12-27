﻿using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppTEST.Data;
using WpfAppTEST.Models;

namespace WpfAppTEST
{
    class MainWindowViewModel : BaseViewModel
    {
        private Item selectedProduct;
        public Item SelectedProduct
        {
            get { return  selectedProduct; }
            set { selectedProduct = value;
                OnPropertyChanged(nameof(selectedProduct));
            }
        }
        private IEnumerable<Item> products;
        public IEnumerable<Item> Products
        {
            get => products;
            set{ 
                products = value;
            OnPropertyChanged(nameof(Products));
            }
        }

        public MainWindowViewModel()
        {
            LoadData();
        }

        public async void LoadData()
        {
            Thread LoadProducts = new Thread(async () =>
            {
                Products = await HttpRequest.GetItemsFromWebApi();
            });
            LoadProducts.Start();
        }

    }
}