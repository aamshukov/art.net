//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
namespace UILab.Art.Framework.Core.DataAccess.DataProvider.Abstractions;

public interface IFileDataProvider<T> : IDataProvider<T>
{
    string FileName { get; }
}
