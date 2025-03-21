﻿using Library.Core.Interfaces;
using Library.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Library.Core.Models;

namespace Library.UI.Views
{
    /// <summary>
    /// Interaction logic for BookWindow.xaml
    /// </summary>
    public partial class BookWindow : Window
    {
        private BookWindowViewModel _viewModel;

        /// <summary>
        /// Constructor for <see cref="BookWindow"/>, using a ViewModel
        /// </summary>
        /// <param name="bwvn">The ViewModel for this window</param>
        public BookWindow()
        {
            InitializeComponent();

            _viewModel = this.DataContext as BookWindowViewModel;
            _viewModel.BookWindow = this;

            this.Loaded += OnLoaded;
        }

        /// <summary>
        /// When the UI is loaded, update the values of the selected <see cref="IBook"/>
        /// on the controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel.UpdateBookValues();
        }

        /// <summary>
        /// Set the <see cref="IBook"/> field of the ViewModel as the
        /// <see cref="IBook"/> that the user input here.
        /// </summary>
        /// <param name="book"></param>
        internal void SetBook(IBook book)
        {
            _viewModel.Book = book;
            _viewModel.UpdateBookValues();
        }
    }
}
