﻿namespace Sales.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Common.Models;
    using GalaSoft.MvvmLight.Command;
    using Sales.Helpers;
    using Services;
    using Xamarin.Forms;

    public class ProductsViewModel : BaseViewModel
    {

        #region Atributo
        private ApiService apiService;
        private bool isRefreshing;
        private ObservableCollection<Products> products;
        #endregion

        #region Propiedad
        public ObservableCollection<Products> Products
        {
            get { return this.products; }
            set { this.SetValue(ref this.products, value); }
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }
        #endregion

        public ProductsViewModel()
        {
            this.apiService = new ApiService();
            this.LoadProducts();
        }

        private async void LoadProducts()
        {
            this.IsRefreshing = true;
            // Chequeo de la Conexion a Internet 
            var conection = await this.apiService.CheckConnection();
            if (!conection.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    conection.Message,
                    Languages.Accept);
                return;
            }

            var url = Application.Current.Resources["UrlAPI"].ToString(); // lo saco del diccionario de Recursos
            var prefix = Application.Current.Resources["UrlPrefix"].ToString(); // lo saco del diccionario de Recursos
            var controller = Application.Current.Resources["UrlProductsController"].ToString(); // lo saco del diccionario de Recursos
            var response = await this.apiService.GetList<Products>(
                url, 
                prefix,
                controller);
            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Accept);
                return;
            }

            var list = (List < Products >) response.Result;
            this.Products = new ObservableCollection<Products>(list);
            this.IsRefreshing = false;
        }

        #region Command
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadProducts);
            }
        }
        #endregion
    }
}
