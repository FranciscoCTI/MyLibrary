using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Library.Core.Interfaces;
using Library.Core.Models;
using Library.UI.Commands;
using System.ComponentModel;
using System.Diagnostics;
using Library.UI.Views;
using System.Windows.Controls;
using Library.Core.Factories;
using Library.Global;
using Library.Services;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Library.UI.ViewModels
{
    /// <summary>
    /// ViewModel for the <see cref="ILibrary"/>> UI. Which shows the
    /// <see cref="IBook"/>s of the current <see cref="ILibrary"/>.
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

        public string LibraryName => $"These are the books on {Library.Name}";

        /// <summary>
        /// The current <see cref="MainWindow"/> UI
        /// </summary>
        public LibraryWindow MainWindow { get; set; }

        /// <summary>
        /// Store the current <see cref="Services.ExceptionManager"/>.
        /// Which manages the exceptions and logs them in a persistent record.
        /// </summary>
        private ExceptionManager ExceptionManager { get;}


        /// <summary>
        /// Manage all the MongoDB processes
        /// </summary>
        private readonly MongoService _mongoService;


        /// <summary>
        /// Creates a new instance of <see cref="LibraryMainViewModel"/>
        /// </summary>
        public LibraryMainViewModel()
        {
            LibraryFactory libraryFactory = new LibraryFactory();
            Library = libraryFactory.CreateLibrary("Mi biblioteca");

            ExceptionManager = new ExceptionManager();

            _mongoService = new MongoService();

            _ = LoadElementsAsync();
        }

        /// <summary>
        /// Load the current state of the <see cref="ILibrary"/> database
        /// </summary>
        /// <returns></returns>
        private async Task LoadElementsAsync()
        {
            var bookListFromDb = _mongoService.GetBooksAsync(MongoConstants.MongoBooksCollectionName);

            var elementList = await bookListFromDb;
            var list = elementList.ToList<IBook>();

            Library.BookCollection.Clear();

            foreach (var itemBook in list)
            {
                Library.BookCollection.Add(itemBook);
            }
        }

        /// <summary>
        /// Launch a <see cref="BookWindow"/> form to allow the user to input the
        /// information of a <see cref="IBook"/>. Then on pressed "Accept", the
        /// <see cref="IBook"/> is inserted into the current <see cref="ILibrary"/>
        /// If there is any error during the process the
        /// <see cref="Services.ExceptionManager"/> Handles it.
        /// </summary>
        private async void AddBook()
        {
            try
            {
                MainWindow.SetBookWindow();
                MainWindow.BookWindow.ShowDialog();

                var bookWindow = MainWindow.GetBookWindow(); 
                    
                BookWindowViewModel vm = bookWindow.DataContext as BookWindowViewModel;

                await AddBookAsync(vm.Book);

                _ = LoadElementsAsync();

            }
            catch (Exception e)
            {
                ExceptionManager.HandleException(e, "Add Book");
            }
        }

        /// <summary>
        /// Adds a new <see cref="IBook"/>> to the library.
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        private async Task AddBookAsync(IBook book)
        {
            await _mongoService.AddBookAsync((Book)book);
        }

        /// <summary>
        /// Removes the <see cref="IBook"/> selected on the <see cref="DataGrid"/>
        /// from the current <see cref="ILibrary"/>. If there is any error during
        /// the process the <see cref="Services.ExceptionManager"/>
        /// handles it.
        /// </summary>
        private async void RemoveBook()
        {
            try
            {
                var toBeRemoved = GetSelectedItem();
                if (toBeRemoved != null)
                {
                    IBook book = (IBook)toBeRemoved;
                    await RemoveBookAsync(book.ISBN);
                    _ = LoadElementsAsync();
                }
            }
            catch (Exception e)
            {
                ExceptionManager.HandleException(e, "Remove Book");
            }
        }

        /// <summary>
        /// Remove a <see cref="IBook"/> from the DB, by its ISBN
        /// </summary>
        /// <param name="isbn">The ISBN of the <see cref="IBook"/> to remove</param>
        private async Task RemoveBookAsync(long isbn)
        {
            await _mongoService.RemoveBookAsync(isbn);
        }

        /// <summary>
        /// Update a <see cref="IBook"/> from the DB, by its ISBN and the new <see cref="IBook"/> element.
        /// </summary>
        /// <param name="isbn">The ISBN of the <see cref="IBook"/> to remove</param>
        /// <param name="bookToUpdate">The  <see cref="IBook"/> with the updated values</param>
        private async Task UpdateBookAsync(long isbn, IBook bookToUpdate)
        {
            await _mongoService.UpdateBookAsync(isbn, bookToUpdate);
        }

        /// <summary>
        /// Edit the <see cref="IBook"/> selected on the <see cref="DataGrid"/>.
        /// Launch a <see cref="BookWindow"/> form to change the data.
        /// When pressed "Accept" the <see cref="IBook"/> is updated in the
        /// <see cref="ILibrary"/>. If there is any error during
        /// the process the <see cref="Services.ExceptionManager"/>
        /// handles it.
        /// </summary>
        private async void EditBook()
        {
            try
            {
                MainWindow.SetBookWindow();

                BookWindow bookWindow = MainWindow.GetBookWindow();

                var toBeEdited = (IBook)MainWindow.GetSelectedItem();

                long isbn = toBeEdited.ISBN;

                if (toBeEdited != null)
                {
                    bookWindow.SetBook(toBeEdited);
                    bookWindow.ShowDialog();

                    if (bookWindow.DialogResult== true)
                    {
                        await UpdateBookAsync(isbn, toBeEdited);

                        _ = LoadElementsAsync();
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionManager.HandleException(e, "Edit Book");
            }
        }

        /// <summary>
        /// Gets the selected  <see cref="IBook"/> from the GridView
        /// </summary>
        /// <returns></returns>
        public IBook? GetSelectedItem()
        {
            return MainWindow.GetSelectedItem();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
