//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
namespace UILab.Art.Framework.Core.DataAccess.Repository;

public record ReadOperataionResult<T>(ReadOnlyMemory<T> Data, // data read from storage
                                      count ReadCount);       // how many bytes have been read

public record WriteOperataionResult(count WrittenCount);      // how many bytes have been written
