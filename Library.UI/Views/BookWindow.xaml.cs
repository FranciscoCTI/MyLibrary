using Library.Core.Interfaces;
using Library.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Library.UI.Views
{
    /// <summary>
    /// Interaction logic for BookWindow.xaml
    /// </summary>
    public partial class BookWindow : Window
    {
        private BookWindowViewModel _viewModel;
        public BookWindow()
        {
            InitializeComponent();

            _viewModel = DataContext as BookWindowViewModel;
            _viewModel.BookWindow = this;
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
