using System;
using System.Collections.Generic;
using System.Linq;
using Library.Core.Interfaces;
using Library.Core.Models;
using Library.UI.Commands;
using System.ComponentModel;
using Library.UI.Views;
using Services;
using System.Windows.Controls;
using Library.Infrastructure.Services;

namespace Library.UI.ViewModels
{
    /// <summary>
    /// ViewModel for the <see cref="ILibrary"/>> UI. Which shows the <see cref="IBook"/>s
    /// of the current <see cref="ILibrary"/>.
    /// </summary>
    internal class LibraryMainViewModel:INotifyPropertyChanged
    {
        /// <summary>
        /// Command to add a new <see cref="IBook"/> to the <see cref="ILibrary"/>.
        /// Launch a new <see cref="BookWindow"/> to input the data.
        /// </summary>
        public RelayCommand AddBookCommand => new(execute => AddBook());

        /// <summary>
        /// Command to remove the selected <see cref="IBook"/> 
        /// </summary>
        public RelayCommand RemoveBookCommand => new(execute => RemoveBook());

        /// <summary>
        /// Command to edit the selected <see cref="IBook"/>.
        /// Launch a new <see cref="BookWindow"/> to edit the data.
        /// </summary>
        public RelayCommand EditBookCommand => new(execute => EditBook());

        /// <summary>
        /// The current <see cref="ILibrary"/> being shown in the UI
        /// </summary>
        public ILibrary Library { get; set; }

        public string LibraryName => $"This are the books on {Library.Name}";

        /// <summary>
        /// The current <see cref="MainWindow"/> UI
        /// </summary>
        public Views.LibraryWindow MainWindow { get; set; }

        /// <summary>
        /// Store the current <see cref="Library.Infrastructure.Services.ExceptionManager"/>.
        /// Which manages the exceptions and logs them in a persistent record.
        /// </summary>
        private ExceptionManager ExceptionManager { get;}

        /// <summary>
        /// Creates a new instance of <see cref="LibraryMainViewModel"/>
        /// </summary>
        public LibraryMainViewModel()
        {
            Library = new Core.Models.Library("Mi biblioteca");

            ExceptionManager = new ExceptionManager();
        }

        /// <summary>
        /// Launch a <see cref="BookWindow"/> form to allow the user to input the
        /// information of a <see cref="IBook"/>. Then on pressed "Accept", the
        /// <see cref="IBook"/> is inserted into the current <see cref="ILibrary"/>
        /// If there is any error during the process the
        /// <see cref="Library.Infrastructure.Services.ExceptionManager"/> Handles it.
        /// </summary>
        private void AddBook()
        {
            try
            {
                MainWindow.SetBookWindow();
                MainWindow.BookWindow.ShowDialog();

                var bookWindow = MainWindow.GetBookWindow(); 
                    
                BookWindowViewModel vm = bookWindow.DataContext as BookWindowViewModel;

                Library.InsertBook(vm.Book);
            }
            catch (Exception e)
            {
                ExceptionManager.HandleException(e, "Add Book");
            }
        }

        /// <summary>
        /// Removes the <see cref="IBook"/> selected on the <see cref="DataGrid"/>
        /// from the current <see cref="ILibrary"/>. If there is any error during
        /// the process the <see cref="Library.Infrastructure.Services.ExceptionManager"/>
        /// handles it.
        /// </summary>
        private void RemoveBook()
        {
            try
            {
                var toBeRemoved = GetSelectedItem();
                if (toBeRemoved != null)
                {
                    IBook book = (IBook)toBeRemoved;
                    Library.RemoveBook(book.ISBN);
                }
            }
            catch (Exception e)
            {
                ExceptionManager.HandleException(e, "Remove Book");
            }
        }

        /// <summary>
        /// Edit the <see cref="IBook"/> selected on the <see cref="DataGrid"/>.
        /// Launch a <see cref="BookWindow"/> form to change the data.
        /// When pressed "Accept" the <see cref="IBook"/> is updated in the
        /// <see cref="ILibrary"/>. If there is any error during
        /// the process the <see cref="Library.Infrastructure.Services.ExceptionManager"/>
        /// handles it.
        /// </summary>
        private void EditBook()
        {
            try
            {
                MainWindow.SetBookWindow();

                BookWindow bookWindow = MainWindow.GetBookWindow();

                var toBeEdited = (IBook)MainWindow.GetSelectedItem();
                if (toBeEdited != null)
                {
                    bookWindow.SetBook(toBeEdited);
                    bookWindow.ShowDialog();
                    Library.UpdateBook(toBeEdited);
                }
            }
            catch (Exception e)
            {
                ExceptionManager.HandleException(e, "Edit Book");
            }
        }

        public IBook? GetSelectedItem()
        {
            return MainWindow.GetSelectedItem();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
