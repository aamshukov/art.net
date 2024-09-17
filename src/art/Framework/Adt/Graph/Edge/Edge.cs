//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
//using UILab.Art.Framework.Core.Domain;

//namespace UILab.Art.Framework.Adt.Graph;

//public class Edge<TVertex, TValue> : HyperEdge<TVertex>
//    where TVertex : EntityType<id>
//{
//    public static readonly Edge<TVertex, TValue> Sentinel = new(0, "Edge:Sentinel");

//    /// <summary>
//    /// Arbitrary value, but in some cases might be 'weight'.
//    /// </summary>
//    public TValue? Value { get; init; }

//    public Edge(id id,
//                string? label = default,
//                TValue? value = default,
//                Flags flags = Flags.Clear,
//                Dictionary<string, object>? attributes = default,
//                string? version = default) : base(id: id, label: label, flags: flags, attributes: attributes, version: version)
//    {
//        Value = value;
//    }

//    /// <summary>
//    /// Gets the first element of the domain, mimics graph's endpoint.
//    /// </summary>
//    public TVertex? U
//    {
//        get
//        {
//            TVertex? u = default;

//            if(Direction == Direction.Undirectional)
//            {
//                if(Domain.Count > 0)
//                    u = Domain[0];
//            }
//            else if(Direction == Direction.Directional || Direction == Direction.Bidirectional)
//            {
//                if(Domain.Count > 0)
//                    u = Domain[0];
//            }

//            return u;
//        }
//    }

//    /// <summary>
//    /// Gets the first element of the tail, mimics graph's endpoint.
//    /// </summary>
//    public TVertex? V
//    {
//        get
//        {
//            TVertex? v = default;

//            if(Direction == Direction.Undirectional)
//            {
//                if(Domain.Count > 1)
//                    v = Domain[1];
//            }
//            else if(Direction == Direction.Directional || Direction == Direction.Bidirectional)
//            {
//                if(Codomain.Count > 0)
//                    v = Codomain[0];
//            }

//            return v;
//        }
//    }

//    public override IEnumerable<object> GetEqualityComponents()
//    {
//        foreach(var component in base.GetEqualityComponents())
//            yield return component;
//        yield return Label;
//        if(Value is not null)
//            yield return Value;
//    }

//    //public override TResult? Accept<TParam, TResult>(IVisitor<TParam, TResult> visitor, TParam? param = default)
//    //{
//    //    Assert.NonNullReference(visitor, nameof(visitor));
//    //    return visitor.Visit(this, param);
//    //}
//}
