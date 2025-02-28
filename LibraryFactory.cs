using System;

public class LibraryFactory
{
	public LibraryFactory()
	{
	}

    public ILibrary CreateLibrary()
    {
        return new Library("Los libros de mi casa");
    }
}
