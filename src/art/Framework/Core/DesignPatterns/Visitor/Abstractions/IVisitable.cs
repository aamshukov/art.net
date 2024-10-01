//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
namespace UILab.Art.Framework.Core.DesignPatterns.Visitor.Abstractions;

public interface IVisitable
{
    TResult? Accept<TParam, TResult>(IVisitor visitor, TParam? param = default);
}
