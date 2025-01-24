namespace DragonUtilities.Interfaces;

/// <summary>
/// The FileService provides an abstraction over file I/O operations.
/// It supports reading from and writing to files in a serialized format (JSON) as well as managing file existence.
/// </summary>
public interface IFileService
{
    /// <summary>
    /// Writes serialized content to a file. If the file already exists, it will be overwritten.
    /// </summary>
    /// <typeparam name="T">The type of the content to be serialized and written.</typeparam>
    /// <param name="filePath">The path to the file where the content will be written.</param>
    /// <param name="content">The content to be written to the file.</param>
    void WriteToFile<T>(string filePath, T content);

    /// <summary>
    /// Reads and deserializes content from a file.
    /// </summary>
    /// <typeparam name="T">The type to which the content will be deserialized.</typeparam>
    /// <param name="filePath">The path to the file from which the content will be read.</param>
    /// <returns>The deserialized content of the file.</returns>
    /// <exception cref="FileNotFoundException">Thrown when the file does not exist.</exception>
    T? ReadFromFile<T>(string filePath);

    /// <summary>
    /// Appends serialized content to an existing file. The content will be added on a new line.
    /// </summary>
    /// <typeparam name="T">The type of the content to be serialized and appended.</typeparam>
    /// <param name="filePath">The path to the file where the content will be appended.</param>
    /// <param name="content">The content to be appended to the file.</param>
    void AppendToFile<T>(string filePath, T content);

    /// <summary>
    /// Checks whether a file exists at the specified path.
    /// </summary>
    /// <param name="filePath">The path to the file to check for existence.</param>
    /// <returns>True if the file exists; otherwise, false.</returns>
    bool FileExists(string filePath);

    /// <summary>
    /// Deletes a file at the specified path if it exists.
    /// </summary>
    /// <param name="filePath">The path to the file to be deleted.</param>
    void DeleteFile(string filePath);

    /// <summary>
    /// Reads all lines from a file as an enumerable collection of strings.
    /// </summary>
    /// <param name="filePath">The path to the file from which the lines will be read.</param>
    /// <returns>An enumerable collection of lines from the file.</returns>
    /// <exception cref="FileNotFoundException">Thrown when the file does not exist.</exception>
    IEnumerable<string> ReadLines(string filePath);
}