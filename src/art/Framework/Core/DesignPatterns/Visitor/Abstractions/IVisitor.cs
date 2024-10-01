//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
namespace UILab.Art.Framework.Core.DesignPatterns.Visitor.Abstractions;

public interface IVisitor
{
    TResult? Visit<TParam, TResult>(IVisitable visitable, TParam? param = default);
}
