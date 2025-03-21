﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Library.Core.Interfaces;
using Library.Core.Models;
using Library.UI.Commands;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.Runtime.CompilerServices;
using Library.UI.Views;
using System.Windows.Controls;
using System.Windows.Data;
using Library.Core.Enums;
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

        public string LibraryName => $"The books on {Library.Name}:";

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

        private string _bookNameFilterString = string.Empty;
        private string _bookIsbnFilterString = string.Empty;
        private string _bookAuthorsFilterString = string.Empty;
        private string _bookThemesFilterString = string.Empty;
        private string _bookDescriptionFilterString = string.Empty;

        private string _selectedCollection = "";

        public string SelectedCollection
        {
            get
            {
                return _selectedCollection;
            }
            set
            {
                _selectedCollection = value;
                MongoConstants.MongoBooksCollectionName = _selectedCollection;
                _ = LoadElementsAsync();
            }
        }
        public List<string> PossibleCollections { get; set; } = new List<string>();

        /// <summary>
        /// Store the string for filtering by <see cref="IBook"/> name
        /// </summary>
        public string BookNameFilterString
        {
            get
            {
                return _bookNameFilterString;
            }
            set
            {
                _bookNameFilterString = value;
                OnPropertyChanged(nameof(BookNameFilterString));
                UpdateDgrBooks();
            }
        }

        /// <summary>
        /// Store the string for filtering by <see cref="IBook"/> ISBN
        /// </summary>
        public string BookIsbnFilterString
        {
            get
            {
                return _bookIsbnFilterString;
            }
            set
            {
                _bookIsbnFilterString = value;
                OnPropertyChanged(nameof(BookIsbnFilterString));
                UpdateDgrBooks();
            }
        }

        /// <summary>
        /// Store the string for filtering by <see cref="IBook"/> authors
        /// </summary>
        public string BookAuthorsFilterString
        {
            get
            {
                return _bookAuthorsFilterString;
            }
            set
            {
                _bookAuthorsFilterString = value;
                OnPropertyChanged(nameof(BookAuthorsFilterString));
                UpdateDgrBooks();
            }
        }

        /// <summary>
        /// Store the string for filtering by <see cref="IBook"/> themes
        /// </summary>
        public string BookThemesFilterString
        {
            get
            {
                return _bookThemesFilterString;
            }
            set
            {
                _bookThemesFilterString = value;
                OnPropertyChanged(nameof(BookThemesFilterString));
                UpdateDgrBooks();
            }
        }

        /// <summary>
        /// Store the string for filtering by <see cref="IBook"/> description
        /// </summary>
        public string BookDescriptionFilterString
        {
            get
            {
                return _bookDescriptionFilterString;
            }
            set
            {
                _bookDescriptionFilterString = value;
                OnPropertyChanged(nameof(BookDescriptionFilterString));
                UpdateDgrBooks();
            }
        }

        /// <summary>
        /// Creates a new instance of <see cref="LibraryMainViewModel"/>
        /// </summary>
        public LibraryMainViewModel()
        {
            LibraryFactory libraryFactory = new LibraryFactory();
            Library = libraryFactory.CreateLibrary("Mi biblioteca");

            ExceptionManager = new ExceptionManager();

            _mongoService = new MongoService();

            var a = _mongoService.GetPossibleCollections();
            PossibleCollections = a.Result;

            SelectedCollection = PossibleCollections.First();

            _ = LoadElementsAsync();

        }

        /// <summary>
        /// Update the collection of <see cref="IBook"/> in the <see cref="MainWindow"/>, according with its filters
        /// </summary>
        private void UpdateDgrBooks()
        {
            if (MainWindow != null)
            {
                MainWindow.UpdateDgrBooks();
            }
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
                if (itemBook.Themes == null)
                {
                    itemBook.Themes = new List<Theme>();
                }
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

                if (bookWindow.DialogResult == true)
                {
                    await AddBookAsync(vm.Book);
                    _ = LoadElementsAsync();
                }
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
        /// Update a <see cref="IBook"/> from the DB, by its ISBN and the new
        /// <see cref="IBook"/> element.
        /// </summary>
        /// <param name="isbn">The ISBN of the <see cref="IBook"/> to remove</param>
        /// <param name="bookToUpdate">The  <see cref="IBook"/> with the updated values
        /// </param>
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

                if (toBeEdited != null)
                {
                    long isbn = toBeEdited.ISBN;

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

        protected virtual void OnPropertyChanged([CallerMemberName] string?
            propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
