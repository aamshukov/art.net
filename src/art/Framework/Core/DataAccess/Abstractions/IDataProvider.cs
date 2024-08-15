//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using UILab.Art.Framework.Core.Content.Abstractions;

namespace UILab.Art.Framework.Core.DataAccess.Abstractions;

public interface IDataProvider<T>
{
    void Load(IContent<T> content);

    Task LoadAsync(IContent<T> content, CancellationToken cancellationToken);
}
